using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public static class TestRunner
   {
      public static void RunAssertion<T>(TestState testState, Action<T> assertion, IViewTestFixture view, IExceptionFilter exceptionFilter, IAssertionAction assertionAction)
      {
         bool anyFail = false;

         view.PrintTestMethodHead(testState);
         if (testState.HasProblem)
         {
            testState.ReportProblem();
            view.PrintTestMethodFail();
            assertionAction.Fail("Wczasie inicjalizacji pojawiły się błędy.");//todo
            //return;
         }

         int i = 1;
         foreach (var testCaseState in testState.TestCaseStates)
         {
            view.PrintTestCaseHead(i++, testCaseState);
            if (testCaseState.HasProblem)
            {
               testCaseState.ReportProblem();
               view.PrintTestCaseFail();
               anyFail = true;
               continue;
            }

            var data = (T)testCaseState.Data;
            Exception exception = Catch.Exception(() => assertion(data));

            if (exception == null)
            {
               view.PrintSucces();
            }
            else 
            {
               view.ReportException(exception);

               if (exceptionFilter.IsSuccessException(exception))
               {
                  view.PrintSucces();
               }
               else if (exceptionFilter.IsIgnoreException(exception))
               {
                  view.PrintTestCaseIgnore();
               }
               else
               {
                  view.PrintTestCaseFail();
                  anyFail = true;
               }
            }
         }

         if (anyFail)
            assertionAction.Fail("Jeden z przypadków testowych nie przeszedł.");
      }

      public static TestState RunInitializer<T>(Func<IEnumerable<T>> testCaseGenerator, Action<T> inicializer, IInicjalizerView view)
      {
         var testState = new TestState();
         
         IEnumerable<T> testCases = null;
         Exception methodException = null;
         string consoleOutput = Catch.Output(() =>
            methodException = Catch.Exception(() => testCases = testCaseGenerator()));

         testState.AppendToOutput(consoleOutput);

         if (methodException != null)
         {
            testState.AddProblemReporter(() => view.TestCaseGeneratorThrowException(methodException));
            return testState;
         }

         if (testCases == null)
         {
            testState.AddProblemReporter(view.TestCaseGeneratorReturnNull);
            return testState;
         }

         IEnumerator<T> enumerator = testCases.GetEnumerator();
         while (true)
         {
            Exception exception = null;
            bool isNext = false;
            consoleOutput = Catch.Output(() =>
               exception = Catch.Exception(() => isNext = enumerator.MoveNext()));

            if (exception != null)
            {
               var caseState = new TestCaseState();

               caseState.AppendToOutput(consoleOutput);
               caseState.AddProblemReporter(() => view.TestCaseGeneratorThrowOnGettingNextTestCase(exception));
               
               testState.AddTestCaseState(caseState);
               break;
            }

            if (isNext == false)
               break;

            var testCase = enumerator.Current;

            consoleOutput += Catch.Output(() =>
               exception = Catch.Exception(() => inicializer(testCase)));

            var testCaseState = new TestCaseState();
            testCaseState.AppendToOutput(consoleOutput);

            if (exception != null)
            {
               testCaseState.AddProblemReporter(() => view.ExceptionOnInitializeTest(exception));
            }
            testCaseState.Data = testCase;

            testState.AddTestCaseState(testCaseState);
         }

         if (testState.TestCaseStates.Count == 0)
            testState.AddProblemReporter(view.TestCaseGeneratorReturnZeroTestCases);

         return testState;
      }
   }
}