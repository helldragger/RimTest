using RimTest.tests;
using System;
using System.Linq;
using System.Reflection;
using Verse;

namespace RimTest
{
    public class RimTest
    {
        public static string SKIP = "➥";
        public static string FAIL = "✘";
        public static string PASS = "✓";


        public static void IsValidTestSuite(Type testSuite)
        {
            if(testSuite == null)
            {
                throw new ArgumentNullException();
            }
            if (!CheckTestSuiteIsStatic(testSuite))
            {
                throw new InvalidTestSuiteException($"{testSuite.Name}: test suite must be static.");
            }
            if (!CheckTestSuiteIsPublic(testSuite))
            {
                throw new InvalidTestSuiteException($"{testSuite.Name}: test suite must be public.");
            }
        }

        public static bool CheckTestSuiteIsPublic(Type testSuite)
        {
            if (testSuite == null) return false;
            return testSuite.IsPublic;
        }
        public static bool CheckTestSuiteIsStatic(Type testSuite)
        {
            if (testSuite == null) return false;
            return testSuite.IsClass && testSuite.IsSealed && testSuite.IsAbstract;
        }

        public static void IsValidTest(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException();
            }

            if (!CheckTestReturnsVoid(method))
            {
                throw new InvalidTestException($"{method.Name}: tests must have a void return type.");
            }
            if (!CheckTestIsParameterFree(method))
            {
                throw new InvalidTestException($"{method.Name}: tests must need no parameters.");
            }
            if (!CheckTestIsStatic(method))
            {
                throw new InvalidTestException($"{method.Name}: tests must be static.");
            }
            if (!CheckTestIsPublic(method))
            {
                throw new InvalidTestException($"{method.Name}: tests must be public.");
            }
        }

        public static bool CheckTestIsPublic(MethodInfo method)
        {
            if (method == null) return false;
            return method.IsPublic;
        }
        public static bool CheckTestIsStatic(MethodInfo method)
        {
            if (method == null) return false;
            return method.IsStatic;
        }
        public static bool CheckTestReturnsVoid(MethodInfo method)
        {
            if (method == null) return false;
            return method.ReturnType == typeof(void);
        }
        public static bool CheckTestIsParameterFree(MethodInfo method)
        {
            if (method == null) return false;
            return method.GetParameters().Length == 0;
        }

        public static void RunTestFunc(MethodInfo method)
        {
            try
            {
                IsValidTest(method);
            }
            catch (Exception e)
            {
                Log.Message($"    [{SKIP}] {e.InnerException}");
                return;
            }
            try
            {
                // tests are static (null reference object) and do NOT accept arguments (null parameters array)
                method.Invoke(null, null);
                Log.Message($"    [{PASS}] {method.Name}");
            }
            catch (Exception e)
            {
                Log.Error($"    [{FAIL}] {method.Name}: {e.InnerException} \n___________MESSAGE\n{e.Message}\n__________STACKTRACE:\n{e.StackTrace}");
            }
        }

        public static void RunTestSuite(Type testSuite)
        {
            try
            {
                IsValidTestSuite(testSuite);
            }
            catch (Exception e)
            {
                Log.Warning($"  [{SKIP}] {e.InnerException}");
                return;
            }
            Log.Message($"  TEST SUITE: {testSuite.FullName}");
            foreach (MethodInfo method in testSuite.GetMethods().Where((MethodInfo info) => info.TryGetAttribute<Test>() != null))
            {
                RunTestFunc(method);
            }
        }

        public static void RunTests()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                // DO NOT 
                if ( !RimTestMod.Settings.RunOwnTests && asm == Assembly.GetExecutingAssembly() )
                    continue;
                bool testFound = false;
                foreach (Type testSuite in asm.GetTypes().Where((Type type) => type.TryGetAttribute<TestSuite>() != null))
                {

                    if (!(testSuite.IsClass && testSuite.IsSealed && testSuite.IsAbstract))
                    {
                        Log.Error($"  [{SKIP}] {testSuite.Name} INVALID: test suite must be a static class.");
                        continue;
                    }
                    if (!testFound)
                    {
                        testFound = true;
                        Log.Message($"TESTING ASSEMBLY: {asm.FullName}");
                    }
                    RunTestSuite(testSuite);
                }
            }
        }
    }
}
