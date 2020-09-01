using System;
using static RimTest.Assertion;
using static RimTest.Testing.Validator;
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
            AssertFunc(() => IsValidTestSuite(typeof(MockValidTestSuite))).Not.To.Throw();
        }

        [Test]
        public static void PassWhenPublic()
        {
            Assert(CheckTestSuiteIsPublic(typeof(MockValidTestSuite))).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonPublic()
        {
            Type type = typeof(MockNonPublicTestSuite);
            Assert(CheckTestSuiteIsPublic(type)).To.Be.False();
            AssertFunc(() => IsValidTestSuite(type)).To.Throw();
        }

        [Test]
        public static void IsValidTestSuiteThrowWhenNull()
        {
            AssertFunc(() => IsValidTestSuite(null)).To.Throw();
        }

        [Test]
        public static void ChecksAreFalseWhenNull()
        {
            Assert(CheckTestSuiteIsPublic(null)).To.Be.False();
            Assert(CheckTestSuiteIsStatic(null)).To.Be.False();
        }



        [Test]
        public static void PassWhenStatic()
        {
            Assert(CheckTestSuiteIsStatic(typeof(MockValidTestSuite))).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonStatic()
        {
            Type type = typeof(MockNonStaticTestSuite);
            Assert(CheckTestSuiteIsStatic(type)).To.Be.False();
            AssertFunc(() => IsValidTestSuite(type)).To.Throw();
        }
        [Test]
        public static void ThrowWhenInvalid()
        {
            AssertFunc(() => IsValidTestSuite(typeof(MockNonStaticTestSuite))).To.Throw();
            AssertFunc(() => IsValidTestSuite(typeof(MockNonPublicTestSuite))).To.Throw();
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member