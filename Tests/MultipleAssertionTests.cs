using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using QuickTestsFramework.NUnit;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class MultipleAssertionTests
   {
      [Test]
      public void T01()
      {
         var a = new MultipleAssertion(new NUnitExceptionFilter(), new NUnitAssertionAction());

          AssertInvoke.That(() => 
            a.Run(() => Assert.Fail(a == null ? "df" : " sd"), // 1
               () => Assert.AreEqual(2, 2), // 2
               () => Assert.AreEqual(1, 2), // 3
               () => Assert.Pass("ok"), // 4
               () => Assert.Ignore("ok - ignore"), // 5
               () => Assert.That(a, Is.Null)), // 6
            @"
Assertion 1 FAIL:
 sd

Assertion 3 FAIL:
  Expected: 1
  But was:  2


Assertion 4 PASS:
ok

Assertion 5 IGNORE:
ok - ignore

Assertion 6 FAIL:
  Expected: null
  But was:  <QuickTestsFramework.MultipleAssertion>", typeof(AssertionException), @"Assertions: 1, 3, 6 failed.");
      }

      [Test]
      public void T02()
      {
         var a = new MultipleAssertion(new NUnitExceptionFilter(), new NUnitAssertionAction());

         AssertInvoke.That(() => 
            a.Run(() => Assert.AreEqual(1, 2)), 
         @"
Assertion 1 FAIL:
  Expected: 1
  But was:  2", typeof(AssertionException), @"Assertions: 1 failed.");
      }

      [Test]
      public void T03()
      {
         var a = new MultipleAssertion(new NUnitExceptionFilter(), new NUnitAssertionAction());

         a.Run(
            () => Assert.AreEqual(2, 2));
      }

      [Test]
      public void T04()
      {
         var a = new MultipleAssertion(new NUnitExceptionFilter(), new NUnitAssertionAction());

         a.Run();
      }
   }
}