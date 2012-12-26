using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace QuickTestsFramework
{
   public interface ITestMethodSelectorFromCallStack
   {
      MethodBase GetCallingTestMethod();
   }
}