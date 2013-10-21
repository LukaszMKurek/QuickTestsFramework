using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.NUnit
{
   public sealed class NUnitAssertionAction : IAssertionAction
   {
      public void Ignore(string message)
      {
         NUnitHelper.InvokeStatic("NUnit.Framework.Assert", "Ignore", message);
         //Assert.Ignore(message);
      }

      public void Fail(string message)
      {
         NUnitHelper.InvokeStatic("NUnit.Framework.Assert", "Fail", message);
         //Assert.Fail(message);
      }
   }
}
