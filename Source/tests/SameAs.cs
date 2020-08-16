using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests
{
    [TestSuite]
    public static class SameAs
    {
        [Test]
        public static void PassWhenSame()
        {
            IComparable mock = "a";
            Assert(mock).To.Be.TheSame(mock);
        }
        [Test]
        public static void ThrowWhenNotSame()
        {
            IComparable mock = "a";
            IComparable mock2 = "b";
            try
            {
                Assert(mock).To.Be.TheSame(mock2);
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