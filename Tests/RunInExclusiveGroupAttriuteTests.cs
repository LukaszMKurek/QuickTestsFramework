using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class RunInExclusiveGroupAttriuteTests
   {
      private RunInExclusiveGroupAttriuteTestInternal _ut;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _ut = new RunInExclusiveGroupAttriuteTestInternal();
         AssertInvoke.That(() => _ut.SetUp(), @"
Process...
");
      }

      [Test]
      public void T01()
      {
         AssertInvoke.That(() => _ut.T01(), @"", typeof(IgnoreException), @"Test nie został wykonany ponieważ nie znajduje się grupie testów wybranych przez atrybut [RunInExclusiveGroup].");
      }

      [Test]
      public void T02()
      {
         AssertInvoke.That(() => _ut.T02(), @"
*** Test case 1 ***

T02: init; tc: 1

T02: assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

T02: init; tc: 2

T02: assert; tc: 2

___ Test case OK _________");
      }

      [Test]
      public void T03()
      {
         AssertInvoke.That(() => _ut.T03(), @"", typeof(IgnoreException), @"Test nie został wykonany ponieważ nie znajduje się grupie testów wybranych przez atrybut [RunInExclusiveGroup].");
      }

      [Test]
      public void T04()
      {
         AssertInvoke.That(() => _ut.T04(), @"

*** Test case 1 ***

T04: init; tc: 1

T04: assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

T04: init; tc: 2

T04: assert; tc: 2

___ Test case OK _________");
      }

      [Test]
      public void T05()
      {
         AssertInvoke.That(() => _ut.T05(1), @"", typeof(AssertionException), @"Metoda nie została zakwalifikowana przez QuickTestsFramework do uruchomienia.
Metoda posiada atrybut RunInExclusiveGroupAttribute ale metoda nie spełnia wymagań niezbędych do uruchomienia jako test."); //todo differences
      }

      [Test]
      public void T06()
      {
         AssertInvoke.That(() => _ut.T06(), @"", typeof(AssertionException), @"Test z atrybutem [TraditionalTest] nie może zawierać wywołania metody Run.");
      }

      [Test]
      public void T07()
      {
         AssertInvoke.That(() => _ut.T07(), @"", typeof(AssertionException), @"Metoda jest ignorowana.
Metoda posiada atrybut RunInExclusiveGroupAttribute ale metoda nie spełnia wymagań niezbędych do uruchomienia jako test.");
      }
   }
}
