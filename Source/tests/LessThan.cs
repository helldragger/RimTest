using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests
{
    [TestSuite]
    public static class LessThan
    {
        [Test]
        public static void PassWhenLess()
        {
            Assert(1).To.Be.LessThan(2);
        }
        [Test]
        public static void ThrowWhenNotLess()
        {
            try
            {
                Assert(1).To.Be.LessThan(1);
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