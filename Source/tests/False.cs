using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests
{
    [TestSuite]
    public static class False
    {
        [Test]
        public static void PassWhenFalse()
        {
            Assert(false).To.Be.False();
        }

        [Test]
        public static void ThrowWhenNotFalse()
        {
            try
            {
                Assert(true).To.Be.False();
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