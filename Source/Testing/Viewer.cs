using RimTest.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Verse;
using static RimTest.Testing.Assembly2TestSuiteLink;
using static RimTest.Testing.AssemblyExplorer;
using static RimTest.Testing.AssemblyStatusExtension;
using static RimTest.Testing.TestExplorer;
using static RimTest.Testing.TestStatusExtension;
using static RimTest.Testing.TestSuite2TestLink;
using static RimTest.Testing.TestSuiteExplorer;
using static RimTest.Testing.TestSuiteStatusExtension;
namespace RimTest.Testing
{
    static class Viewer
    {
        static Action<string> Info = (string msg) => Log.Message(msg);
        static Action<string> Warn = (string msg) => Log.Warning(msg);
        static Action<string> Err = (string msg) => Log.Error(msg);
        static void LogDetailledErrors(ICollection<Assembly> asms)
        {
            foreach (Assembly asm in asms)
            {
                AssemblyStatus asmStatus = GetAssemblyStatus(asm);
                string asmName = asm.GetName().Name;
                switch (asmStatus)
                {
                    case AssemblyStatus.WARNING:
                        Warn($"[{StatusSymbol(asmStatus)}] {asmName} > {GetAssemblyError(asm).Message}");
                        break;
                    case AssemblyStatus.ERROR:
                        Err($"[{StatusSymbol(asmStatus)}] {asmName} > {GetAssemblyError(asm)}");
                        break;
                    case AssemblyStatus.UNKNOWN:
                        Info($"[{StatusSymbol(asmStatus)}] {asmName} > Not Run Yet");
                        break;
                    default:
                        break;
                }
                //Errored tests display
                foreach (Type testSuite in GetTestSuites(asm))
                {
                    TestSuiteStatus tsStatus = GetTestSuiteStatus(testSuite);
                    if (tsStatus is TestSuiteStatus.PASS) continue;
                    switch (tsStatus)
                    {
                        case TestSuiteStatus.WARNING:
                        case TestSuiteStatus.SKIP:
                            Warn($"    [{StatusSymbol(tsStatus)}] {testSuite.Name} > {GetTestSuiteError(testSuite)}");
                            break;
                        case TestSuiteStatus.ERROR:
                            Err($"    [{StatusSymbol(tsStatus)}] {testSuite.Name} > {GetTestSuiteError(testSuite)}");
                            break;
                        case TestSuiteStatus.UNKNOWN:
                            Info($"    [{StatusSymbol(tsStatus)}] {testSuite.Name} > Not Run Yet");
                            break;
                        default:
                            break;
                    }

                    foreach (MethodInfo test in GetTests(testSuite))
                    {
                        TestStatus tStatus = GetTestStatus(test);
                        switch (tStatus)
                        {
                            case TestStatus.SKIP:
                                Warn($"        [{StatusSymbol(tStatus)}] {testSuite.Name}.{test.Name} > {GetTestError(test)}");
                                continue;
                            case TestStatus.ERROR:
                                Err($"        [{StatusSymbol(tStatus)}] {testSuite.Name}.{test.Name} > {GetTestError(test)}");
                                continue;
                            case TestStatus.UNKNOWN:
                                Info($"        [{StatusSymbol(tStatus)}] {testSuite.Name}.{test.Name} > Not Run Yet");
                                continue;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        
        static string BuildAsmSummary(Assembly asm, Tally<TestSuiteStatus> tsTally, Tally<TestStatus> tTally)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"[{StatusSymbol(GetAssemblyStatus(asm))}] {asm.GetName().Name} ");
            builder.Append($"|| Test Suites :");
            foreach (TestSuiteStatus status in Enum.GetValues(typeof(TestSuiteStatus)))
            {
                int tally = tsTally[status];
                if (tally != 0)
                {
                    builder.Append($" {tally} {StatusSymbol(status)} ");
                }
            }

            builder.Append($"|| Tests :");
            foreach (TestStatus status in Enum.GetValues(typeof(TestStatus)))
            {
                int tally = tTally[status];
                if (tally != 0)
                {
                    builder.Append($" {tally} {StatusSymbol(status)} ");
                }
            }

            return builder.ToString();
        }

        static void LogSummary(ICollection<Assembly> asms)
        {
            foreach (Assembly asm in asms)
            {
                Tally<TestSuiteStatus> tsTally = new Tally<TestSuiteStatus>();
                Tally<TestStatus> tTally = new Tally<TestStatus>();

                // test results tallying
                foreach (Type testSuite in GetTestSuites(asm))
                {
                    tsTally[GetTestSuiteStatus(testSuite)]++;

                    foreach (MethodInfo test in GetTests(testSuite))
                    {
                        tTally[GetTestStatus(test)]++;
                    }
                }

                string asmResult = BuildAsmSummary(asm, tsTally, tTally);

                switch (GetAssemblyStatus(asm))
                {
                    case AssemblyStatus.UNKNOWN:
                    case AssemblyStatus.PASS:
                        Info(asmResult);
                        continue;
                    case AssemblyStatus.WARNING:
                        Warn(asmResult);
                        break;
                    case AssemblyStatus.ERROR:
                        Err(asmResult);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void LogTestsResults()
        {
            List<Assembly> asms = GetAssemblies();
            asms.Sort();
            Info("==TESTING START");
            Info("__SUMMARY");
            LogSummary(asms);
            Info("__ERRORS");
            LogDetailledErrors(asms);
            Info("==TESTING END");

        }
    }
}
