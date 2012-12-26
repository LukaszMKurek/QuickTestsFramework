using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public sealed class TestMethodInvoker
   {
      private readonly string _name;
      private readonly bool _willExecute;
      private readonly Action _execute;
      private readonly Action _runDuringVerification;

      public TestMethodInvoker(string name, bool willExecute, Action execute, Action runDuringVerification)
      {
         _name = name;
         _willExecute = willExecute;
         _execute = execute;
         _runDuringVerification = runDuringVerification;
      }

      public string Name
      {
         get { return _name; }
      }

      public bool WillExecute
      {
         get { return _willExecute; }
      }

      public Action Execute
      {
         get { return _execute; }
      }

      public Action RunDuringVerification
      {
         get { return _runDuringVerification; }
      }
   }
}