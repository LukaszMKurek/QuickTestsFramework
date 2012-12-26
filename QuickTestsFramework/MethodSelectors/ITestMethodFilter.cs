using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public interface ITestMethodFilter
   {
      IEnumerable<MethodData> Filter(IEnumerable<MethodData> inputMethodDatas);
   }
}