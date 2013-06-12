﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QuickTestsFramework
{
    public sealed class Runner
    {
       private bool _initialized;
       private readonly Dictionary<string, TestState> _testStates = new Dictionary<string, TestState>();
       private Dictionary<string, TestMethodInvoker> _testMethods;
       private readonly IExceptionFilter _exceptionFilter;
       private readonly IViewTestFixture _viewTestFixture;
       private readonly IInicjalizerView _inicjalizerView;
       private readonly TestSelector _testSelector;
       private readonly IAssertionAction _assertionAction;
       private readonly ITestMethodSelectorFromCallStack _testMethodSelectorFromCallStack;

       public Runner(IExceptionFilter exceptionFilter, IViewTestFixture viewTestFixture, IInicjalizerView inicjalizerView, TestSelector testSelector, IAssertionAction assertionAction, ITestMethodSelectorFromCallStack testMethodSelectorFromCallStack)
       {
          _exceptionFilter = exceptionFilter;
          _viewTestFixture = viewTestFixture;
          _inicjalizerView = inicjalizerView;
          _testSelector = testSelector;
          _assertionAction = assertionAction;
          _testMethodSelectorFromCallStack = testMethodSelectorFromCallStack;
       }

       /// <summary>
       /// Metoda odpowiedzialna za uruchomienie wszystkich (albo tylko wybranych) metod generujących przypadki testowe i metod inicujących dane.
       /// </summary>
       /// <param name="testFixtureInstance">Instancja klasy testu w którym uruchamiają się testy.</param>
       public void RunInitializers(object testFixtureInstance)
       {
          if (_initialized)
             throw new InvalidOperationException("Multiple initialization occurred.");

          IEnumerable<TestMethodInvoker> testsToRun = _testSelector.GetTestsToRun(testFixtureInstance);

          _testMethods = testsToRun.ToDictionary(x => GetMethodName(x.Method));

          foreach (var testMethod in _testMethods.Values)
          {
             if (testMethod.WillExecute)
             {
                try
                {
                   testMethod.Execute();
                }
                catch (TargetInvocationException ex)
                {
                   Action reportProblem = () => _viewTestFixture.ReportInitializationExcepton(ex.InnerException);
                   ReportProblem(GetMethodName(testMethod.Method), reportProblem);
                }
                catch (Exception ex)
                {
                   Action reportProblem = () => _viewTestFixture.ReportInitializationExcepton(ex);
                   ReportProblem(GetMethodName(testMethod.Method), reportProblem);
                }
             }
          }

          _initialized = true;
       }

       private static string GetMethodName(MethodBase method)
       {
          return method.DeclaringType.FullName + method.Name + "(" + string.Join(", ", method.GetParameters().Select(x => x.ParameterType.FullName)) + ")";
       }

       /// <summary>
       /// Uruchamia podane delegaty w zależności to fazy uruchamiania testu.
       /// </summary>
       /// <param name="inicializer">Delegata uruchamiana z TestFixtureSetup. Odpowiedzialna za zebranie danych początkowych dla procesu.</param>
       /// <param name="assertion">Delegata uruchamiana po zakończeniu procesu celem weryfikacji poprawności danych wujściowych.</param>
       public void Run(Action inicializer, Action assertion)
       {
          string methodName = GetNameOfRunningTest();
          if (_initialized == false)
          {
             if (inicializer == null)
             {
                ReportProblem(methodName, () => _viewTestFixture.ReportNullArgument("Inicjalizer nie może być nullem."));
                return;
             }

             RunInitializer(methodName, () => new object[1], i => inicializer());
             return;
          }

          if (assertion == null)
          {
             ReportProblem(methodName, _viewTestFixture.ReportAssertDelegateIsNull);
          }

          try
          {
             RunAssertion(methodName, (object i) => assertion());
          }
          catch (Exception ex)
          {
             _exceptionFilter.FilterExceptionThrownByAssertionRunner(ex);

             throw;
          }
       }

       private string GetNameOfRunningTest()
       {
          MethodBase callingTestMethod = _testMethodSelectorFromCallStack.GetCallingTestMethod();
          return GetMethodName(callingTestMethod);
       }

       /// <summary>
       /// Uruchamia podane delegaty w zależności to fazy uruchamiania testu.
       /// </summary>
       /// <param name="testCaseGenerator">Delegata odpowiedzialna za generowanie przypadków testowych. Przypadki są keszowane i są dostępne z parametru delegat inicializer oraz assertion</param>
       /// <param name="inicializer">Delegata uruchamiana z TestFixtureSetup. Odpowiedzialna za zebranie danych początkowych dla procesu.</param>
       /// <param name="assertion">Delegata uruchamiana po zakończeniu procesu celem weryfikacji poprawności danych wujściowych.</param>
       public void Run<T>(Func<IEnumerable<T>> testCaseGenerator, Action<T> inicializer, Action<T> assertion)
       {
          string methodName = GetNameOfRunningTest();
          if (_initialized == false)
          {
             if (testCaseGenerator == null)
             {
                ReportProblem(methodName, () => _viewTestFixture.ReportNullArgument("TestCaseGenerator nie może być nullem."));
                return;
             }

             if (inicializer == null)
             {
                ReportProblem(methodName, () => _viewTestFixture.ReportNullArgument("Inicjalizer nie może być nullem."));
                return;
             }

             RunInitializer(methodName, testCaseGenerator, inicializer);
             return;
          }

          if (assertion == null)
          {
             ReportProblem(methodName, _viewTestFixture.ReportAssertDelegateIsNull);
          }

          try
          {
             RunAssertion(methodName, assertion);
          }
          catch (Exception ex)
          {
             _exceptionFilter.FilterExceptionThrownByAssertionRunner(ex);

             throw;
          }
       }

       // todo test na wywołanie metody nie bezpośrednio z testu.

       private void RunInitializer<T>(string methodName, Func<IEnumerable<T>> testCaseGenerator, Action<T> inicializer)
       {
          TestState testState = TestRunner.RunInitializer(testCaseGenerator, inicializer, _inicjalizerView);
          _testStates.Add(methodName, testState);
       }

       private void RunAssertion<T>(string methodName, Action<T> assertion)
       {
          TestMethodInvoker testMethodInvoker = _testMethods[methodName];
          testMethodInvoker.RunDuringVerification();
          if (testMethodInvoker.WillExecute)
          {
             TestRunner.RunAssertion(_testStates[methodName], assertion, _viewTestFixture, _exceptionFilter, _assertionAction);
          }
       }

       private void ReportProblem(string methodName, Action reportProblem)
       {
          TestState testState;
          if (_testStates.TryGetValue(methodName, out testState))
          {
             testState.AddProblemReporter(reportProblem);
             return;
          }

          testState = new TestState();
          testState.AddProblemReporter(reportProblem);
          _testStates.Add(methodName, testState);
       }
    }
}
