using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System;

namespace QuickTestsFramework
{
   public static class Catch
   {
      private static string ConsoleOutput(Action action)
      {
         TextWriter tmp = Console.Out;
         var sw = new StringWriter();

         try
         {
            Console.SetOut(sw);
            action();
            return sw.ToString();
         }
         finally
         {
            Console.SetOut(tmp);
            sw.Dispose();
         }
      }

      private static void RedirectTraceAndDebugOutputToConsole(Action action)
      {
         var writer = new TextWriterTraceListener(Console.Out);
         try
         {
            AddDebugListenerIfNotExist(writer);
            AddTraceListenerIfNotExist(writer);

            action();
         }
         finally
         {
            RemoveTraceListenerIfExist(writer);
            RemoveDebugListenerIfExist(writer);
         }
      }

      [Conditional("DEBUG")]
      private static void AddDebugListenerIfNotExist(TextWriterTraceListener writer)
      {
         if (Debug.Listeners.OfType<TextWriterTraceListener>().Any(tw => tw.Writer == Console.Out) == false)
            Debug.Listeners.Add(writer);
      }

      [Conditional("DEBUG")]
      private static void RemoveDebugListenerIfExist(TextWriterTraceListener writer)
      {
         Debug.Listeners.Remove(writer);
      }

      [Conditional("TRACE")]
      private static void AddTraceListenerIfNotExist(TextWriterTraceListener writer)
      {
         if (Trace.Listeners.OfType<TextWriterTraceListener>().Any(tw => tw.Writer == Console.Out) == false)
            Trace.Listeners.Add(writer);
      }

      [Conditional("TRACE")]
      private static void RemoveTraceListenerIfExist(TextWriterTraceListener writer)
      {
         Trace.Listeners.Remove(writer);
      }

      public static string Output(Action action)
      {
         return ConsoleOutput(() => RedirectTraceAndDebugOutputToConsole(action));
      }

      public static Exception Exception(Action action)
      {
         try
         {
            action();
         }
         catch (Exception ex)
         {
            return ex;
         }

         return null;
      }
   }
}