using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickTestsFramework.MSTest
{
   internal sealed class MSTestHelper
   {
      private static Assembly s_nUnitAssembly;

      public static Assembly GetAssembly()
      {
         if (s_nUnitAssembly == null)
         {
            const string assemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
            s_nUnitAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetName().Name == assemblyName) ??
                              Assembly.Load(new AssemblyName { Name = assemblyName });
         }

         return s_nUnitAssembly;
      }

      public static Type GetType(string name)
      {
         return GetAssembly().GetType(name, throwOnError: true);
      }

      public static object InvokeStatic(string typeName, string methodName, params object[] args)
      {
         try
         {
            return GetType(typeName)
               .GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, null, args.Select(i => i.GetType()).ToArray(), null)
               .Invoke(null,  args);
         }
         catch (TargetInvocationException ex)
         {
            throw ex.InnerException;
         }
      }

      public static bool IsTypeOf(object obj, string typeName)
      {
         return GetType(typeName).IsInstanceOfType(obj);
      }
   }
}