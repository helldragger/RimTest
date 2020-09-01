using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests.assertion
{
    [TestSuite]
    public static class Throw
    {
        [Test]
        public static void PassWhenThrow()
        {
            AssertFunc(() => throw new Exception()).To.Throw();
        }
        [Test]
        public static void ThrowWhenNotThrow()
        {
            try
            {
                AssertFunc(() => 1).To.Throw();
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