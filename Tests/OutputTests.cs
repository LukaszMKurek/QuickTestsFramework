using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class OutputTests
   {
      private OutputTestsInternal _ut;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _ut = new OutputTestsInternal();
         AssertInvoke.That(() => _ut.SetUp(), @"
Debug 1: Before 1
Trace 1: Before 1
Output 1: Before 1
Debug 2: Before 1
Trace 2: Before 1
Output 2: Before 1
Debug 1: Init 1: 1
Trace 1: Init 1: 1
Debug 2: Init 1: 1
Trace 2: Init 1: 1
Debug 1: Init 1: 2
Trace 1: Init 1: 2
Debug 2: Init 1: 2
Trace 2: Init 1: 2
Debug 1: Init 1: 3
Trace 1: Init 1: 3
Debug 2: Init 1: 3
Trace 2: Init 1: 3
Debug 1: After 1
Trace 1: After 1
Output 1: After 1
Debug 2: After 1
Trace 2: After 1
Output 2: After 1
Debug 1: Before 2
Trace 1: Before 2
Output 1: Before 2
Debug 2: Before 2
Trace 2: Before 2
Output 2: Before 2
Debug 1: Init 2: 4
Trace 1: Init 2: 4
Debug 2: Init 2: 4
Trace 2: Init 2: 4
Debug 1: Init 2: 5
Trace 1: Init 2: 5
Debug 2: Init 2: 5
Trace 2: Init 2: 5
Debug 1: Init 2: 6
Trace 1: Init 2: 6
Debug 2: Init 2: 6
Trace 2: Init 2: 6
Debug 1: After 2
Trace 1: After 2
Output 1: After 2
Debug 2: After 2
Trace 2: After 2
Output 2: After 2
Debug 1: Before 3
Trace 1: Before 3
Output 1: Before 3
Debug 2: Before 3
Trace 2: Before 3
Output 2: Before 3
Debug 1: Init: X
Trace 1: Init: X
Debug 2: Init: X
Trace 2: Init: X
Debug 1: After 3
Trace 1: After 3
Output 1: After 3
Debug 2: After 3
Trace 2: After 3
Output 2: After 3");
      }
   
      [Test]
      public void T01()
      {
         AssertInvoke.That(() => _ut.T01(), @"
Debug 1: Before 1
Trace 1: Before 1
Output 1: Before 1
Debug 2: Before 1
Trace 2: Before 1
Output 2: Before 1

*** Test case 1 ***

Debug 1: Init 1: 1
Trace 1: Init 1: 1
Output 1: Init 1: 1
Debug 2: Init 1: 1
Trace 2: Init 1: 1
Output 2: Init 1: 1

Debug 1: Assert 1: 1
Trace 1: Assert 1: 1
Output 1: Assert 1: 1
Debug 2: Assert 1: 1
Trace 2: Assert 1: 1
Output 2: Assert 1: 1

___ Test case OK _________

*** Test case 2 ***

Debug 1: Init 1: 2
Trace 1: Init 1: 2
Output 1: Init 1: 2
Debug 2: Init 1: 2
Trace 2: Init 1: 2
Output 2: Init 1: 2

Debug 1: Assert 1: 2
Trace 1: Assert 1: 2
Output 1: Assert 1: 2
Debug 2: Assert 1: 2
Trace 2: Assert 1: 2
Output 2: Assert 1: 2

___ Test case OK _________

*** Test case 3 ***

Debug 1: Init 1: 3
Trace 1: Init 1: 3
Output 1: Init 1: 3
Debug 2: Init 1: 3
Trace 2: Init 1: 3
Output 2: Init 1: 3

Debug 1: Assert 1: 3
Trace 1: Assert 1: 3
Output 1: Assert 1: 3
Debug 2: Assert 1: 3
Trace 2: Assert 1: 3
Output 2: Assert 1: 3

___ Test case OK _________

Debug 1: After 1
Trace 1: After 1
Output 1: After 1
Debug 2: After 1
Trace 2: After 1
Output 2: After 1");
      }

      [Test]
      public void T02()
      {
         AssertInvoke.That(() => _ut.T02(), @"
Debug 1: Before 2
Trace 1: Before 2
Output 1: Before 2
Debug 2: Before 2
Trace 2: Before 2
Output 2: Before 2

*** Test case 1 ***

Debug 1: Init 2: 4
Trace 1: Init 2: 4
Output 1: Init 2: 4
Debug 2: Init 2: 4
Trace 2: Init 2: 4
Output 2: Init 2: 4

Debug 1: Assert 2: 4
Trace 1: Assert 2: 4
Output 1: Assert 2: 4
Debug 2: Assert 2: 4
Trace 2: Assert 2: 4
Output 2: Assert 2: 4

___ Test case OK _________

*** Test case 2 ***

Debug 1: Init 2: 5
Trace 1: Init 2: 5
Output 1: Init 2: 5
Debug 2: Init 2: 5
Trace 2: Init 2: 5
Output 2: Init 2: 5

Debug 1: Assert 2: 5
Trace 1: Assert 2: 5
Output 1: Assert 2: 5
Debug 2: Assert 2: 5
Trace 2: Assert 2: 5
Output 2: Assert 2: 5

___ Test case OK _________

*** Test case 3 ***

Debug 1: Init 2: 6
Trace 1: Init 2: 6
Output 1: Init 2: 6
Debug 2: Init 2: 6
Trace 2: Init 2: 6
Output 2: Init 2: 6

Debug 1: Assert 2: 6
Trace 1: Assert 2: 6
Output 1: Assert 2: 6
Debug 2: Assert 2: 6
Trace 2: Assert 2: 6
Output 2: Assert 2: 6

___ Test case OK _________

Debug 1: After 2
Trace 1: After 2
Output 1: After 2
Debug 2: After 2
Trace 2: After 2
Output 2: After 2");
      }

      [Test]
      public void T03()
      {
         AssertInvoke.That(() => _ut.T03(), @"
Debug 1: Before 3
Trace 1: Before 3
Output 1: Before 3
Debug 2: Before 3
Trace 2: Before 3
Output 2: Before 3

*** Test case 1 ***

Debug 1: Init: X
Trace 1: Init: X
Output 1: Init: X
Debug 2: Init: X
Trace 2: Init: X
Output 2: Init: X

Debug 1: Assert: X
Trace 1: Assert: X
Output 1: Assert: X
Debug 2: Assert: X
Trace 2: Assert: X
Output 2: Assert: X

___ Test case OK _________

Debug 1: After 3
Trace 1: After 3
Output 1: After 3
Debug 2: After 3
Trace 2: After 3
Output 2: After 3");
      }
   }

   [Ignore]
   internal sealed class OutputTestsInternal
   {
      private Runner _runner;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(this);
      }

      private void WriteOutput(string message)
      {
         Debug.WriteLine("Debug 1: " + message);
         Trace.WriteLine("Trace 1: " + message);
         Console.WriteLine("Output 1: " + message);

         Debug.WriteLine("Debug 2: " + message);
         Trace.WriteLine("Trace 2: " + message);
         Console.WriteLine("Output 2: " + message);
      }

      [Test]
      public void T01()
      {
         WriteOutput("Before 1");
         _runner.Run(
            () => new[] { 1, 2, 3 },
            i => WriteOutput(string.Format("Init 1: {0}", i)),
            i => WriteOutput(string.Format("Assert 1: {0}", i)));
         WriteOutput("After 1");
      }

      [Test]
      public void T02()
      {
         WriteOutput("Before 2");
         _runner.Run(
            () => new[] { 4, 5, 6 },
            i => WriteOutput(string.Format("Init 2: {0}", i)),
            i => WriteOutput(string.Format("Assert 2: {0}", i)));
         WriteOutput("After 2");
      }

      [Test]
      public void T03()
      {
         WriteOutput("Before 3");
         _runner.Run(
            () => WriteOutput("Init: X"),
            () => WriteOutput("Assert: X"));
         WriteOutput("After 3");
      }
   }
}
