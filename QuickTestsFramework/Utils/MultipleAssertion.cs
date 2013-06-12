using System;
using System.Collections.Generic;
using System.Linq;

namespace QuickTestsFramework
{
   public sealed class MultipleAssertion
   {
      private readonly IExceptionFilter _exceptionFilter;
      private readonly IAssertionAction _assertionAction;

      public MultipleAssertion(IExceptionFilter exceptionFilter, IAssertionAction assertionAction)
      {
         _exceptionFilter = exceptionFilter;
         _assertionAction = assertionAction;
      }

      // użucie expression może 
      public void Run(params Action[] actions)
      {
         Run((IEnumerable<Action>)actions);
      }

      public void Run(IEnumerable<Action> actions)
      {
         if (actions == null)
            throw new ArgumentNullException("actions");
         
         var exceptions = new List<int>();
         int i = 1;
         foreach (var action in actions)
         {
            try
            {
               action();
            }
            catch (Exception ex)
            {
               string messageToDisplay = _exceptionFilter.RenderExceptionConditional(ex);
               if (_exceptionFilter.IsSuccessException(ex))
               {
                  Console.WriteLine("Assertion {0} PASS:\r\n{1}\r\n", i, messageToDisplay);
               }
               else if (_exceptionFilter.IsIgnoreException(ex))
               {
                  Console.WriteLine("Assertion {0} IGNORE:\r\n{1}\r\n", i, messageToDisplay);
               }
               else
               {
                  exceptions.Add(i);

                  Console.WriteLine("Assertion {0} FAIL:\r\n{1}\r\n", i, messageToDisplay);
               }
            }
            i++;
         }

         if (exceptions.Count == 0)
            return;

         _assertionAction.Fail(string.Format("Assertions: {0} failed.", string.Join(", ", exceptions)));
      }
   }
}
