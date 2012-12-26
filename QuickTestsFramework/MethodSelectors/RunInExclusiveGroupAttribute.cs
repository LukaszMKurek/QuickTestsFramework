using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
   public sealed class RunInExclusiveGroupAttribute : Attribute
   { }
}