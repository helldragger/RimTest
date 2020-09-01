using System;

namespace RimTest
{
    /// <summary>
    /// TestSuite meta-attribute, used to register a class as a test suite to be discovered by RimTest.
    /// </summary>
    /// <remarks>
    /// Only used when the class contains valid tests functions decorated with the meta-attribute Test. 
    /// Valid Test Suites are static and public, see code example
    /// </remarks>
    /// <code>
    /// [TestSuite]
    /// public static class testSuiteA{
    ///     //tests
    /// }
    /// </code>
    public class TestSuite : Attribute
    {
    }
}
