using System;
using static RimTest.Assertion;

namespace RimTest.tests
{
    [TestSuite]
    public static class Null
    {
        [Test]
        public static void PassWhenNull()
        {
            Assert(null).To.Be.Null();
        }
        [Test]
        public static void ThrowWhenNotNull()
        {
            try
            {
                Assert(1).To.Be.Null();
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
    }
}
