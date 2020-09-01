using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests.assertion
{
    [TestSuite]
    public static class True
    {
        [Test]
        public static void PassWhenTrue()
        {
            Assert(true).To.Be.True();
        }
        [Test]
        public static void ThrowWhenNotTrue()
        {
            try
            {
                Assert(false).To.Be.True();
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