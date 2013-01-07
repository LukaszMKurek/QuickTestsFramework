using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [Ignore]
   public sealed class NUnitSpecificAndOtherTestsInternal : NUnitSpecificAndOtherTestsBaseInternal
   {
      private Runner _qt;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _qt = RunnerHelper.Create();
         _qt.RunInitializers(this);
         Console.WriteLine("Process...");
      }

      [Test]
      public void T01()
      {
         _qt.Run(
            () => new object[0],
            tc => Console.WriteLine("T01: init"),
            tc => Console.WriteLine("T01: assert"));
      }
      
      [Test]
      public void T02()
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T02: init; tc: {0}", tc),
            tc => Console.WriteLine("T02: assert; tc: {0}", tc));
      }
      
      [Test]
      [Ignore]
      public void T03()
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T03: init; tc: {0}", tc),
            tc => Console.WriteLine("T03: assert; tc: {0}", tc));
      }

      [TestCase(1)]
      public void T04(int i)
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T04: init; tc: {0}", tc),
            tc => Console.WriteLine("T04: assert; tc: {0}", tc));
      }

      [Test]
      public void T05([Values(1, 2)] int i)
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T05: init; tc: {0}", tc),
            tc => Console.WriteLine("T05: assert; tc: {0}", tc));
      }

      public void T06()
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T06: init; tc: {0}", tc),
            tc => Console.WriteLine("T06: assert; tc: {0}", tc));
      }

      private int _t07;
      [Test]
      [TraditionalTest]
      public void T07()
      {
         Console.WriteLine("T07: before {0}", _t07++);
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T07: init; tc: {0}", tc),
            tc => Console.WriteLine("T07: assert; tc: {0}", tc));
         Console.WriteLine("T07: after {0}", _t07++);
      }

      private int _t08;
      [Test]
      [TraditionalTest]
      public void T08()
      {
         Console.WriteLine("T08: test {0}", _t08++);
      }

      private int _t09;
      [Test]
      public void T09()
      {
         Console.WriteLine("T09: before {0}", _t09++);
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T09: init; tc: {0}", tc),
            tc => Console.WriteLine("T09: assert; tc: {0}", tc));
         Console.WriteLine("T09: after {0}", _t09++);
      }

      private int _t10;
      [Test]
      // this test should fail because does not have [TraditionalTest] attribute nor Run method call.
      // but not fail because is hard do to. One posibility that I see i adding some code at runtime or eventualy
      // at post compile IL rewrite process.
      public void T10()
      {
         Console.WriteLine("T10: without Run {0}", _t10++);
      }

      private int _t11;
      [Test]
      public void T11()
      {
         Console.WriteLine("T11: without Run {0}", _t11++);
         Assert.Fail("bo tak");
      }

      [Test]
      public void T12()
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T12: init; tc: {0}", tc),
            tc => Console.WriteLine("T12: assert; tc: {0}", tc));
      }

      [Test]
      private void T13()
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T13: init; tc: {0}", tc),
            tc => Console.WriteLine("T13: assert; tc: {0}", tc));
      }

      [Test]
      public void T14()
      {
         Txx(14);
      }

      [Test]
      public void T15()
      {
         Txx(15);
      }

      private void Txx(int x)
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T{0}: init; tc: {1}", x, tc),
            tc => Console.WriteLine("T{0}: assert; tc: {1}", x, tc));
      }

      [TestCase(1, 2)]
      public void T16(int i, int j)
      {
         _qt.Run(
            () => new int[1],
            tc => Console.WriteLine("T04: init; tc: {0}", tc),
            tc => Console.WriteLine("T04: assert; tc: {0}", tc));
      }
   }

   [Ignore]
   public class NUnitSpecificAndOtherTestsBaseInternal
   {
      [TestCase(1, 2)]
      public void T16(int i, int j)
      {
      }
   }
}