using System;
using static RimTest.Assertion;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests
{
    public static class MockValidTestSuite
    {
    }

    public class MockNonStaticTestSuite
    {
    }

    internal static class MockNonPublicTestSuite
    {
    }

    [TestSuite]
    public static class TestSuites
    {
        [Test]
        public static void PassWhenValid()
        {
            AssertFunc( () => RimTest.IsValidTestSuite(typeof(MockValidTestSuite)) ).Not.To.Throw();
        }

        [Test]
        public static void PassWhenPublic()
        {
            Assert(RimTest.CheckTestSuiteIsPublic(typeof(MockValidTestSuite))).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonPublic()
        {
            Type type = typeof(MockNonPublicTestSuite);
            Assert(RimTest.CheckTestSuiteIsPublic(type)).To.Be.False();
            AssertFunc(() => RimTest.IsValidTestSuite(type)).To.Throw();
        }

        [Test]
        public static void IsValidTestSuiteThrowWhenNull()
        {
            AssertFunc(() => RimTest.IsValidTestSuite(null)).To.Throw();
        }

        [Test]
        public static void ChecksAreFalseWhenNull()
        {
            Assert(RimTest.CheckTestSuiteIsPublic(null)).To.Be.False();
            Assert(RimTest.CheckTestSuiteIsStatic(null)).To.Be.False();
        }



        [Test]
        public static void PassWhenStatic()
        {
            Assert(RimTest.CheckTestSuiteIsStatic(typeof(MockValidTestSuite))).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonStatic()
        {
            Type type = typeof(MockNonStaticTestSuite);
            Assert(RimTest.CheckTestSuiteIsStatic(type)).To.Be.False();
            AssertFunc(() => RimTest.IsValidTestSuite(type)).To.Throw();
        }
        [Test]
        public static void ThrowWhenInvalid()
        {
            AssertFunc(() => RimTest.IsValidTestSuite(typeof(MockNonStaticTestSuite))).To.Throw();
            AssertFunc(() => RimTest.IsValidTestSuite(typeof(MockNonPublicTestSuite))).To.Throw();
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member