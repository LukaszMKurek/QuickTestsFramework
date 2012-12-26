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
            AddListenerIfNotExist(Debug.Listeners, writer);
            AddListenerIfNotExist(Trace.Listeners, writer);
            action();
         }
         finally
         {
            RemoveListenerIfExist(Trace.Listeners, writer);
            RemoveListenerIfExist(Debug.Listeners, writer);
         }
      }

      private static void AddListenerIfNotExist(TraceListenerCollection traceListeners, TextWriterTraceListener writer)
      {
         if (traceListeners.OfType<TextWriterTraceListener>().Any(tw => tw.Writer == Console.Out) == false)
            traceListeners.Add(writer);
      }

      private static void RemoveListenerIfExist(TraceListenerCollection traceListeners, TextWriterTraceListener writer)
      {
         if (traceListeners.OfType<TextWriterTraceListener>().Any(tw => tw.Writer == Console.Out) == false)
            traceListeners.Add(writer);
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