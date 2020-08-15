using System;
using static RimTest.Assertion;

namespace RimTest.tests
{
    [TestSuite]
    public static class GreaterThan
    {
        [Test]
        public static void PassWhenGreater()
        {
            Assert(1).To.Be.GreaterThan(0);
        }
        [Test]
        public static void ThrowWhenNotGreater()
        {
            try
            {
                Assert(1).To.Be.GreaterThan(1);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
    }
}
