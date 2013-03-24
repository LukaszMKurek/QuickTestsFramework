using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace QuickTestsFramework.Utils
{

    /*
     ResultValidator.
        ForInputData(results).
        TestFunction(a => a.p1 + a.p2.p1).
        WithSpecyfication(spec => spec.
            When(_ => _.p1 == 1 && _.p2.p1 == 1).
                ThenResultIs(2).                   // wersja z delegatą; ThenInneSpecyfication
            When(_ => _.p1 == 2 && _.p2.p1 == 1).
                ThenResultIs(3).
            When(_ => _.p1 == 3 && _.p2.p1 == 1).
                ThenResultIs(3).
            ForRestResultIs(0)
        );
     */

    public sealed class ConditionPathStatistics
    {
        private readonly Dictionary<Expression, ConditionExpressionStatistics> _stats =
            new Dictionary<Expression, ConditionExpressionStatistics>();

        public int TotalPathCount
        {
            get { return _stats.Keys.Count * 2; }
        }

        public int ExecutedPathCount
        {
            get { return _stats.Values.Sum(i => (i.FalseCount > 0 ? 1 : 0) + (i.TrueCount > 0 ? 1 : 0)); }
        }

        public void AddExpressionToWath(Expression expression) // to przez konstruktor podawać, wtedy immutable
        {
            _stats.Add(expression, new ConditionExpressionStatistics());
        }

        public void ReportInvokeExpression(Expression expression, bool expressionResult)
        {
            _stats[expression].ReportConditionResult(expressionResult);
        }

        public void Report()
        {
            foreach (var stat in _stats)
            {
                Console.WriteLine("{0} \r\n\tRETURN TRUE: {1}, FALSE {2}", stat.Key, stat.Value.TrueCount, stat.Value.FalseCount);
            }
        }

        public void StartNewCycle()
        {
            foreach (ConditionExpressionStatistics stat in _stats.Values)
                stat.ResetCycleState();
        }
    }

    public sealed class DelegateSpy
    {
        public Result<TInput> Spy<TInput>(Expression<Func<TInput, bool>> func)
        {
            ParameterExpression pathSpyParameter = Expression.Parameter(typeof(Func<bool, Expression, bool>), "pathSpy");

            var funcExpressionWithSpyInParameter =
                Expression.Lambda<Func<TInput, Func<bool, Expression, bool>, bool>>(
                    func.Body,
                    func.Parameters[0],
                    pathSpyParameter);

            var stat = new ConditionPathStatistics();
            var visitor = new ExpressionVisitorForPathSpy(pathSpyParameter, stat.AddExpressionToWath);
            var spiedLambda = (Expression<Func<TInput, Func<bool, Expression, bool>, bool>>)visitor.
                Visit(funcExpressionWithSpyInParameter);
            Func<TInput, Func<bool, Expression, bool>, bool> compiledSpiedLambda = spiedLambda.Compile();

            Func<TInput, bool> outputFunc = _ => compiledSpiedLambda(_, (result, expr) =>
            {
                stat.StartNewCycle();
                stat.ReportInvokeExpression(expr, result);
                return result;
            });

            return new Result<TInput>(outputFunc, stat);
        }

        public struct Result<TInput>
        {
            private readonly Func<TInput, bool> _func;
            private readonly ConditionPathStatistics _pathStatistics;

            public Result(Func<TInput, bool> func, ConditionPathStatistics pathStatistics)
            {
                _func = func;
                _pathStatistics = pathStatistics;
            }

            public Func<TInput, bool> Func
            {
                get { return _func; }
            }

            public ConditionPathStatistics PathStatistics
            {
                get { return _pathStatistics; }
            }
        }
    }

    public sealed class ConditionExpressionStatistics
    {
        private bool _trueResultIsCountedInCurrentCycle;
        private bool _falseResultIsCountedInCurrentCycle;

        public ConditionExpressionStatistics()
        {
            FalseCount = 0;
            TrueCount = 0;
        }

        public int TrueCount { get; private set; }
        public int FalseCount { get; private set; }

        public void ReportConditionResult(bool result)
        {
            if (result)
            {
                if (_trueResultIsCountedInCurrentCycle == false)
                {
                    TrueCount = TrueCount + 1;
                    _trueResultIsCountedInCurrentCycle = true;
                }
            }

            if (_falseResultIsCountedInCurrentCycle == false)
            {
                FalseCount = FalseCount + 1;
                _falseResultIsCountedInCurrentCycle = true;
            }
        }

        public void ResetCycleState()
        {
            _trueResultIsCountedInCurrentCycle = false;
            _falseResultIsCountedInCurrentCycle = false;
        }
    }

    public sealed class ExpressionVisitorForPathSpy : ExpressionVisitor
    {
        private readonly ParameterExpression _pathSpyParameter;
        private readonly Action<Expression> _reportVisitedConditionExpression;

        public ExpressionVisitorForPathSpy(ParameterExpression pathSpyParameter, Action<Expression> reportVisitedConditionExpression)
        {
            _pathSpyParameter = pathSpyParameter;
            _reportVisitedConditionExpression = reportVisitedConditionExpression;
        }

        public override Expression Visit(Expression node)
        {
            if (node.Type == typeof(bool)) // todo może dla bezpieczeństwa sprawdzać czy już nie przerabiam zbędnie albo co gorsza w złej kolejności
            {
                _reportVisitedConditionExpression(node);
                var visitedNode = base.Visit(node);
                return Expression.Invoke(_pathSpyParameter, visitedNode, Expression.Constant(node));
            }
            return base.Visit(node);
        }
    }

    public static class ResultValidator
    {
        public static InputDataHolder<TInputData> ForInputData<TInputData>(IEnumerable<TInputData> inputData)
        {
            return new InputDataHolder<TInputData>(inputData);
        }

        public struct InputDataHolder<TInputData>
        {
            private readonly IEnumerable<TInputData> _inputData;

            internal InputDataHolder(IEnumerable<TInputData> inputData)
            {
                _inputData = inputData;
            }

            public TestedFunctionHolder<TFunctionResult> TestFunction<TFunctionResult>(
                Func<TInputData, TFunctionResult> functionUderTest)
            {
                return new TestedFunctionHolder<TFunctionResult>(_inputData, functionUderTest);
            }

            public struct TestedFunctionHolder<TFunctionResult>
            {
                private readonly IEnumerable<TInputData> _inputData;
                private readonly Func<TInputData, TFunctionResult> _functionUderTest;

                internal TestedFunctionHolder(
                    IEnumerable<TInputData> inputData, Func<TInputData, TFunctionResult> functionUderTest)
                {
                    _inputData = inputData;
                    _functionUderTest = functionUderTest;
                }

                public void WithSpecyfication(
                    Func<Specyfication<TInputData, TFunctionResult>, Specyfication<TInputData, TFunctionResult>> specificationBuilder)
                {
                    var initial = Specyfication<TInputData, TFunctionResult>.Root;
                    Specyfication<TInputData, TFunctionResult> fullSpec = specificationBuilder(initial);
                    fullSpec.Verify(_inputData, _functionUderTest);
                }
            }
        }
    }

    public sealed class Specyfication<TInputData, TFunctionResult>
    {
        private readonly Specyfication<TInputData, TFunctionResult> _parent; // can be null
        private readonly Expression<Func<TInputData, bool>> _condition;
        private readonly TFunctionResult _result;

        internal Specyfication(
            Specyfication<TInputData, TFunctionResult> parent,
            Expression<Func<TInputData, bool>> condition,
            TFunctionResult result) // to opakować
        {
            _parent = parent;
            _condition = condition;
            _result = result;
        }

        public static Specyfication<TInputData, TFunctionResult> Root =
            new Specyfication<TInputData, TFunctionResult>(null, null, default(TFunctionResult));

        public ConditionHolder When(Expression<Func<TInputData, bool>> condition)
        {
            return new ConditionHolder(this, condition);
        }

        public struct ConditionHolder
        {
            private readonly Specyfication<TInputData, TFunctionResult> _parent;
            private readonly Expression<Func<TInputData, bool>> _condition;

            public ConditionHolder(Specyfication<TInputData, TFunctionResult> parent, Expression<Func<TInputData, bool>> condition)
            {
                _parent = parent;
                _condition = condition;
            }

            public Specyfication<TInputData, TFunctionResult> ThenResultIs(TFunctionResult expectedResult) // wersja z funkcją i Histogramem
            {
                return new Specyfication<TInputData, TFunctionResult>(_parent, _condition, expectedResult);
            }
        }

        public Specyfication<TInputData, TFunctionResult> ForRestResultIs(TFunctionResult expectedResult)
        {
            return new Specyfication<TInputData, TFunctionResult>(this, data => true, expectedResult);
        }

        private sealed class ConditionStatistics
        {
            public int Executed;
            public int Succes;
            public int Fail;
        }

        internal void Verify(IEnumerable<TInputData> testDatas, Func<TInputData, TFunctionResult> testedFunc)
        {
            var specStack = new Stack<Specyfication<TInputData, TFunctionResult>>(); // optimize size?
            var curr = this;
            while (curr != Root)
            {
                specStack.Push(curr);
                curr = curr._parent;
            }

            var delegateSpy = new DelegateSpy();
            var specs = specStack.Select(spec => new
            {
                Spec = spec,
                Func = delegateSpy.Spy(spec._condition),
                Stat = new ConditionStatistics { Succes = 0, Fail = 0 }
            }).ToArray();

            foreach (var inputData in testDatas)
            {
                TFunctionResult result = testedFunc(inputData);

                foreach (var spec in specs)
                {
                    spec.Stat.Executed++;
                    if (spec.Func.Func(inputData))
                    {
                        if (spec.Spec._result.Equals(result) == false) // iequatable
                        {
                            Console.WriteLine(
                                "Błąd!!!\r\n{0}\r\nNiezgodność ze specyfikacją: Oczekiwano {1} a otrzymano {2}.\r\n",
                                _condition.Body, spec.Spec._result, result);

                            spec.Stat.Fail++;
                        }
                        else
                        {
                            spec.Stat.Succes++;
                        }

                        // zrobić formater, jedne normalny, drugi wstawiający wartości, może wykożystać code dom?
                        // zrobić analizę ilości ścieżek w wyrażeniu.

                        break;
                    }
                }
            }

            foreach (var spec in specs)
            {
                Console.WriteLine(
                    "Succes {0}, Fail {1}. Paths {2}/{3}. Executed {4}.",
                    spec.Stat.Succes, spec.Stat.Fail,
                    spec.Func.PathStatistics.ExecutedPathCount, spec.Func.PathStatistics.TotalPathCount,
                    spec.Stat.Executed);
            }
            Console.WriteLine();

            foreach (var spec in specs)
            {
                spec.Func.PathStatistics.Report();
                Console.WriteLine("***\r\n");
            }
        }
    }
}
