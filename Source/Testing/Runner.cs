using RimTest.Util;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using static RimTest.Testing.Assembly2TestSuiteLink;
using static RimTest.Testing.AssemblyExplorer;
using static RimTest.Testing.TestExplorer;
using static RimTest.Testing.TestSuite2TestLink;
using static RimTest.Testing.TestSuiteExplorer;
using static RimTest.Testing.Validator;
namespace RimTest.Testing
{
    /// <summary>
    /// Execute registered tests and update the status of registered tests, test suites and tested assemblies
    /// </summary>
    public static class Runner
    {
        /// <summary>
        /// Check a test validity and run it if possible. 
        /// Update the status to TestStatus.SKIP if invalid, TestStatus.PASS if succesfull, TestStatus.ERROR if failed. 
        /// Registers any detected error to the test.
        /// </summary>
        /// <param name="test"></param>
        /// <seealso cref="TestStatus"/>
        public static void RunTest(MethodInfo test)
        {
            try
            {
                IsValidTest(test);
            }
            catch (Exception e)
            {
                SetTestStatus(test, TestStatus.SKIP);
                SetTestError(test, e);
                return;
            }
            try
            {
                // tests are static (null reference object) and do NOT accept arguments (null parameters array)
                test.Invoke(null, null);
                SetTestStatus(test, TestStatus.PASS);
                SetTestError(test, null);

            }
            catch (Exception e)
            {
                SetTestStatus(test, TestStatus.ERROR);
                SetTestError(test, e);
            }
        }
        /// <summary>
        /// Check a test suite validity and run its tests if possible. 
        /// Update the status to TestSuiteStatus.SKIP if invalid, then TestSuiteStatus.ERROR if any test fails, then TestSuiteStatus.WARNING if any test is skipped, then TestSuiteStatus.UNKNOWN if any test is not run, then TestSuiteStatus.PASS if succesfull. 
        /// Registers any detected error to the test suite.
        /// </summary>
        /// <param name="testSuite"></param>
        /// <seealso cref="TestSuiteStatus"/>
        public static void RunTestSuite(Type testSuite)
        {
            try
            {
                IsValidTestSuite(testSuite);
            }
            catch (Exception e)
            {
                SetTestSuiteStatus(testSuite, TestSuiteStatus.SKIP);
                SetTestSuiteError(testSuite, e);
                return;
            }
            Tally<TestStatus> tally = new Tally<TestStatus>();
            foreach (MethodInfo test in GetTests(testSuite))
            {
                RunTest(test);
                tally[GetTestStatus(test)]++;
            }


            if (tally[TestStatus.ERROR] != 0)
            {
                SetTestSuiteStatus(testSuite, TestSuiteStatus.ERROR);
                SetTestSuiteError(testSuite, new Exception($"{tally[TestStatus.ERROR]} test failed"));
            }
            else if (tally[TestStatus.SKIP] != 0)
            {
                SetTestSuiteStatus(testSuite, TestSuiteStatus.WARNING);
                SetTestSuiteError(testSuite, new Exception($"{tally[TestStatus.SKIP]} test skipped"));
            }
            else if (tally[TestStatus.UNKNOWN] != 0)
            {
                SetTestSuiteStatus(testSuite, TestSuiteStatus.UNKNOWN);
                SetTestSuiteError(testSuite, new Exception($"{tally[TestStatus.UNKNOWN]} test not run yet"));
            }
            else
            {
                SetTestSuiteStatus(testSuite, TestSuiteStatus.PASS);
                SetTestSuiteError(testSuite, null);
            }
        }

        /// <summary>
        /// Run a assembly test suites. 
        /// Update the status to AssemblyStatus.ERROR if any test suite fails, then AssemblyStatus.WARNING if any test suite is skipped or have warnings, then AssemblyStatus.UNKNOWN if any test suite is not run, then AssemblyStatus.PASS if succesfull. 
        /// Registers any detected error to the test suite.
        /// </summary>
        /// <param name="asm"></param>
        /// <seealso cref="AssemblyStatus"/>
        public static void RunAssembly(Assembly asm)
        {
            Tally<TestSuiteStatus> tally = new Tally<TestSuiteStatus>();
            foreach (Type testSuite in GetTestSuites(asm))
            {
                RunTestSuite(testSuite);
                tally[GetTestSuiteStatus(testSuite)]++;
            }
            
            if (tally[TestSuiteStatus.ERROR] != 0)
            {
                SetAssemblyStatus(asm, AssemblyStatus.ERROR);
                SetAssemblyError(asm, new Exception($"{tally[TestSuiteStatus.ERROR]} test suite failed"));
            }
            else if (tally[TestSuiteStatus.SKIP] != 0)
            {
                SetAssemblyStatus(asm, AssemblyStatus.WARNING);
                SetAssemblyError(asm, new Exception($"{tally[TestSuiteStatus.SKIP]} test suite skipped"));
            }
            else if (tally[TestSuiteStatus.WARNING] != 0)
            {
                SetAssemblyStatus(asm, AssemblyStatus.WARNING);
                SetAssemblyError(asm, new Exception($"{tally[TestSuiteStatus.WARNING]} test suite have warnings"));
            }
            else if (tally[TestSuiteStatus.UNKNOWN] != 0)
            {
                SetAssemblyStatus(asm, AssemblyStatus.UNKNOWN);
                SetAssemblyError(asm, new Exception($"{tally[TestSuiteStatus.UNKNOWN]} test suite not run yet"));
            }
            else
            {
                SetAssemblyStatus(asm, AssemblyStatus.PASS);
                SetAssemblyError(asm, null);
            }
            
        }
        /// <summary>
        /// </summary>
        public static void RunAllRegisteredTests()
        {
            foreach (Assembly asm in GetAssemblies())
            {
                RunAssembly(asm);
            }
        }
    }
}
