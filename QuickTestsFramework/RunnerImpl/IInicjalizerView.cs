using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public interface IInicjalizerView
   {
      void TestCaseGeneratorThrowException(Exception exception);
      void TestCaseGeneratorReturnNull();
      void TestCaseGeneratorReturnZeroTestCases();
      void TestCaseGeneratorThrowOnGettingNextTestCase(Exception exception);
      void ExceptionOnInitializeTest(Exception exception);
   }
}