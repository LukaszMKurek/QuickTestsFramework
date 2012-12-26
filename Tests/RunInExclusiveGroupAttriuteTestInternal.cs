using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [Ignore]
   public sealed class RunInExclusiveGroupAttriuteTestInternal
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
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T01: init; tc: {0}", tc),
            tc => Console.WriteLine("T01: assert; tc: {0}", tc));
      }

      [Test]
      [RunInExclusiveGroup]
      public void T02()
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T02: init; tc: {0}", tc),
            tc => Console.WriteLine("T02: assert; tc: {0}", tc));
      }

      [Test]
      public void T03()
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T03: init; tc: {0}", tc),
            tc => Console.WriteLine("T03: assert; tc: {0}", tc));
      }

      [Test]
      [RunInExclusiveGroup]
      public void T04()
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T04: init; tc: {0}", tc),
            tc => Console.WriteLine("T04: assert; tc: {0}", tc));
      }

      [TestCase(1)]
      [RunInExclusiveGroup]
      public void T05(int i)
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T05: init; tc: {0}", tc),
            tc => Console.WriteLine("T05: assert; tc: {0}", tc));
      }

      [Test]
      [RunInExclusiveGroup]
      [TraditionalTest]
      public void T06()
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T06: init; tc: {0}", tc),
            tc => Console.WriteLine("T06: assert; tc: {0}", tc));
      }

      [Test]
      [Ignore]
      [RunInExclusiveGroup]
      public void T07()
      {
         _qt.Run(
            () => new[] { 1, 2 },
            tc => Console.WriteLine("T07: init; tc: {0}", tc),
            tc => Console.WriteLine("T07: assert; tc: {0}", tc));
      }
   }
}