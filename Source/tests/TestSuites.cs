using System;
using System.Reflection;
using static RimTest.Assertion;

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
            Type type = typeof(MockValidTestSuite);
            AssertFunc( () => RimTest.IsValidTestSuite(type) ).Not.To.Throw();
        }

        [Test]
        public static void PassWhenPublic()
        {
            Type type = typeof(MockValidTestSuite).GetTypeInfo();
            Assert(RimTest.CheckTestSuiteIsPublic(type)).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonPublic()
        {
            Type type = typeof(MockNonPublicTestSuite).GetTypeInfo();
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
            Type type = typeof(MockValidTestSuite).GetTypeInfo();
            Assert(RimTest.CheckTestSuiteIsStatic(type)).To.Be.True();
        }

        [Test]
        public static void ThrowWhenNonStatic()
        {
            Type type = typeof(MockNonStaticTestSuite).GetTypeInfo();
            Assert(RimTest.CheckTestSuiteIsStatic(type)).To.Be.False();
            AssertFunc(() => RimTest.IsValidTestSuite(type)).To.Throw();
        }
        [Test]
        public static void ThrowWhenInvalid()
        {
            Type type = typeof(MockNonStaticTestSuite).GetTypeInfo();
            AssertFunc(() => RimTest.IsValidTestSuite(type)).To.Throw();
            type = typeof(MockNonPublicTestSuite).GetTypeInfo();
            AssertFunc(() => RimTest.IsValidTestSuite(type)).To.Throw();
        }
    }
}
