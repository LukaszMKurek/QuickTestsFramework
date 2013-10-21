using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public sealed class TestState
   {
      public TestState()
      {
         Output = "";
         ReportProblem = () => { };
         HasProblem = false;
         _testCaseStates = new List<TestCaseState>();
      }

      // name
      public string Output { get; private set; }
      private readonly List<TestCaseState> _testCaseStates;
      public ReadOnlyCollection<TestCaseState> TestCaseStates 
      {
         get { return _testCaseStates.AsReadOnly(); }
      }
      public Action ReportProblem { get; private set; }
      public bool HasProblem { get; private set; }

      public void AppendToOutput(string message)
      {
         Output += message;
      }

      public void AddTestCaseState(TestCaseState testCaseState)
      {
         _testCaseStates.Add(testCaseState);
      }

      public void AddProblemReporter(Action reporter)
      {
         HasProblem = true;
         ReportProblem += reporter;
      }
   }
}