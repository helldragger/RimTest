using System;
using static RimTest.Assertion;

namespace RimTest.tests
{
    [TestSuite]
    public static class BetweenInclusive
    {
        [Test]
        public static void PassWhenBetweenInclusive()
        {
            Assert(1).To.Be.BetweenInclusive(0, 2);
        }
        [Test]
        public static void PassWhenEqualsMin()
        {
            Assert(0).To.Be.BetweenInclusive(0, 2);
        }
        [Test]
        public static void PassWhenEqualsMax()
        {
            Assert(2).To.Be.BetweenInclusive(0, 2);
        }
        [Test]
        public static void PassWhenEqualsBoth()
        {
            Assert(1).To.Be.BetweenInclusive(1, 1);
        }
        [Test]
        public static void ThrowWhenUnder()
        {
            try
            {
                Assert(-1).To.Be.BetweenInclusive(0, 2);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
        [Test]
        public static void ThrowWhenOver()
        {
            try
            {
                Assert(3).To.Be.BetweenInclusive(0, 2);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
        [Test]
        public static void ThrowWhenInvalidLimits()
        {
            try
            {
                Assert(1).To.Be.BetweenInclusive(2, 0);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
    }
}
