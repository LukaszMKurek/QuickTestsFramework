using System.Linq;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace QuickTestsFramework
{
   public sealed class MethodData
   {
      public MethodData()
      {
         Messages = "";
      }

      public string Messages { get; private set; }
      public bool HasError { get; private set; }
      public bool HasIgnore { get; private set; }
      public MethodInfo MethodInfo { get; set; }
      public bool WillExecute 
      {
         get { return HasError == false && HasIgnore == false; }
      }
      
      public void AddErrorMessage(string message)
      {
         Messages += message;
         HasError = true;
      }

      public void AddIgnoreMessage(string message)
      {
         Messages += message;
         HasIgnore = true;
      }
   }
}