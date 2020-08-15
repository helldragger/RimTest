using System;
using static RimTest.Assertion;

namespace RimTest.tests
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
