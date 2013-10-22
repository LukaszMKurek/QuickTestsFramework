using System.Linq;
using System.Collections.Generic;
using System;
using QuickTestsFramework.Internals;
using QuickTestsFramework.MSTest;

namespace QuickTestsFramework.MSTest.Tests
{
   public static class RunnerHelper
   {
      public static Runner Create()
      {
         var exceptionFilter = new MSTestExceptionFilter(printStacktrace: false);
         var viewTestFixture = new ViewTestFixture(exceptionFilter);
         var inicjalizerView = new InicjalizerView(exceptionFilter);
         var nUnitAssertionAction = new MSTestAssertionAction();
         var runInExclusiveGroupAttributeFilter = new RunInExclusiveGroupAttributeFilter();
         var traditionalTestAttributeFilter = new TraditionalTestAttributeFilter();
         var testMethodSelector = new TestSelector(
            new MSTestTestMethodSelectorFromTestFixture(),
            new ITestMethodFilter[] { runInExclusiveGroupAttributeFilter, traditionalTestAttributeFilter },
            nUnitAssertionAction);
         var nUnitTestMethodSelectorFromCallStack = new MSTestTestMethodSelectorFromCallStack();

         return new Runner(
            exceptionFilter, viewTestFixture, inicjalizerView,
            testMethodSelector, nUnitAssertionAction, nUnitTestMethodSelectorFromCallStack);
      }
   }
}