using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public sealed class RunInExclusiveGroupAttributeFilter : ITestMethodFilter
   {
      public IEnumerable<MethodData> Filter(IEnumerable<MethodData> inputMethodDatas)
      {
         var methodData = inputMethodDatas.ToArray();
         bool anyMethodInExclusiveGroup = methodData.Any(
            method => method.WillExecute && DoesMethodHaveExclusiveAttribute(method));

         return methodData.Select(method =>
         {
            bool methodHaveExclusiveAttribute = DoesMethodHaveExclusiveAttribute(method);

            if (anyMethodInExclusiveGroup && methodHaveExclusiveAttribute && method.WillExecute == false)
            {
               method.AddErrorMessage("Metoda posiada atrybut RunInExclusiveGroupAttribute ale metoda nie spe³nia wymagañ niezbêdych do uruchomienia jako test.\r\n");
               
               return method;
            }

            if (anyMethodInExclusiveGroup && methodHaveExclusiveAttribute == false && method.WillExecute)
            {
               method.AddIgnoreMessage("Test nie zosta³ wykonany poniewa¿ nie znajduje siê grupie testów wybranych przez atrybut [RunInExclusiveGroup].\r\n");
               
               return method;
            }

            return method;
         });
      }

      private static bool DoesMethodHaveExclusiveAttribute(MethodData method)
      {
         return method.MethodInfo.IsDefined(typeof(RunInExclusiveGroupAttribute), true);
      }
   }
}