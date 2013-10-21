using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.NUnit
{
   public sealed class NUnitExceptionFilter : IExceptionFilter
   {
      private readonly bool _printStacktrace;

      public NUnitExceptionFilter(bool printStacktrace = true)
      {
         _printStacktrace = printStacktrace;
      }

      public string RenderExceptionConditional(Exception exception)
      {
         if (IsNunitException(exception))
            return exception.Message;

         return RenderException(exception);
      }

      public string RenderException(Exception exception)
      {
         if (_printStacktrace)
            return exception.ToString();

         return exception.Message;
      }

      private bool IsNunitException(Exception exception)
      {
         //return exception is SuccessException || exception is AssertionException || exception is IgnoreException;
         return
            NUnitHelper.IsTypeOf(exception, "NUnit.Framework.SuccessException")
            || NUnitHelper.IsTypeOf(exception, "NUnit.Framework.AssertionException")
            || NUnitHelper.IsTypeOf(exception, "NUnit.Framework.IgnoreException");
      }

      public bool IsSuccessException(Exception exception)
      {
         //return exception is SuccessException;
         return NUnitHelper.IsTypeOf(exception, "NUnit.Framework.SuccessException");
      }

      public bool IsIgnoreException(Exception exception)
      {
         //return exception is IgnoreException;
         return NUnitHelper.IsTypeOf(exception, "NUnit.Framework.IgnoreException");
      }

      /// <summary>
      /// Cut unnecesary stacktrace.
      /// </summary>
      /// <param name="exception"></param>
      public void FilterExceptionThrownByAssertionRunner(Exception exception)
      {
         //if (exception is AssertionException || exception is IgnoreException)
         if (NUnitHelper.IsTypeOf(exception, "NUnit.Framework.AssertionException") || NUnitHelper.IsTypeOf(exception, "NUnit.Framework.IgnoreException"))
            throw exception;
      }
   }
}