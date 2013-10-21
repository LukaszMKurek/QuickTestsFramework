using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public interface ITestMethodFilter
   {
      IEnumerable<MethodData> Filter(IEnumerable<MethodData> inputMethodDatas);
   }
}