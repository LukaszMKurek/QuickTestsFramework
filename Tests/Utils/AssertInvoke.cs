using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   public static class AssertInvoke
   {
      public static void That(Action action, string outputIsEqualTo)
      {
         string output = "";
         Assert.DoesNotThrow(() => 
            {
               output = Catch.Output(action);
            });
         Assert.That(output.Trim(), Is.EqualTo(outputIsEqualTo.Trim()), "Catched console output is different than expected.");
      }

      public static void That(Action action, string outputIsEqualTo, Type exceptionType, string exceptionMessage)
      {
         string output = Catch.Output(() =>
         {
            Exception exception = Assert.Throws(exceptionType, () => action());
            Assert.That(exception.Message.Trim(), Is.EqualTo(exceptionMessage), "Exception.Message is different than expected.");
         });
         Assert.That(output.Trim(), Is.EqualTo(outputIsEqualTo.Trim()), "Catched console output is different than expected.");
      }
   }
}