namespace RimTest.Testing
{
    /// <summary>
    /// Registered Test Suite current test execution status.
    /// </summary>
    public enum TestSuiteStatus
    {
        /// <summary>
        /// Invalid Test Suite
        /// </summary>
        SKIP,
        /// <summary>
        /// Any test fails
        /// </summary>
        ERROR, 
        /// <summary>
        /// Any test skipped
        /// </summary>
        WARNING,
        /// <summary>
        /// Any test is not run
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// All tests are successful
        /// </summary>
        PASS
    }
    /// <summary>
    /// Utils for TestStatus
    /// </summary>
    public static class TestSuiteStatusExtension
    {
        /// <summary>
        /// Value to string symbol converter
        /// </summary>
        /// <param name="status"></param>
        /// <returns>user friendly string representation</returns>
        public static string StatusSymbol(TestSuiteStatus status)
        {
            switch (status)
            {
                case TestSuiteStatus.UNKNOWN:
                    return "??";
                case TestSuiteStatus.SKIP:
                    return "➥";
                case TestSuiteStatus.PASS:
                    return "✓";
                case TestSuiteStatus.WARNING:
                    return "!!";
                case TestSuiteStatus.ERROR:
                    return "✘";
                default:
                    return "";
            }
        }
    }
}
