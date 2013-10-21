using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.NUnit
{
   public sealed class NUnitTestMethodSelectorFromTestFixture : ITestMethodSelectorFromTestFixture
   {
      public IEnumerable<MethodData> GetTestsMethod(object testFixtureInstance)
      {
         return testFixtureInstance.GetType().GetMethods().Select(method =>
         {
            var methodRunData = new MethodData { MethodInfo = method };

            if (method.IsDefined(NUnitHelper.GetType("NUnit.Framework.TestAttribute"), true) == false)
            {
               methodRunData.AddErrorMessage("Metoda nie została zakwalifikowana przez QuickTestsFramework do uruchomienia.\r\n");
               
               return methodRunData;
            }

            if (method.GetParameters().Length > 0 || method.ReturnType != typeof(void))
            {
               methodRunData.AddErrorMessage("Obecnie nie są obsługiwane testy parametryczne w stylu NUnit.\r\n");
               
               return methodRunData;
            }

            if (method.IsDefined(NUnitHelper.GetType("NUnit.Framework.IgnoreAttribute"), true))
            {
               methodRunData.AddIgnoreMessage("Metoda jest ignorowana.\r\n");
               
               return methodRunData;
            }

            return methodRunData;
         });
      }
   }
}