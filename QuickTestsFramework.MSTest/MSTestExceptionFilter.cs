using System;
using System.Collections.Generic;
using System.Linq;
using QuickTestsFramework.Internals;

namespace QuickTestsFramework.MSTest
{
   public sealed class MSTestExceptionFilter : IExceptionFilter
   {
      private const string FAILED_EXCEPTION = "Microsoft.VisualStudio.TestTools.UnitTesting.AssertFailedException";
      private const string INCONCLUSIVE_EXCEPTION = "Microsoft.VisualStudio.TestTools.UnitTesting.AssertInconclusiveException";
      private readonly bool _printStacktrace;

      public MSTestExceptionFilter(bool printStacktrace = true)
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
         return
            MSTestHelper.IsTypeOf(exception, FAILED_EXCEPTION)
            || MSTestHelper.IsTypeOf(exception, INCONCLUSIVE_EXCEPTION);
      }

      public bool IsSuccessException(Exception exception)
      {
         return false;
      }

      public bool IsIgnoreException(Exception exception)
      {
         return false;
      }

      public bool IsInconclusiveException(Exception exception)
      {
         return MSTestHelper.IsTypeOf(exception, INCONCLUSIVE_EXCEPTION);
      }

      /// <summary>
      /// Cut unnecesary stacktrace.
      /// </summary>
      /// <param name="exception"></param>
      public void FilterExceptionThrownByAssertionRunner(Exception exception)
      {
         if (MSTestHelper.IsTypeOf(exception, FAILED_EXCEPTION) || MSTestHelper.IsTypeOf(exception, INCONCLUSIVE_EXCEPTION))
            throw exception;
      }
   }
}