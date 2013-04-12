using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace QuickTestsFramework
{
    /// <summary>
    /// Catch and return exception or console, trace, debug output.
    /// </summary>
    public static class Catch
    {
        /// <summary>
        /// Name of trace listener that was used during catching output. You may use this information to add some filter. But remember if any listener redirected output from console costom listener won't by add.
        /// </summary>
        public static readonly string TraceListenerName = "QuickTestFrameworkTraceListener";

        /// <summary>
        /// You can decide to disable caching output from Debug and Trace.
        /// </summary>
        public static bool CatchCanHandleTraceAndDebug = true;

        /// <summary>
        /// Catch and return Console output in region.
        /// </summary>
        public static string ConsoleOutput(Action action)
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
            var writer = new TextWriterTraceListener(Console.Out, TraceListenerName);
            try
            {
                AddTraceListenerIfNotExist(writer);

                action();
            }
            finally
            {
                RemoveTraceListenerIfExist(writer);
            }
        }

        private static void AddTraceListenerIfNotExist(TextWriterTraceListener writer)
        {
            if (CatchCanHandleTraceAndDebug && Trace.Listeners.OfType<TextWriterTraceListener>().Any(tw => tw.Writer == Console.Out) == false)
                Trace.Listeners.Add(writer);
        }

        private static void RemoveTraceListenerIfExist(TextWriterTraceListener writer)
        {
            Trace.Listeners.Remove(writer);
        }

        /// <summary>
        /// Catch and return console, trace, debug output in region. Depending on confuguration. See:
        /// </summary>
        public static string Output(Action action)
        {
            return ConsoleOutput(() => RedirectTraceAndDebugOutputToConsole(action));
        }

        /// <summary>
        /// Catch and return exception in region.
        /// </summary>
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
