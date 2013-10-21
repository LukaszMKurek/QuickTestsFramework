using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public interface ITestMethodSelectorFromTestFixture
   {
      IEnumerable<MethodData> GetTestsMethod(object testFixtureInstance);
   }
}