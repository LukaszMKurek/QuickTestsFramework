using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.MSTest
{
   public sealed class MSTestAssertionAction : IAssertionAction
   {
      public void Ignore(string message)
      {
         MSTestHelper.InvokeStatic("Microsoft.VisualStudio.TestTools.UnitTesting.Assert", "Inconclusive", message);
         //Assert.Inconclusive(message);
      }

      public void Fail(string message)
      {
         MSTestHelper.InvokeStatic("Microsoft.VisualStudio.TestTools.UnitTesting.Assert", "Fail", message);
         //Assert.Fail(message);
      }

      public void Inconclusive(string message)
      {
         MSTestHelper.InvokeStatic("Microsoft.VisualStudio.TestTools.UnitTesting.Assert", "Inconclusive", message);
         //Assert.Fail(message);
      }
   }
}
