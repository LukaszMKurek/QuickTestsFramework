using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using NUnit.Framework;

namespace QuickTestsFramework.Tests.Demo
{
   [TestFixture]
   [Explicit]
   public sealed class Demo03MoreRealisticTests
   {
      private Runner _runner;
      private readonly List<Client> _clients = new List<Client>();
      private readonly List<Product> _products = new List<Product>();

      [TestFixtureSetUp]
      public void SetUp()
      {
         _runner = RunnerHelper.Create();
         _runner.RunInitializers(this);

         BatchProcess(_clients, _products); // this process can be very slow, but will be called only once. We assumed that data from each tests are completly disjunctive.

         // data from _products and _clients we can write to database when process use database for input and output.
         // after and of batch process all data from database can be read to lists for quick assertion.
      }

      [Test]
      public void T01()
      {
         _runner.Run(
            inicializer: () => 
            {
               var client = new Client {
                  Name = "c1",
                  Status = 0
               };
               var product = new Product {
                  Name = "p1",
                  Client = client,
                  Status = 0
               };

               _clients.Add(client);
               _products.Add(product);

               DumpStatuses(product);
            }, 
            assertion: () =>
            {
               var product = _products.First(p => p.Name == "p1"); // we are responsilbe for proper identyficating objects. 
               DumpStatusesAfter(product);

               Assert.That(product.Status, Is.EqualTo(0));
               Assert.That(product.Client.Status, Is.EqualTo(0));
            });
      }

      [Test]
      public void T02()
      {
         _runner.Run(
            inicializer: () =>
            {
               var client = new Client {
                  Name = "c2",
                  Status = 97
               };
               var product = new Product {
                  Name = "p2",
                  Client = client,
                  Status = 0
               };

               _clients.Add(client);
               _products.Add(product);

               DumpStatuses(product);
            },
            assertion: () =>
            {
               var product = _products.First(p => p.Name == "p2"); 
               DumpStatusesAfter(product);

               Assert.That(product.Status, Is.EqualTo(1));
               Assert.That(product.Client.Status, Is.EqualTo(7));
            });
      }

      private static void DumpStatuses(Product product)
      {
         Console.WriteLine("Product.Status: {0}", product.Status);
         Console.WriteLine("Product.Client.Status: {0}", product.Client.Status);
      }

      private static void DumpStatusesAfter(Product product)
      {
         Console.WriteLine("After: ");
         DumpStatuses(product);
      }

      private static void BatchProcess(List<Client> clients, List<Product> products)
      {
         foreach (var product in products)
         {
            if (product.Status == 0 && product.Client.Status == 97)
            {
               product.Status = 1;
               product.Client.Status = 7;
            }

            Thread.Sleep(500); // very slow procees...
         }
      }
   }

   public sealed class Client
   {
      public string Name;
      public int Status;
   }

   public sealed class Product
   {
      public string Name;
      public int Status;
      public Client Client;
   }
}
