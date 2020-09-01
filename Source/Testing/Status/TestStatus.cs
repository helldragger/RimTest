namespace RimTest.Testing
{
    /// <summary>
    /// Registered Test current execution status.
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// invalid test
        /// </summary>
        SKIP, 
        /// <summary>
        /// not run yet
        /// </summary>
        UNKNOWN, 
        /// <summary>
        /// failed
        /// </summary>
        ERROR, 
        /// <summary>
        /// passed
        /// </summary>
        PASS
    }
    /// <summary>
    /// Utils for TestStatus
    /// </summary>
    public static class TestStatusExtension
    {
        /// <summary>
        /// Value to string symbol converter
        /// </summary>
        /// <param name="status"></param>
        /// <returns>user friendly string representation</returns>
        public static string StatusSymbol(TestStatus status)
        {
            switch (status)
            {
                case TestStatus.UNKNOWN:
                    return "??";
                case TestStatus.SKIP:
                    return "➥";
                case TestStatus.PASS:
                    return "✓";
                case TestStatus.ERROR:
                    return "✘";
                default:
                    return "";
            }
        }
    }
}
