using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public sealed class TraditionalTestAttributeFilter : ITestMethodFilter
   {
      public IEnumerable<MethodData> Filter(IEnumerable<MethodData> inputMethodDatas)
      {
         return inputMethodDatas.Select(method =>
         {
            if (TraditionalTestAttribute(method) && method.WillExecute)
            {
               method.AddErrorMessage("Test z atrybutem [TraditionalTest] nie mo¿e zawieraæ wywo³ania metody Run.\r\n");
            }

            return method;
         });
      }

      private static bool TraditionalTestAttribute(MethodData method)
      {
         return method.MethodInfo.IsDefined(typeof(TraditionalTestAttribute), true);
      }
   }
}