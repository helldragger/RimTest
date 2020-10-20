using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Verse;
using static RimTest.Testing.Validator;

namespace RimTest.Testing
{
    /// <summary>
    /// Time profiling system
    /// </summary>
    public static class TimeElapsedExplorer
    {
        static readonly IDictionary<Assembly, double> asm2TimeElapsed = new Dictionary<Assembly, double>();
        static readonly IDictionary<Type, double> testSuite2TimeElapsed = new Dictionary<Type, double>();
        static readonly IDictionary<MethodInfo, double> test2TimeElapsed = new Dictionary<MethodInfo, double>(); 
        /// <summary>
        /// </summary>
        public static void UpdateAllAssembliesTimeElapsed()
        {
            foreach (Assembly asm in AssemblyExplorer.GetAllKnownAssemblies())
            {
                UpdateAssemblyTimeElapsed(asm);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="time"></param>
        public static void SetAssemblyTimeElapsed(Assembly asm, double time)
        {
            asm2TimeElapsed[asm] = time;
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        public static void UpdateAssemblyTimeElapsed(Assembly asm)
        {
            double totaltime = 0;
            foreach (var ts in Assembly2TestSuiteLink.GetTestSuites(asm))
            {
                UpdateTestSuiteTimeElapsed(ts);
                double testtime = GetTestSuiteTimeElapsed(ts);
                if (testtime != -1) totaltime += testtime;
            }
            SetAssemblyTimeElapsed(asm, totaltime);
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static double GetAssemblyTimeElapsed(Assembly asm)
        {
            if (asm2TimeElapsed.ContainsKey(asm)) return asm2TimeElapsed[asm];
            else UpdateAssemblyTimeElapsed(asm);
            return asm2TimeElapsed[asm];
        }
        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="time"></param>
        public static void SetTestSuiteTimeElapsed(Type ts, double time)
        {
            testSuite2TimeElapsed[ts] = time;
        }
        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        public static void UpdateTestSuiteTimeElapsed(Type ts)
        {
            double totaltime = 0;
            foreach (var test in TestSuite2TestLink.GetTests(ts))
            {
                double testtime = GetTestTimeElapsed(test);
                if (testtime != -1) totaltime += testtime;
            }
            SetTestSuiteTimeElapsed(ts, totaltime);
        }
        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static double GetTestSuiteTimeElapsed(Type ts)
        {

            if (testSuite2TimeElapsed.ContainsKey(ts)) return testSuite2TimeElapsed[ts];
            else UpdateTestSuiteTimeElapsed(ts);
            return testSuite2TimeElapsed[ts];
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <param name="time"></param>
        public static void SetTestTimeElapsed(MethodInfo test, double time)
        {
            test2TimeElapsed[test] = time;
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static double GetTestTimeElapsed(MethodInfo test)
        {
            if (test2TimeElapsed.ContainsKey(test)) return test2TimeElapsed[test];
            return -1;
        }
    }

    /// <summary>
    /// Keeps track of filtered tests for easy test finding
    /// </summary>
    public static class FilteredExplorer
    {
        static Regex filter = new Regex(@"");
        /// <summary>
        /// Are failed state asm shown?
        /// </summary>
        static public bool failEnabledAsm = true;
        /// <summary>
        /// Are warning state asm shown?
        /// </summary>
        static public bool warningEnabledAsm = true;
        /// <summary>
        /// Are unknown state asm shown?
        /// </summary>
        static public bool unknownEnabledAsm = true;
        /// <summary>
        /// Are passed state asm shown?
        /// </summary>
        static public bool passEnabledAsm = true;

        /// <summary>
        /// Are failed state TS shown?
        /// </summary>
        static public bool failEnabledTS = true;
        /// <summary>
        /// Are warning state TS shown?
        /// </summary>
        static public bool warningEnabledTS = true;
        /// <summary>
        /// Are unknown state TS shown?
        /// </summary>
        static public bool unknownEnabledTS = true;
        /// <summary>
        /// Are skipped state TS shown?
        /// </summary>
        static public bool skipEnabledTS = true;
        /// <summary>
        /// Are passed state TS shown?
        /// </summary>
        static public bool passEnabledTS = true;

        /// <summary>
        /// Are failed state T shown?
        /// </summary>
        static public bool failEnabledT = true;
        /// <summary>
        /// Are unknown state T shown?
        /// </summary>
        static public bool unknownEnabledT = true;
        /// <summary>
        /// Are skipped state T shown?
        /// </summary>
        static public bool skipEnabledT = true;
        /// <summary>
        /// Are passed state T shown?
        /// </summary>
        static public bool passEnabledT = true;

        /// <summary>
        /// </summary>
        /// <param name="filter"></param>
        public static void UpdateFilter(Regex filter)
        {
            FilteredExplorer.filter = filter;
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static bool DoesAssemblyStatusMatchesFilter(Assembly asm)
        {
            return (AssemblyExplorer.GetAssemblyStatus(asm)) switch
            {
                AssemblyStatus.ERROR => failEnabledAsm,
                AssemblyStatus.WARNING => warningEnabledAsm,
                AssemblyStatus.UNKNOWN => unknownEnabledAsm,
                AssemblyStatus.PASS => passEnabledAsm,
                _ => false,
            };
        }
        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static bool DoesTestSuiteStatusMatchesFilter(Type ts)
        {
            return (TestSuiteExplorer.GetTestSuiteStatus(ts)) switch
            {
                TestSuiteStatus.SKIP => skipEnabledTS,
                TestSuiteStatus.ERROR => failEnabledTS,
                TestSuiteStatus.WARNING => warningEnabledTS,
                TestSuiteStatus.UNKNOWN => unknownEnabledTS,
                TestSuiteStatus.PASS => passEnabledTS,
                _ => false,
            };
        }
        /// <summary>
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool DoesTestStatusMatchesFilter(MethodInfo t)
        {
            return (TestExplorer.GetTestStatus(t)) switch
            {
                TestStatus.SKIP => skipEnabledT,
                TestStatus.ERROR => failEnabledT,
                TestStatus.UNKNOWN => unknownEnabledT,
                TestStatus.PASS => passEnabledT,
                _ => false,
            };
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static bool DoesAssemblyMatchesFilter(Assembly asm)
        {
            return (filter.IsMatch(asm.GetName().Name) || Assembly2TestSuiteLink.GetTestSuites(asm).Any((Type ts) => DoesTestSuiteMatchesFilter(ts))) && DoesAssemblyStatusMatchesFilter(asm);
        }
        /// <summary>
        /// </summary>
        /// <param name="testSuite"></param>
        /// <returns></returns>
        public static bool DoesTestSuiteMatchesFilter(Type testSuite)
        {
            return (filter.IsMatch(testSuite.Name) || TestSuite2TestLink.GetTests(testSuite).Any((MethodInfo t) => DoesTestMatchesFilter(t))) && DoesTestSuiteStatusMatchesFilter(testSuite);
        }
        /// <summary>
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static bool DoesTestMatchesFilter(MethodInfo test)
        {
            return filter.IsMatch(test.Name) && DoesTestStatusMatchesFilter(test);
        }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Assembly> GetFilteredAssemblies()
        {
            return AssemblyExplorer
                .GetAllKnownAssemblies()
                .Where((Assembly asm) => DoesAssemblyMatchesFilter(asm))
                .ToList();
        }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetFilteredTestSuites()
        {
            return TestSuiteExplorer
                .GetAllKnownTestSuites()
                .Where((Type ts) => DoesTestSuiteMatchesFilter(ts))
                .ToList();
        }
        /// <summary>
        /// </summary>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetFilteredTestSuites(Assembly asm)
        {
            return Assembly2TestSuiteLink
                .GetTestSuites(asm)
                .Where((Type ts) => DoesTestSuiteMatchesFilter(ts))
                .ToList();
        }
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetFilteredTests()
        {
            return TestExplorer
                .GetAllKnownTests()
                .Where((MethodInfo t) => DoesTestMatchesFilter(t))
                .ToList();
        }
        /// <summary>
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetFilteredTests(Type ts)
        {
            return TestSuite2TestLink
                .GetTests(ts)
                .Where((MethodInfo t) => DoesTestMatchesFilter(t))
                .ToList();
        }
    }
    /// <summary>
    /// Keeps track of test statuses
    /// </summary>
    public static class StatusExplorer
    {

        static readonly IDictionary<AssemblyStatus, int> asmStatus2count = new Dictionary<AssemblyStatus, int>();
        static readonly IDictionary<TestSuiteStatus, int> tsStatus2count = new Dictionary<TestSuiteStatus, int>();
        static readonly IDictionary<TestStatus, int> tStatus2count = new Dictionary<TestStatus, int>();
        /// <summary>
        /// </summary>
        public static void UpdateAllStatusCounts()
        {
            foreach (AssemblyStatus status in Enum.GetValues(typeof(AssemblyStatus)))
            {
                UpdateAssemblyStatusCount(status);
            }
            foreach (TestSuiteStatus status in Enum.GetValues(typeof(TestSuiteStatus)))
            {
                UpdateTestSuiteStatusCount(status);
            }
            foreach (TestStatus status in Enum.GetValues(typeof(TestStatus)))
            {
                UpdateTestStatusCount(status);
            }
        }
        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateAssemblyStatusCount(AssemblyStatus status)
        {
            int value = FilteredExplorer
                    .GetFilteredAssemblies()
                    .Where((Assembly asm) => AssemblyExplorer.GetAssemblyStatus(asm) == status)
                    .Count();
            if (asmStatus2count.ContainsKey(status)) asmStatus2count[status] = value;
            else asmStatus2count.Add(status, value);
        }
        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public static int GetAssemblyStatusCount(AssemblyStatus status)
        {
            if (!asmStatus2count.ContainsKey(status)) UpdateAssemblyStatusCount(status);
            return asmStatus2count[status];
        }
        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateTestSuiteStatusCount(TestSuiteStatus status)
        {
            int value = FilteredExplorer
                    .GetFilteredTestSuites()
                    .Where((Type ts) => TestSuiteExplorer.GetTestSuiteStatus(ts) == status)
                    .Count();
            if (tsStatus2count.ContainsKey(status)) tsStatus2count[status] = value;
            else tsStatus2count.Add(status, value);
        }

        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        public static int GetTestSuiteStatusCount(TestSuiteStatus status)
        {
            if (!tsStatus2count.ContainsKey(status)) UpdateTestSuiteStatusCount(status);
            return tsStatus2count[status];
        }
        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        public static void UpdateTestStatusCount(TestStatus status)
        {
            int value = FilteredExplorer
                    .GetFilteredTests()
                    .Where((MethodInfo t) => TestExplorer.GetTestStatus(t) == status)
                    .Count();
            if (tStatus2count.ContainsKey(status)) tStatus2count[status] = value;
            else tStatus2count.Add(status, value);
        }

        /// <summary>
        /// </summary>
        /// <param name="status"></param>
        public static int GetTestStatusCount(TestStatus status)
        {
            if (!tStatus2count.ContainsKey(status)) UpdateTestStatusCount(status);
            return tStatus2count[status];
        }

    }

    /// <summary>
    /// Stores and manages Assembly level data, aka known statuses and exceptions.
    /// </summary>
    public static class AssemblyExplorer
    {
        static readonly IDictionary<Assembly, AssemblyStatus> asm2Status = new Dictionary<Assembly, AssemblyStatus>();
        static readonly IDictionary<Assembly, Exception> asm2Error = new Dictionary<Assembly, Exception>();
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ICollection<Assembly> GetAllKnownAssemblies()
        {
            return asm2Status.Keys;
        }

        

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
            if (!asm2Status.ContainsKey(asm)) asm2Status.Add(asm, AssemblyStatus.UNKNOWN);
            return asm2Status[asm];
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
        /// <returns></returns>
        public static ICollection<Type> GetAllKnownTestSuites()
        {
            return testSuite2Status.Keys;
        }

        

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
            if (!testSuite2Status.ContainsKey(testSuite)) testSuite2Status.Add(testSuite, TestSuiteStatus.UNKNOWN);
            
            return testSuite2Status[testSuite];
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
            if (!test2Status.ContainsKey(test)) test2Status.Add(test, TestStatus.UNKNOWN);
            return test2Status[test];
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

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static ICollection<MethodInfo> GetAllKnownTests()
        {
            return test2Status.Keys;
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
                if(Assembly2TestSuiteLink.GetTestSuites(asm).Count != 0)
                {
                    AssemblyExplorer.SetAssemblyStatus(asm, AssemblyStatus.UNKNOWN);
                }
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
                try
                {
                    Assembly2TestSuiteLink.RegisterTestSuite2Asm(testSuite, asm);
                    IsValidTestSuite(testSuite);
                    TestSuiteExplorer.SetTestSuiteStatus(testSuite, TestSuiteStatus.UNKNOWN);
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
                try
                {
                    TestSuite2TestLink.RegisterTest2TestSuite(test, testSuite);
                    IsValidTest(test);
                    TestExplorer.SetTestStatus(test, TestStatus.UNKNOWN);
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
