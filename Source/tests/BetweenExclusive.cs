using System;
using static RimTest.Assertion;

namespace RimTest.tests
{
    [TestSuite]
    public static class BetweenExclusive
    {
        [Test]
        public static void PassWhenBetweenExclusive()
        {
            Assert(1).To.Be.BetweenExclusive(0, 2);
        }

        [Test]
        public static void ThrowWhenEqualToMin()
        {
            try
            {
                Assert(0).To.Be.BetweenExclusive(0, 2);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
        [Test]
        public static void ThrowWhenEqualToMax()
        {
            try
            {
                Assert(2).To.Be.BetweenExclusive(0, 2);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
        [Test]
        public static void ThrowWhenEqualToBoth()
        {
            try
            {
                Assert(1).To.Be.BetweenExclusive(1, 1);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
        [Test]
        public static void ThrowWhenUnder()
        {
            try
            {
                Assert(-1).To.Be.BetweenExclusive(0, 2);
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
                Assert(3).To.Be.BetweenExclusive(0, 2);
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
                Assert(1).To.Be.BetweenExclusive(2, 0);
            }
            catch (Exception)
            {
                return;
            }
            throw new Exception("Should have thrown an exception.");
        }
    }
}
