using System.Collections.Generic;
using System.Linq;
//using Microsoft.Test.VariationGeneration;

namespace QuickTestsFramework
{
    public sealed class TestApiAdapter : ICombinationarSequecnceGenerator
    {
        public IEnumerable<IValueGetter> Sequence(IEnumerable<ParameterSpec> parametersSpec, int order)
        {
            /*var p = new List<ParameterBase>();
            foreach (ParameterSpec parameterSpec in parametersSpec)
            {
                var parameterValues = new Parameter<object>(parameterSpec.ParameterName);
                foreach (object possibleValue in parameterSpec.PossibleValues)
                    parameterValues.Add(possibleValue);

                p.Add(parameterValues);
            }
            var model = new Model(p);

            var valueGetter = new ValueGetter();
            return model.GenerateVariations(order).Select(variation =>
            {
                valueGetter.SetCurr(variation);
                return valueGetter;
            });*/
            return null;
        }

        /*private sealed class ValueGetter : IValueGetter
        {
            private Variation _variation;

            public T GetValue<T>(string key)
            {
                return (T)_variation[key];
            }

            public void SetCurr(Variation variation)
            {
                _variation = variation;
            }
        }*/
    }
}