using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace QuickTestsFramework.NUnit
{
   public sealed class NUnitTestMethodSelectorFromCallStack : ITestMethodSelectorFromCallStack
   {
      //global::NUnit.Framework.TestContext.CurrentContext.Test.FullName
      public MethodBase GetCallingTestMethod()
      {
         StackFrame[] stackFrames = new StackTrace().GetFrames();
         if (stackFrames == null)
            throw new InvalidOperationException("Reflection sucks. Probably you wrong use Run method.");

         StackFrame stackFrame = stackFrames.FirstOrDefault(x =>
         {
            var methodBase = x.GetMethod();
            return
               methodBase.IsDefined(NUnitHelper.GetType("NUnit.Framework.TestAttribute"), true) ||
               methodBase.IsDefined(NUnitHelper.GetType("NUnit.Framework.TestCaseSourceAttribute"), true) ||
               methodBase.IsDefined(NUnitHelper.GetType("NUnit.Framework.TestCaseAttribute"), true);
         });
         if (stackFrame == null)
            throw new InvalidOperationException("Run method can be call only by test method with [Test] attribute.");

         return stackFrame.GetMethod();
      }
   }
}