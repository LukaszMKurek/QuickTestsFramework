using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace QuickTestsFramework
{
   public sealed class TestMethodInvoker
   {
      private readonly MethodBase _method;
      private readonly bool _willExecute;
      private readonly Action _execute;
      private readonly Action _runDuringVerification;

      public TestMethodInvoker(MethodBase method, bool willExecute, Action execute, Action runDuringVerification)
      {
         _method = method;
         _willExecute = willExecute;
         _execute = execute;
         _runDuringVerification = runDuringVerification;
      }

      public MethodBase Method
      {
         get { return _method; }
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