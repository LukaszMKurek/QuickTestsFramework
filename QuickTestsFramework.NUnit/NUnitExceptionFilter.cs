using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

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
         return exception is SuccessException || exception is AssertionException || exception is IgnoreException;
      }

      public bool IsSuccessException(Exception exception)
      {
         return exception is SuccessException;
      }

      public bool IsIgnoreException(Exception exception)
      {
         return exception is IgnoreException;
      }

      /// <summary>
      /// Cut unnecesary stacktrace.
      /// </summary>
      /// <param name="exception"></param>
      public void FilterExceptionThrownByAssertionRunner(Exception exception)
      {
         if (exception is AssertionException || exception is IgnoreException)
            throw exception;
      }
   }
}
