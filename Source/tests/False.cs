using System;
using static RimTest.Assertion;

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
