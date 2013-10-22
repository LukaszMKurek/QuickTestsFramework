using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuickTestsFramework.MSTest.Tests
{
   [TestClass]
   public class EmptyTest
   {
      private static Runner _runner;

      [ClassInitialize]
      public static void SetUp(TestContext test)
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(new EmptyTest());

         // consolidate data from initializers and execute batch process here
      }

      [TestMethod]
      public void T01()
      {
         _runner.Run(
             inicializer: () =>
             {

             },
             assertion: () =>
             {

             });
      }

      [TestMethod]
      public void T02_parametrized()
      {
         _runner.Run(
             testCaseGenerator: () => Enumerable.Range(0, 10),
             inicializer: testCase =>
             {

             },
             assertion: testCase =>
             {

             });
      }
   }
}
