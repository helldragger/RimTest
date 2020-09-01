/*
using static RimTest.Assertion;
#pragma warning disable CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement
namespace RimTest.tests
{
    [TestSuite]
    public static class TestSuiteExplorer
    {
        private static class MockTestSuite
        {
            [Test]
            public static void correctTest1() { return; }
            [Test]
            public static void correctTest2() { return; }
            [Test]
            public static bool incorrectTest1() { return true; }
            [Test]
            internal static void incorrectTest2() { return; }
        }
    }

    [TestSuite]
    static class ExampleSkippedTestSuite
    {
        [Test]
        public static void UnranTest() { }
    }

    [TestSuite]
    public static class ExampleErroringTestSuite
    {
        [Test]
        public static void ErroringTest() { Assert(true).To.Be.False(); }
        [Test]
        public static void ValidTest() { Assert(false).To.Be.False(); }
    }

    [TestSuite]
    public static class ExampleWarningTestSuite
    {
        [Test]
        public static bool SkippedTest() { return true; }
        [Test]
        public static void ValidTest() { Assert(false).To.Be.False(); }
    }

}
#pragma warning restore CS1591 // Commentaire XML manquant pour le type ou le membre visible publiquement

*/