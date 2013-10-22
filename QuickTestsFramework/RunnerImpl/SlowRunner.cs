using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickTestsFramework.Internals
{
   /// <summary>
   /// Umożliwia uruchamianie testów szybkich w sposób konwencjonalny, np. na potrzeby debugowania testów.
   /// </summary>
   internal sealed class SlowRunner
   {
      private readonly Action _action;

      /// <summary>
      /// Umożliwia uruchamianie testów szybkich w sposób konwencjonalny, np. na potrzeby debugowania testów.
      /// </summary>
      internal SlowRunner(Action action)
      {
         _action = action;
      }

      internal void Run(Action inicializer, Action assertion)
      {
         if (inicializer == null)
            throw new ArgumentNullException("inicializer");
         if (assertion == null)
            throw new ArgumentNullException("assertion");

         Console.WriteLine("*** SLOW MODE ENABLED ***\r\n");
         inicializer();
         Console.WriteLine();
         _action();
         Console.WriteLine();
         assertion();
      }

      internal void Run<T>(Func<IEnumerable<T>> testCaseGenerator, Action<T> inicializer, Action<T> assertion)
      {
         if (testCaseGenerator == null)
            throw new ArgumentNullException("testCaseGenerator");
         if (inicializer == null)
            throw new ArgumentNullException("inicializer");
         if (assertion == null)
            throw new ArgumentNullException("assertion");

         Console.WriteLine("*** SLOW MODE ENABLED ***\r\n");

         int n = 0;
         foreach (var testCase in testCaseGenerator())
         {
            Console.WriteLine("*** Test case " + n++ + "\r\n");
            inicializer(testCase);
            Console.WriteLine();
            _action();
            Console.WriteLine();
            assertion(testCase);
            Console.WriteLine("\r\n*** SLOW MODE ENABLED ***\r\n");
         }
      }
   }
}