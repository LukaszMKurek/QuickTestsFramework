using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public sealed class Sequential
   {
      private bool _firstLoop = true;
      private int _currLoop;
      private int _currCall;
      private int _n = -1;
      private readonly List<object> _arr = new List<object>();

      private Sequential()
      {}

      public static IEnumerable<T> New<T>(Func<Sequential, T> action)
      {
         if (action == null)
            throw new ArgumentNullException("action");

         var sequential = new Sequential();
         
         yield return action(sequential);

         sequential._firstLoop = false;
         sequential._currLoop += 1;

         while (sequential._currLoop < sequential._n)
         {
            sequential._currCall = 0;
            yield return action(sequential);
            sequential._currLoop += 1;
         }
      }

      public T OneOf<T>(params T[] array)
      {
         if (_firstLoop)
         {
            if (array == null)
               throw new ArgumentNullException("array");

            if (array.Length == 0)
               throw new InvalidOperationException("Length of array must be grather than zero.");

            if (_n != -1 && _n != array.Length)
               throw new InvalidOperationException("Length of all arrays must be equal.");

            _n = array.Length;
            _arr.Add(array);
         }

         return ((T[])_arr[_currCall++])[_currLoop];
      }
   }
}