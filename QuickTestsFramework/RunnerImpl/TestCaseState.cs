using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public sealed class TestCaseState
   {
      public TestCaseState()
      {
         Output = "";
         ReportProblem = () => { };
         HasProblem = false;
      }

      // order
      public object Data { get; set; }
      public string Output { get; private set; }
      public Action ReportProblem { get; private set; }
      public bool HasProblem { get; private set; }

      public void AppendToOutput(string message)
      {
         Output += message;
      }

      public void AddProblemReporter(Action reporter)
      {
         HasProblem = true;
         ReportProblem += reporter;
      }
   }
}