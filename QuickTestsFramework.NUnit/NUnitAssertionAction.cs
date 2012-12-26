using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.NUnit
{
   public sealed class NUnitAssertionAction : IAssertionAction
   {
      public void Ignore(string message)
      {
         Assert.Ignore(message);
      }

      public void Fail(string message)
      {
         Assert.Fail(message);
      }
   }
}