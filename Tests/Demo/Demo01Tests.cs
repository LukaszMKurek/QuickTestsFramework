using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework.Tests.Demo
{
   [TestFixture]
   [Explicit]
   public sealed class Demo01Tests
   {
      private Runner _runner;

      [TestFixtureSetUp]
      public void SetUp()
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(this); // Run all tests in fixture. For each test will be run code that initialize some "state".

         // At this place should be run a process that use "state".
         Console.WriteLine("Process...");
      }

      [Test]
      public void T01()
      {
         _runner.Run(
            () => // this delegate will be called before the process called in SetUp method.
            {
               Console.WriteLine("T01: Initialize input state."); 
               Trace.WriteLine("Trace T01: Initialize input state.");
               // Output from Console, Trace and Debug will be catched and stored internal until assertion delegate start.
               // Output of the test should look like there was no magic hidden underneath
            },
            () => // this delegate will be called after the procese called in SetUp method.
            {
               Console.WriteLine("T01: Analyze output state.");
               Trace.WriteLine("Trace T01: Analyze output state.");
            });
      }

      [Test]
      public void T02()
      {
         _runner.Run(
            () =>
            {
               Console.WriteLine("T02: Initialize input state.");
               Debug.WriteLine("Debug T02: Initialize input state.");
            },
            () =>
            {
               Console.WriteLine("T02: Analyze output state.");

               Assert.Fail("some reason...");
            });
      }
   }
}
