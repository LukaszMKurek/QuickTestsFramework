using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public interface IAssertionAction
   {
      void Ignore(string message);
      void Fail(string message);
   }
}