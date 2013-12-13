using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;
using QuickTestsFramework.NUnit;

namespace QuickTestsFramework.Tests.Demo
{
   [TestFixture]
   [Explicit]
   public sealed class Demo02Tests
   {
      private Runner _runner;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(this);

         Console.WriteLine("Process...");
      }

      [Test]
      public void T01()
      {
         _runner.Run(
            () => new[] { new { A = 1, B = 2 }, new { A = 5, B = 7 }, new { A = 6, B = 9 } }, // Generate test cases. It starting before initializing delegate. 
            tc =>
            {
               Console.WriteLine("Init T01: tc = {0}", tc);
               // initialize state...
            },
            tc =>
            {
               Console.WriteLine("Verifi T01: tc = {0}", tc);
               Assert.That(tc.A, Is.Not.EqualTo(5)); // simulate one assertion fail.
            });
      }

      [Test]
      [RunInExclusiveGroup]
      public void T02()
      {
         _runner.Run(
            () => Enumerable.Range(0, 4),
            tc =>
            {
               Console.WriteLine("Init T01: tc = {0}", tc);
               return new { TestCaseValue = tc, ValueFromInitializer = tc*tc };
            },
            tc =>
            {
               Console.WriteLine("Verifi T01: tc = {0}", tc);
               Assert.That(tc.ValueFromInitializer, Is.Not.EqualTo(4)); // simulate one assertion fail.
            });
      }

      [Test]
      [RunInExclusiveGroup]
      public void T03()
      {
         var random = new Random();
         _runner.Run(
            () => Sequential.New(_ => new
            {
               A = _.OneOf(1, 3, 5, 8, 5),
               B = _.OneOf(1, 4, 2, 8, 3),
               C = "const",
               D = random.Next(100)
            }),
            tc =>
            {
               Console.WriteLine("Init T01: tc = {0}", tc);
            },
            tc =>
            {
               Console.WriteLine("Verifi T01: tc = {0}", tc);
               Assert.That(tc.A, Is.Not.EqualTo(5)); // simulate one assertion fail.
            });
      }

      [Test]
      [TraditionalTest] // this mean that this test run only in one stage like normal NUnit tests.
      public void T04()
      {
         var multiAssert = new MultipleAssertion(new NUnitExceptionFilter(), new NUnitAssertionAction());

         multiAssert.Run( // all assertion will run.
            () => Assert.True(true),
            () => Assert.True(false),
            () => Assert.True(true),
            () => Assert.True(false));
      }
   }
}