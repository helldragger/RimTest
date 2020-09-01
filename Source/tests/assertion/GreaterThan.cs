using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests.assertion
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

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member