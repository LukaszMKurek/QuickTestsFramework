using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [Ignore]
   internal sealed class NullArgumentTestsInternal
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
         Console.WriteLine("before");
         _qt.Run(
            (Func<int[]>)null,
            null,
            null);
      }

      [Test]
      public void T02()
      {
         Console.WriteLine("before");
         _qt.Run(
            () => { Console.WriteLine("init"); return new[] { 1, 2 }; },
            null,
            null);
      }

      [Test]
      public void T03()
      {
         Console.WriteLine("before");
         _qt.Run(
            () => { Console.WriteLine("init"); return new[] { 1, 2 }; },
            tc => Console.WriteLine("T03: init; tc: {0}", tc),
            null);
      }

      private bool _firstRunT04 = true;
      [Test]
      public void T04()
      {
         Console.WriteLine("before");
         _qt.Run(
            () => { Console.WriteLine("init"); return new[] { 1, 2 }; },
            _firstRunT04 ? (tc => Console.WriteLine("T04: init; tc: {0}", tc)) : (Action<int>)null,
            null);
         _firstRunT04 = false;
      }
      
      [Test]
      public void T05()
      {
         Console.WriteLine("before");
         _qt.Run(
            () => (int[])null,
            tc => Console.WriteLine("T05: init"),
            tc => Console.WriteLine("T05: assert"));
      }
   }
}