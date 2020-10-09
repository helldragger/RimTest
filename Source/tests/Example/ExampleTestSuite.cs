using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RimTest.Assertion;
namespace RimTest.tests.Example
{
    [TestSuite]
    public static class ExampleErroringTestSuite
    {
        [Test]
        public static void invalidTest() { Assert(true).To.Be.False(); }
        [Test]
        public static void validTest() { Assert(true).To.Be.True(); }
        [Test]
        public static bool skippedTest() { return true; }
    }
}
