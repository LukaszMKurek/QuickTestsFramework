using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public sealed class InicjalizerView : IInicjalizerView
   {
      private readonly IExceptionFilter _exceptionFilter;

      public InicjalizerView(IExceptionFilter exceptionFilter)
      {
         _exceptionFilter = exceptionFilter;
      }

      public void TestCaseGeneratorThrowException(Exception exception)
      {
         Console.WriteLine("TestCaseGenerator throw exception: \r\n{0}", _exceptionFilter.RenderExceptionConditional(exception));
      }

      public void TestCaseGeneratorReturnNull()
      {
         Console.WriteLine("TestCaseGenerator return null.");
      }

      public void TestCaseGeneratorReturnZeroTestCases()
      {
         Console.WriteLine("TestCaseGenerator return zero test cases.");
      }

      public void TestCaseGeneratorThrowOnGettingNextTestCase(Exception exception)
      {
         Console.WriteLine("Wyjątek podczas generowania TestCase: \r\n{0}", _exceptionFilter.RenderExceptionConditional(exception));
      }

      public void ExceptionOnInitializeTest(Exception exception)
      {
         Console.WriteLine("Wyjątek podczas inicjalizowania przypadku testowego: \r\n{0}", _exceptionFilter.RenderExceptionConditional(exception));
      }
   }
}