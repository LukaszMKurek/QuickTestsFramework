using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class NullArgumentTests
   {
      private NullArgumentTestsInternal _ut;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _ut = new NullArgumentTestsInternal();
         AssertInvoke.That(() => _ut.SetUp(), @"
before
before
before
before
before
Process...");
      }

      [Test]
      public void T01()
      {
        AssertInvoke.That(() => _ut.T01(), @"
before

TestCaseGenerator nie może być nullem.
Delegata assercji nie może być null.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }

      [Test]
      public void T02()
      {
         AssertInvoke.That(() => _ut.T02(), @"
before

Inicjalizer nie może być nullem.
Delegata assercji nie może być null.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }

      [Test]
      public void T03()
      {
         AssertInvoke.That(() => _ut.T03(), @"
before
init

Delegata assercji nie może być null.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }

      [Test]
      public void T04()
      {
         AssertInvoke.That(() => _ut.T04(), @"before
init

Delegata assercji nie może być null.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }

      [Test]
      public void T05()
      {
         AssertInvoke.That(() => _ut.T05(), @"
before

TestCaseGenerator return null.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }
   }

   //[TestFixture]
}
