using System;
using static RimTest.Assertion;

namespace RimTest.tests
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
