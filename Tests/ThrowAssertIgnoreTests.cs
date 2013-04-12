using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class ThrowAssertIgnoreTests
   {
      private ThrowAssertIgnoreTestsInternal _ut;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _ut = new ThrowAssertIgnoreTestsInternal();
         AssertInvoke.That(() => _ut.SetUp(), @"
ExceptionBeforeRun; before;
ExceptionAfterRun; before;
ExceptionAfterRun; after;
Process...");
      }

      [Test]
      public void ExceptionInInicjalizer()
      {
         AssertInvoke.That(() => _ut.ExceptionInInicjalizer(), @"
*** Test case 1 ***

ExceptionInInicjalizer; init; tc: 1

ExceptionInInicjalizer; assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

ExceptionInInicjalizer; init; tc: 2

Wyj¹tek podczas inicjalizowania przypadku testowego: 
Assert.Ignore message

___ Test case FAIL _________

*** Test case 3 ***

ExceptionInInicjalizer; init; tc: 5

ExceptionInInicjalizer; assert; tc: 5

___ Test case OK _________

*** Test case 4 ***

ExceptionInInicjalizer; init; tc: 7

Wyj¹tek podczas inicjalizowania przypadku testowego: 
Assert.Ignore message

___ Test case FAIL _________

*** Test case 5 ***

ExceptionInInicjalizer; init; tc: 9

ExceptionInInicjalizer; assert; tc: 9

___ Test case OK _________", typeof(AssertionException), @"Jeden z przypadków testowych nie przeszed³.");
      }

      [Test]
      public void ExceptionInAserrt()
      {
         AssertInvoke.That(() => _ut.ExceptionInAserrt(), @"
*** Test case 1 ***

ExceptionInAserrt; init; tc: 1

ExceptionInAserrt; assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

ExceptionInAserrt; init; tc: 2

ExceptionInAserrt; assert; tc: 2
Assercja rzuci³a wyj¹tek: 
Assert.Ignore message

___ Test case IGNORE _________

*** Test case 3 ***

ExceptionInAserrt; init; tc: 5

ExceptionInAserrt; assert; tc: 5

___ Test case OK _________

*** Test case 4 ***

ExceptionInAserrt; init; tc: 7

ExceptionInAserrt; assert; tc: 7
Assercja rzuci³a wyj¹tek: 
Assert.Ignore message

___ Test case IGNORE _________

*** Test case 5 ***

ExceptionInAserrt; init; tc: 9

ExceptionInAserrt; assert; tc: 9

___ Test case OK _________");
      }

      [Test]
      public void ExceptionOnStartTestCaseGeneration()
      {
         AssertInvoke.That(() => _ut.ExceptionOnStartTestCaseGeneration(), @"
ExceptionOnStartTestCaseGeneration;

TestCaseGenerator throw exception: 
Assert.Ignore message

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawi³y siê b³êdy.");
      }

      [Test]
      public void ExceptionOnYieldTestCaseGeneration()
      {
         AssertInvoke.That(() => _ut.ExceptionOnYieldTestCaseGeneration(), @"
*** Test case 1 ***

ExceptionOnYieldTestCaseGeneration; 1
ExceptionInAserrt; init; tc: 1

ExceptionInAserrt; assert; tc: 1

___ Test case OK _________

*** Test case 2 ***

ExceptionOnYieldTestCaseGeneration; 2

Wyj¹tek podczas generowania TestCase: 
Assert.Ignore message

___ Test case FAIL _________", typeof(AssertionException), @"Jeden z przypadków testowych nie przeszed³.");
      }

      [Test]
      public void ExceptionBeforeRun()
      {
         AssertInvoke.That(() => _ut.ExceptionBeforeRun(), @"
ExceptionBeforeRun; before;", typeof(IgnoreException), @"Assert.Ignore message");
      }

      [Test]
      public void ExceptionAfterRun()
      {
         AssertInvoke.That(() => _ut.ExceptionAfterRun(), @"
ExceptionAfterRun; before;

Metoda testowa rzuci³a wyj¹tek.
Assert.Ignore message

___ Test FAIL _________", typeof(AssertionException), @"Wczasie inicjalizacji pojawi³y siê b³êdy.");
      }
   }
}