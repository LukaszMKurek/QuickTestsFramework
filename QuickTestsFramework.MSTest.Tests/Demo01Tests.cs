using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickTestsFramework.MSTest.Tests
{
   [TestClass]
   public class Demo01Tests
   {
      private static Runner _runner;

      [ClassInitialize]
      public static void SetUp(TestContext test)
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(new Demo01Tests());

         // consolidate data from initializers and execute batch process here
      }

      [TestMethod]
      public void T01()
      {
         _runner.Run(
            inicializer: () =>
            {
               Console.WriteLine("init...");
            },
            assertion: () =>
            {
               Console.WriteLine("assert...");
            });
      }

      [TestMethod]
      public void T02_parametrized()
      {
         _runner.Run(
            testCaseGenerator: () => Enumerable.Range(0, 10),
            inicializer: testCase =>
            {
               Assert.Inconclusive();
            },
            assertion: testCase =>
            {

            });
      }

      [TestMethod]
      public void T03_parametrized()
      {
         _runner.Run(
            testCaseGenerator: () => Enumerable.Range(0, 10),
            inicializer: testCase =>
            {
               
            },
            assertion: testCase =>
            {
               Assert.Inconclusive();
            });
      }
   }
}