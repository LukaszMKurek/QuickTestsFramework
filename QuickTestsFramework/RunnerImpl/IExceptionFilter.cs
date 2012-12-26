using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public interface IExceptionFilter
   {
      string RenderExceptionConditional(Exception exception);
      string RenderException(Exception exception);
      bool IsSuccessException(Exception exception);
      bool IsIgnoreException(Exception exception);
      void FilterExceptionThrownByAssertionRunner(Exception exception);
   }
}