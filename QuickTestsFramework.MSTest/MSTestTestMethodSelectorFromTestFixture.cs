using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.MSTest
{
   public sealed class MSTestTestMethodSelectorFromTestFixture : ITestMethodSelectorFromTestFixture
   {
      public IEnumerable<MethodData> GetTestsMethod(object testFixtureInstance)
      {
         return testFixtureInstance.GetType().GetMethods().Select(method =>
         {
            var methodRunData = new MethodData { MethodInfo = method };

            if (method.IsDefined(MSTestHelper.GetType("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"), true) == false)
            {
               methodRunData.AddErrorMessage("Metoda nie została zakwalifikowana przez QuickTestsFramework do uruchomienia.\r\n");
               
               return methodRunData;
            }

            if (method.GetParameters().Length > 0 || method.ReturnType != typeof(void))
            {
               methodRunData.AddErrorMessage("Metoda testowa nie może posiadać parametrów lub zwracać typ.\r\n");
               
               return methodRunData;
            }

            if (method.IsDefined(MSTestHelper.GetType("Microsoft.VisualStudio.TestTools.UnitTesting.IgnoreAttribute"), true))
            {
               methodRunData.AddIgnoreMessage("Metoda jest ignorowana.\r\n");
               
               return methodRunData;
            }

            return methodRunData;
         });
      }
   }
}