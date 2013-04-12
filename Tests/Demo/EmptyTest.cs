using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuickTestsFramework;

namespace QuickTestsFramework.Tests
{
    [TestFixture]
    public sealed class EmptyTest
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
                testCaseGenerator: () => Enumerable.Range(0, 10),
                inicializer: tc =>
                {
                    
                },
                assertion: tc =>
                {
                    
                });
        }
    }
}
