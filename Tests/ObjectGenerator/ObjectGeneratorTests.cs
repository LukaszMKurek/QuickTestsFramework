using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;

namespace QuickTestsFramework
{
    public sealed class A
    {
        public int p1;
        public B p2;
    }

    public sealed class B
    {
        public int p1;
        public string p2;
    }

    [TestFixture]
    public sealed class Class1
    {
        [Test]
        public void T01()
        {
            int l = 3;
            var generator = new ObjectGenerator(new TestApiAdapter());
            var results = generator.Pairwise(i =>
                new A
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new B
                    {
                        p1 = One.Of(1, 2) == 1 ? 2 : 3,
                        p2 = "3"
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(6));
        }

        [Test]
        public void T02()
        {
            int l = 3;
            var generator = new ObjectGenerator(new TestApiAdapter());
            var results = generator.Pairwise2(i =>
                new A
                {
                    p1 = One.Of(1, 2 * l, 3),
                    p2 = new B
                    {
                        p1 = One.Of(1, 2),
                        p2 = One.Of("1", "2", "3")
                    }
                }).ToArray();

            Assert.That(results.Length, Is.EqualTo(9));
        }
    }
}
