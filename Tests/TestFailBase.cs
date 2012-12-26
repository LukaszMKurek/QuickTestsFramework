using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   internal abstract class TestFailBase
   {
      private Runner _qt;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _qt = RunnerHelper.Create();
         _qt.RunInitializers(this);
         Console.WriteLine("Process...");
      }

      protected abstract void ThrowException();

      [Test]
      public void ExceptionInInicjalizer()
      {
         _qt.Run(
            () => new[] { 1, 2, 5, 7, 9 },
            tc =>
            {
               Console.WriteLine("ExceptionInInicjalizer; init; tc: {0}", tc);
               if (tc == 2 || tc == 7)
                  ThrowException();
            },
            tc => Console.WriteLine("ExceptionInInicjalizer; assert; tc: {0}", tc));
      }

      [Test]
      public void ExceptionInAserrt()
      {
         _qt.Run(
            () => new[] { 1, 2, 5, 7, 9 },
            tc => Console.WriteLine("ExceptionInAserrt; init; tc: {0}", tc),
            tc =>
            {
               Console.WriteLine("ExceptionInAserrt; assert; tc: {0}", tc);
               if (tc == 2 || tc == 7)
                  ThrowException();
            });
      }

      [Test]
      public void ExceptionOnStartTestCaseGeneration()
      {
         _qt.Run(
            () =>
            {
               Console.WriteLine("ExceptionOnStartTestCaseGeneration;");
               ThrowException();
               return new int[0];
            },
            tc => Console.WriteLine("ExceptionInAserrt; init; tc: {0}", tc),
            tc => Console.WriteLine("ExceptionInAserrt; assert; tc: {0}", tc));
      }

      [Test]
      public void ExceptionOnYieldTestCaseGeneration()
      {
         _qt.Run(
            Initialize,
            tc => Console.WriteLine("ExceptionInAserrt; init; tc: {0}", tc),
            tc => Console.WriteLine("ExceptionInAserrt; assert; tc: {0}", tc));
      }

      private IEnumerable<int> Initialize()
      {
         {
            Console.WriteLine("ExceptionOnYieldTestCaseGeneration; 1");
            yield return 1;
            Console.WriteLine("ExceptionOnYieldTestCaseGeneration; 2");
            ThrowException();
            yield return 2;
         }
      }

      [Test]
      public void ExceptionBeforeRun()
      {
         Console.WriteLine("ExceptionBeforeRun; before;");
         ThrowException();

         _qt.Run(
            () => new[] { 1, 2, 5 },
            tc => Console.WriteLine("ExceptionBeforeRun; init; tc: {0}", tc),
            tc => Console.WriteLine("ExceptionBeforeRun; assert; tc: {0}", tc));

         Console.WriteLine("ExceptionBeforeRun; after;");
      }

      [Test]
      public void ExceptionAfterRun()
      {
         Console.WriteLine("ExceptionAfterRun; before;");
         
         _qt.Run(
            () => new[] { 1, 2, 5 },
            tc => Console.WriteLine("ExceptionBeforeRun; init; tc: {0}", tc),
            tc => Console.WriteLine("ExceptionBeforeRun; assert; tc: {0}", tc));

         Console.WriteLine("ExceptionAfterRun; after;");
         ThrowException();
      }
   }

   [Ignore]
   internal sealed class ThrowAssertFailTestsInternal : TestFailBase
   {
      protected override void ThrowException()
      {
         Assert.Fail("Assert.Fail message");
      }
   }

   [Ignore]
   internal sealed class ThrowAssertIgnoreTestsInternal : TestFailBase
   {
      protected override void ThrowException()
      {
         Assert.Ignore("Assert.Ignore message");
      }
   }

   [Ignore]
   internal sealed class ThrowAssertPassTestsInternal : TestFailBase
   {
      protected override void ThrowException()
      {
         Assert.Pass("Assert.Pass message");
      }
   }

   [Ignore]
   internal sealed class ThrowExceptionTestsInternal : TestFailBase
   {
      protected override void ThrowException()
      {
         throw new Exception("exception");
      }
   }
}