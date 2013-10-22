using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework.Internals
{
   public interface IExceptionFilter
   {
      string RenderExceptionConditional(Exception exception);
      string RenderException(Exception exception);
      bool IsSuccessException(Exception exception);
      bool IsIgnoreException(Exception exception);
      bool IsInconclusiveException(Exception exception);
      void FilterExceptionThrownByAssertionRunner(Exception exception);
   }
}