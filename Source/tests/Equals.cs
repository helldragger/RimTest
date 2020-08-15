using System;
using static RimTest.Assertion;

namespace RimTest.tests
{
    [TestSuite]
    public static class Equals
    {
        [Test]
        public static void PassWhenEqual()
        {
            Assert(1).To.Be.EqualTo(1);
        }
        [Test]
        public static void ThrowWhenNotEqual()
        {
            try
            {
                Assert(1).To.Be.EqualTo(2);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
    }
}
