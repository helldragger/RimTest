using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Verse;
using static RimTest.Testing.Validator;

namespace RimTest.Testing
{
    /// <summary>
    /// Stores and manages Assembly level data, aka known statuses and exceptions.
    /// </summary>
    public static class AssemblyExplorer
    {
        static readonly IDictionary<Assembly, AssemblyStatus> asm2Status = new Dictionary<Assembly, AssemblyStatus>();
        static readonly IDictionary<Assembly, Exception> asm2Error = new Dictionary<Assembly, Exception>();
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="status"></param>
        public static void SetAssemblyStatus(Assembly asm, AssemblyStatus status)
        {
            asm2Status[asm] = status;
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="error"></param>
        public static void SetAssemblyError(Assembly asm, Exception error)
        {
            asm2Error[asm] = error;
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns>The current registered AssemblyStatus else AssemblyStatus.UNKNOWN</returns>
        /// <seealso cref="AssemblyStatus"/>
        public static AssemblyStatus GetAssemblyStatus(Assembly asm)
        {
            if (asm2Status.ContainsKey(asm)) return asm2Status[asm];
            return AssemblyStatus.UNKNOWN;
        }

        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns>The current registered Exception else null if none registered</returns>
        public static Exception GetAssemblyError(Assembly asm)
        {
            if (asm2Error.ContainsKey(asm)) return asm2Error[asm];
            return null;
        }

    }
    /// <summary>
    /// Stores and manage the links between registered assemblies and their registered test suites
    /// </summary>
    public static class Assembly2TestSuiteLink
    {
        static readonly IDictionary<Assembly, ISet<Type>> asm2TestSuites = new Dictionary<Assembly, ISet<Type>>();
        /// <summary>
        /// Can register multiple unique test suites to the same assembly. Will not store duplicates.
        /// </summary>
        /// <param name="testSuite"></param>
        /// <param name="asm"></param>
        public static void RegisterTestSuite2Asm(Type testSuite, Assembly asm)
        {
            if (!asm2TestSuites.ContainsKey(asm))
            {
                asm2TestSuites[asm] = new HashSet<Type>();
            }
            asm2TestSuites[asm].Add(testSuite);
        }
        /// <summary>
        /// </summary>
        /// <returns>A list of all known assemblies</returns>
        public static List<Assembly> GetAssemblies()
        {
            return new List<Assembly>(asm2TestSuites.Keys);
        }

        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns>A set of registered tests suites for this assembly. Returns an empty set if the assembly is not registered.</returns>
        public static ISet<Type> GetTestSuites(Assembly asm)
        {
            if (asm2TestSuites.ContainsKey(asm)) return asm2TestSuites[asm];
            return new HashSet<Type>();
        }

    }

    /// <summary>
    /// Stores and manages Test Suite level data, aka known statuses and exceptions.
    /// </summary>
    public static class TestSuiteExplorer
    {
        static readonly IDictionary<Type, TestSuiteStatus> testSuite2Status = new Dictionary<Type, TestSuiteStatus>();
        static readonly IDictionary<Type, Exception> testSuite2Error = new Dictionary<Type, Exception>();
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <param name="status"></param>
        public static void SetTestSuiteStatus(Type testSuite, TestSuiteStatus status)
        {
            testSuite2Status[testSuite] = status;
        }
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <param name="error"></param>
        public static void SetTestSuiteError(Type testSuite, Exception error)
        {
            testSuite2Error[testSuite] = error;
        }
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns>current registered TestSuiteStatus else TestSuiteStatus.UNKNOWN</returns>
        public static TestSuiteStatus GetTestSuiteStatus(Type testSuite)
        {
            if (testSuite2Status.ContainsKey(testSuite)) return testSuite2Status[testSuite];
            return TestSuiteStatus.UNKNOWN;
        }
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns>current registered Exception else null if none registered</returns>
        public static Exception GetTestSuiteError(Type testSuite)
        {
            if (testSuite2Error.ContainsKey(testSuite)) return testSuite2Error[testSuite];
            return null;
        }
    }

    /// <summary>
    /// Stores and manage the links between registered test suites and their registered tests
    /// </summary>
    public static class TestSuite2TestLink
    {
        static readonly IDictionary<Type, ISet<MethodInfo>> testSuite2Tests = new Dictionary<Type, ISet<MethodInfo>>();

        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="testSuite"></param>
        public static void RegisterTest2TestSuite(MethodInfo test, Type testSuite)
        {
            if (!testSuite2Tests.ContainsKey(testSuite))
            {
                testSuite2Tests[testSuite] = new HashSet<MethodInfo>();
            }
            testSuite2Tests[testSuite].Add(test);
        }
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns>A set of registered tests for this test suite. Returns an empty set if the test suite is not registered.</returns>
        public static ISet<MethodInfo> GetTests(Type testSuite)
        {
            if (testSuite2Tests.ContainsKey(testSuite)) return testSuite2Tests[testSuite];
            return new HashSet<MethodInfo>();
        }
    }

    /// <summary>
    /// Stores and manages Test level data, aka known statuses and exceptions.
    /// </summary>
    public static class TestExplorer
    {
        static readonly IDictionary<MethodInfo, TestStatus> test2Status = new Dictionary<MethodInfo, TestStatus>();
        static readonly IDictionary<MethodInfo, Exception> test2Error = new Dictionary<MethodInfo, Exception>();
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="status"></param>
        public static void SetTestStatus(MethodInfo test, TestStatus status)
        {
            test2Status[test] = status;
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="error"></param>
        public static void SetTestError(MethodInfo test, Exception error)
        {
            test2Error[test] = error;
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <returns>current registered TestStatus else TestStatus.UNKNOWN</returns>
        public static TestStatus GetTestStatus(MethodInfo test)
        {
            if (test2Status.ContainsKey(test)) return test2Status[test];
            return TestStatus.UNKNOWN;
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <returns>current registered Exception else null if none registered</returns>
        public static Exception GetTestError(MethodInfo test)
        {
            if (test2Error.ContainsKey(test)) return test2Error[test];
            return null;
        }
    }

    /// <summary>
    /// Explores and registers every tested assembly, test suites and tests currently loaded.
    /// </summary>
    public static class Explorer
    {
        /// <summary>
        /// </summary>
        public static void ExploreAndRegisterAssemblies()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                ExploreAndRegisterTestSuites(asm);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="asm">Entry point</param>
        public static void ExploreAndRegisterTestSuites(Assembly asm)
        {
            foreach (Type testSuite in asm.GetTypes()
                    .Where((Type type) => type.TryGetAttribute<TestSuite>() != null))
            {
                Assembly2TestSuiteLink.RegisterTestSuite2Asm(testSuite, asm);
                try
                {
                    IsValidTestSuite(testSuite);
                }
                catch (Exception e)
                {
                    TestSuiteExplorer.SetTestSuiteError(testSuite, e.InnerException);
                    TestSuiteExplorer.SetTestSuiteStatus(testSuite, TestSuiteStatus.SKIP);
                    continue;
                }
                ExploreAndRegisterTests(testSuite);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="testSuite">Entry point</param>
        public static void ExploreAndRegisterTests(Type testSuite)
        {
            foreach (MethodInfo test in testSuite.GetMethods().Where((MethodInfo info) => info.TryGetAttribute<Test>() != null))
            {
                TestSuite2TestLink.RegisterTest2TestSuite(test, testSuite);
                try
                {
                    IsValidTest(test);
                }
                catch (Exception e)
                {
                    TestExplorer.SetTestError(test, e.InnerException);
                    TestExplorer.SetTestStatus(test, TestStatus.SKIP);
                    continue;
                }
            }
        }

    }
}
