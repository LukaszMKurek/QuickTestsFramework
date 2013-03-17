using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace QuickTestsFramework
{
    public static class One
    {
        public static T Of<T>(params T[] a)
        {
            throw new InvalidOperationException("Zgodnie z projektem metoda ta nie może być nigdy wywołana.");
        }
    }

    public struct ParameterSpec
    {
        public string ParameterName { get; private set; }
        public IEnumerable PossibleValues { get; private set; }

        public ParameterSpec(string parameterName, IEnumerable possibleValues)
            : this()
        {
            ParameterName = parameterName;
            PossibleValues = possibleValues;
        }
    }

    public interface IValueGetter
    {
        T GetValue<T>(string key);
    }

    public interface ICombinationarSequecnceGenerator
    {
        IEnumerable<IValueGetter> Sequence(IEnumerable<ParameterSpec> parametersSpec, int order);
    }

    public sealed class ObjectGenerator
    {
        private readonly ICombinationarSequecnceGenerator _generator;

        public ObjectGenerator(ICombinationarSequecnceGenerator generator)
        {
            _generator = generator;
        }

        public IEnumerable<T> Pairwise<T>(Expression<Func<int, T>> generator, int order = 2)
        {
            int nn = 0;
            Func<MethodCallExpression, string> parameterNameGenerator =
                _ => (nn++).ToString(CultureInfo.InvariantCulture);
            return Gen(generator, parameterNameGenerator, order);
        }

        public IEnumerable<T> Pairwise2<T>(Expression<Func<int, T>> generator, int order = 2)
        {
            var newExpr = generator.Body as MemberInitExpression; // todo null??
            var dict = FindAllProperUsePlaceHolderMethod(newExpr);

            Func<MethodCallExpression, string> parameterNameGenerator =
                methodCallExpression => dict[methodCallExpression];
            return Gen(generator, parameterNameGenerator, order);
        }

        private IEnumerable<T> Gen<T>(
            Expression<Func<int, T>> generator,
            Func<MethodCallExpression, string> parameterNameGenerator,
            int order)
        {
            var valueGetterParameterExpression = Expression.Parameter(typeof(IValueGetter), "valueGetter");
            var data = new List<ParameterSpec>();
            var testExpressionVisitor = new ObjectGeneratorExpressionVisitor(
                valueGetterParameterExpression,
                parameterNameGenerator,
                (paramName, possibleValues) => data.Add(new ParameterSpec(paramName, possibleValues)));

            var expression = (Expression<Func<int, T>>)testExpressionVisitor.Visit(generator);
            var mapperExpr = Expression.Lambda<Func<int, IValueGetter, T>>(
                expression.Body,
                expression.Parameters.Concat(new[] { valueGetterParameterExpression }));

            Func<int, IValueGetter, T> mapper = mapperExpr.Compile();

            return _generator.Sequence(data, order).
                              Select((valueGetter, n) => mapper(n, valueGetter));
        }

        private static Dictionary<MethodCallExpression, string> FindAllProperUsePlaceHolderMethod(MemberInitExpression newExpr)
        {
            var dict = new Dictionary<MethodCallExpression, string>();
            FindAllProperUsePlaceHolderMethod(newExpr, "/", dict);
            return dict;
        }

        private static void FindAllProperUsePlaceHolderMethod(
            MemberInitExpression memberInitExpression,
            string prefix,
            Dictionary<MethodCallExpression, string> dict)
        {
            var memberAssignsWithCall = memberInitExpression.Bindings.
                Cast<MemberAssignment>().
                Where(i => i.Expression.NodeType == ExpressionType.Call);
            foreach (var memberAssignment in memberAssignsWithCall)
                dict.Add(
                    (MethodCallExpression)memberAssignment.Expression,
                    prefix + "/" + memberAssignment.Member.Name);

            var memberAssignsWithNew = memberInitExpression.Bindings.
                Cast<MemberAssignment>().
                Where(i => i.Expression.NodeType == ExpressionType.MemberInit);
            foreach (var memberAssignment in memberAssignsWithNew)
                FindAllProperUsePlaceHolderMethod(
                    (MemberInitExpression)memberAssignment.Expression,
                    prefix + "/" + memberAssignment.Member.Name,
                    dict);
        }
    }

    internal sealed class ObjectGeneratorExpressionVisitor : ExpressionVisitor
    {
        private readonly ParameterExpression _valueGetterParameterExpression;
        private readonly Action<string, IEnumerable> _onFoundDataSource;
        private readonly Func<MethodCallExpression, string> _parameterNameGenerator;

        public ObjectGeneratorExpressionVisitor(
            ParameterExpression valueGetterParameterExpression,
            Func<MethodCallExpression, string> parameterNameGenerator,
            Action<string, IEnumerable> onFoundDataSource)
        {
            _valueGetterParameterExpression = valueGetterParameterExpression;
            _onFoundDataSource = onFoundDataSource;
            _parameterNameGenerator = parameterNameGenerator;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var possibleValues = (IEnumerable)Expression.Lambda<Func<object>>(node.Arguments[0]).Compile()();

            string parameterName = _parameterNameGenerator(node);
            _onFoundDataSource(parameterName, possibleValues);

            var parameterNameConstant = Expression.Constant(parameterName);

            MethodCallExpression callValueGetterExpression = Expression.Call(
                _valueGetterParameterExpression,
                "GetValue",
                new[] { node.Arguments[0].Type.GetElementType() },
                new Expression[] { parameterNameConstant });

            return callValueGetterExpression;
        }
    }
}
