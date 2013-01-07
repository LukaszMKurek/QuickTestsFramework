using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;
using NUnit.Framework;

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
               methodBase.IsDefined(typeof(TestAttribute), true) || 
               methodBase.IsDefined(typeof(TestCaseSourceAttribute), true) || 
               methodBase.IsDefined(typeof(TestCaseAttribute), true);
         });
         if (stackFrame == null)
            throw new InvalidOperationException("Run method can be call only by test method with [Test] attribute.");

         return stackFrame.GetMethod();
      }
   }

   public sealed class NUnitTestMethodSelectorFromTestFixture : ITestMethodSelectorFromTestFixture
   {
      public IEnumerable<MethodData> GetTestsMethod(object testFixtureInstance)
      {
         return testFixtureInstance.GetType().GetMethods().Select(method =>
         {
            var methodRunData = new MethodData { MethodInfo = method };

            if (method.IsDefined(typeof(TestAttribute), true) == false)
            {
               methodRunData.AddErrorMessage("Metoda nie zosta³a zakwalifikowana przez QuickTestsFramework do uruchomienia.\r\n");
               
               return methodRunData;
            }

            if (method.GetParameters().Length > 0 || method.ReturnType != typeof(void))
            {
               methodRunData.AddErrorMessage("Obecnie nie s¹ obs³ugiwane testy parametryczne w stylu NUnit.\r\n");
               
               return methodRunData;
            }

            if (method.IsDefined(typeof(IgnoreAttribute), true))
            {
               methodRunData.AddIgnoreMessage("Metoda jest ignorowana.\r\n");
               
               return methodRunData;
            }

            return methodRunData;
         });
      }
   }
}