using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.MSTest
{
    public sealed class MSTestTestMethodSelectorFromCallStack : ITestMethodSelectorFromCallStack
    {
        //global::NUnit.Framework.TestContext.CurrentContext.Test.FullName
        public MethodBase GetCallingTestMethod()
        {
            StackFrame[] stackFrames = new StackTrace().GetFrames();
            if (stackFrames == null)
                throw new InvalidOperationException("Reflection sucks. Probably you wrong use Run method.");

            StackFrame stackFrame = stackFrames.FirstOrDefault(x =>
            {
                MethodBase methodBase = x.GetMethod();
                return methodBase.IsDefined(MSTestHelper.GetType("Microsoft.VisualStudio.TestTools.UnitTesting.TestMethodAttribute"), true);
            });

            if (stackFrame == null)
                throw new InvalidOperationException(
                    "Run method can be call only by test method with [TestMethod] attribute and the test project must have disabled IL optimization (innlining was main problem).");

            return stackFrame.GetMethod();
        }
    }
}
