﻿using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public interface IViewTestFixture
   {
      void PrintSucces();
      void PrintTestCaseIgnore();
      void PrintTestCaseFail();
      void PrintTestCaseInconclusive();
      void PrintTestMethodFail();
      void PrintTestCaseHead(int i, TestCaseState testCaseState);
      void PrintTestMethodHead(TestState testState);
      void ReportException(Exception exception);
      void ReportInitializationExcepton(Exception exception);
      void ReportNullArgument(string message);
      void ReportAssertDelegateIsNull();
   }
}