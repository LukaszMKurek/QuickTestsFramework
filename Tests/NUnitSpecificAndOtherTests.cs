using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class NUnitSpecificAndOtherTests
   {
      private NUnitSpecificAndOtherTestsInternal _ut;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _ut = new NUnitSpecificAndOtherTestsInternal();
         AssertInvoke.That(() => _ut.SetUp(), @"
T09: before 0
T09: after 1
T10: without Run 0
T11: without Run 0
Process...
");
      }

      [Test]
      public void T01()
      {
         AssertInvoke.That(() => _ut.T01(), @"
TestCaseGenerator return zero test cases.

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawiły się błędy.");
      }

      [Test]
      public void T02()
      {
         AssertInvoke.That(() => _ut.T02(), @"
*** Test case 1 ***

T02: init; tc: 0

T02: assert; tc: 0

___ Test case OK _________");
      }

      [Test]
      public void T03()
      {
         AssertInvoke.That(() => _ut.T03(), @"", typeof(IgnoreException), @"Metoda jest ignorowana.");
      }

      [Test]
      public void T04()
      {
         AssertInvoke.That(() => _ut.T04(1), @"", typeof(AssertionException), @"Metoda nie została zakwalifikowana przez QuickTestsFramework do uruchomienia.");
      }

      [Test]
      public void T05()
      {
         AssertInvoke.That(() => _ut.T05(1), @"", typeof(AssertionException), @"Obecnie nie są obsługiwane testy parametryczne w stylu NUnit.");
      }

      /*[Test]
      public void T06()
      {
         AssertInvoke.That(() => _ut.T06(), @"");
      }*/

      [Test]
      public void T07()
      {
         AssertInvoke.That(() => _ut.T07(), @"
T07: before 0
", typeof(AssertionException), @"Test z atrybutem [TraditionalTest] nie może zawierać wywołania metody Run.");
      }

      [Test]
      public void T08()
      {
         AssertInvoke.That(() => _ut.T08(), @"T08: test 0");
      }

      [Test]
      public void T09()
      {
         AssertInvoke.That(() => _ut.T09(), @"
T09: before 2

*** Test case 1 ***

T09: init; tc: 1

T09: assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

T09: init; tc: 2

T09: assert; tc: 2

___ Test case OK _________

T09: after 3");
      }

      [Test]
      public void T10()
      {
         AssertInvoke.That(() => _ut.T10(), @"
T10: without Run 1
");
      }

      [Test]
      public void T11()
      {
         AssertInvoke.That(() => _ut.T11(), @"
T11: without Run 1", typeof(AssertionException), @"bo tak");
      }

      [Test]
      public void T12()
      {
         AssertInvoke.That(() => _ut.T12(), @"
*** Test case 1 ***

T12: init; tc: 0

T12: assert; tc: 0

___ Test case OK _________");
      }

      /*[Test]
      public void T13()
      {
         AssertInvoke.That(() => _ut.T13(), @"");
      }*/

      [Test]
      public void T14()
      {
         AssertInvoke.That(() => _ut.T14(), @"
*** Test case 1 ***

T14: init; tc: 1

T14: assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

T14: init; tc: 2

T14: assert; tc: 2

___ Test case OK _________
");
      }

      [Test]
      public void T15()
      {
         AssertInvoke.That(() => _ut.T15(), @"
*** Test case 1 ***

T15: init; tc: 1

T15: assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

T15: init; tc: 2

T15: assert; tc: 2

___ Test case OK _________");
      }
   }
}
