using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public sealed class ViewTestFixture : IViewTestFixture
   {
      private readonly IExceptionFilter _exceptionFilter;

      public ViewTestFixture(IExceptionFilter exceptionFilter)
      {
         _exceptionFilter = exceptionFilter;
      }

      public void PrintSucces()
      {
         Console.WriteLine("\r\n___ Test case OK _________\r\n");
      }

      public void PrintTestCaseIgnore()
      {
         Console.WriteLine("\r\n___ Test case IGNORE _________\r\n");
      }
      
      public void PrintTestCaseFail()
      {
         Console.WriteLine("\r\n___ Test case FAIL _________\r\n");
      }

      public void PrintTestMethodFail()
      {
         Console.WriteLine("\r\n___ Test FAIL _________\r\n");
      }

      public void PrintTestCaseHead(int i, TestCaseState testCaseState)
      {
         Console.WriteLine("*** Test case {0} ***\r\n", i);
         Console.WriteLine(testCaseState.Output);
      }

      public void PrintTestMethodHead(TestState testState)
      {
         Console.WriteLine(testState.Output);
      }

      public void ReportException(Exception exception)
      {
         Console.WriteLine("Assercja rzuciła wyjątek: \r\n{0}", _exceptionFilter.RenderExceptionConditional(exception));
      }

      public void ReportInitializationExcepton(Exception exception)
      {
         Console.WriteLine("Metoda testowa rzuciła wyjątek.\r\n" + _exceptionFilter.RenderException(exception));
      }

      public void ReportNullArgument(string message)
      {
         Console.WriteLine(message);
      }

      public void ReportAssertDelegateIsNull()
      {
         Console.WriteLine("Delegata assercji nie może być null.");
      }
   }
}