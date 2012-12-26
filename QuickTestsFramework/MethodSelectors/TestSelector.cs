using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickTestsFramework
{
   /*
    Wprowadzic mechanizm akcji dla konkretnych wyjątków żucanych
    * sprawdzac czy wołany test użyje run
    * wprowadzić arybut określający tradycyjny test
    */

   public sealed class TestSelector
   {
      private readonly ITestMethodSelectorFromTestFixture _methodSelectorFromTestFixture;
      private readonly IAssertionAction _assertionAction;
      private readonly ITestMethodFilter[] _testMethodFilter;

      public TestSelector(ITestMethodSelectorFromTestFixture testMethodSelectorFromTestFixture, IEnumerable<ITestMethodFilter> testMethodFilters, IAssertionAction assertionAction)
      {
         _methodSelectorFromTestFixture = testMethodSelectorFromTestFixture;
         _assertionAction = assertionAction;
         _testMethodFilter = testMethodFilters.ToArray();
      }

      // wziąść pod uwagę wszystkie aspekty dziedizczenia i interfejsów.
      // co się działo będzie z metodami prywatnymi?
      // co z atrybutami potomymi?
      public IEnumerable<TestMethodInvoker> GetTestsToRun(object testFixtureInstance)
      {
         IEnumerable<MethodData> methodRunDatas = _methodSelectorFromTestFixture.GetTestsMethod(testFixtureInstance);

         var filterMethodData = _testMethodFilter.Aggregate(methodRunDatas, (current, t) => t.Filter(current));

         return MapMethodData(filterMethodData, testFixtureInstance);
      }

      private IEnumerable<TestMethodInvoker> MapMethodData(IEnumerable<MethodData> filterMethodDataByExclusiveGroup, object testFixtureInstance)
      {
         return filterMethodDataByExclusiveGroup.Select(method => new TestMethodInvoker(
            name: method.MethodInfo.Name,
            willExecute: method.WillExecute,
            runDuringVerification: () =>
            {
               if (method.HasError)
                  _assertionAction.Fail(method.Messages);
               else if (method.HasIgnore)
                  _assertionAction.Ignore(method.Messages);
            },
            execute: () => method.MethodInfo.Invoke(testFixtureInstance, new object[0]) // TargetInvocationException is not rethrow because I don't want lose oryginal stackrace
         ));
      }
   }
}
