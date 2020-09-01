namespace RimTest.Testing
{
    /// <summary>
    /// Registered Assembly current test execution status.
    /// </summary>
    public enum AssemblyStatus
    {
        /// <summary>
        /// Any test suite fails
        /// </summary>
        ERROR,
        /// <summary>
        /// Any test suite is skipped or have warnings
        /// </summary>
        WARNING,
        /// <summary>
        /// Any test suite is not run
        /// </summary>
        UNKNOWN,
        /// <summary>
        /// All test suites are successful
        /// </summary>
        PASS
    }
    /// <summary>
    /// Utils for AssemblyStatus
    /// </summary>
    public static class AssemblyStatusExtension
    {
        /// <summary>
        /// Value to string symbol converter
        /// </summary>
        /// <param name="status"></param>
        /// <returns>user friendly string representation</returns>
        public static string StatusSymbol(AssemblyStatus status)
        {
            switch (status)
            {
                case AssemblyStatus.UNKNOWN:
                    return "??";
                case AssemblyStatus.PASS:
                    return "✓";
                case AssemblyStatus.WARNING:
                    return "!!";
                case AssemblyStatus.ERROR:
                    return "✘";
                default:
                    return "";
            }
        }
    }
}
