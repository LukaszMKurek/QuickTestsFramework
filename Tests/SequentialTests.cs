using System.Linq;
using System.Collections.Generic;
using System;
using NUnit.Framework;

namespace QuickTestsFramework.Tests
{
   [TestFixture]
   public sealed class SequentialTests
   {
      [Test]
      public void T01()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = 1,
                  B = 2,
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"{ A = 1, B = 2, C = 7 }");
      }

      [Test]
      public void T02()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1, 2, 3), 
                  B = _.OneOf(4m, 5m, 7m),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"
{ A = 1, B = 4, C = 7 }
{ A = 2, B = 5, C = 7 }
{ A = 3, B = 7, C = 7 }");
      }

      [Test]
      public void T03()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1),
                  B = _.OneOf(4m),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"{ A = 1, B = 4, C = 7 }");
      }

      [Test]
      public void T04()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1, 2),
                  B = _.OneOf(4m, 5m),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"
{ A = 1, B = 4, C = 7 }
{ A = 2, B = 5, C = 7 }");
      }

      [Test]
      public void T05()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf<int>(),
                  B = _.OneOf<decimal>(),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"", typeof(InvalidOperationException), @"Length of array must be grather than zero.");
      }

      [Test]
      public void T06()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1, 2, 3),
                  B = _.OneOf(4m, 5m),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"", typeof(InvalidOperationException), @"Length of all arrays must be equal.");
      }

      [Test]
      public void T07()
      {
         AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1, 5),
                  C = 7.0,
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"
{ A = 1, C = 7 }
{ A = 5, C = 7 }");
      }

      [Test]
      public void T08()
      {
          AssertInvoke.That(
            () =>
            {
               var res = Sequential.New(_ => new
               {
                  A = _.OneOf(1, 5),
                  B = _.OneOf(3, 7),
                  C = _.OneOf(2, 9),
               });

               foreach (var re in res)
               {
                  Console.WriteLine(re);
               }
            }, @"
{ A = 1, B = 3, C = 2 }
{ A = 5, B = 7, C = 9 }");
      }
   }
}