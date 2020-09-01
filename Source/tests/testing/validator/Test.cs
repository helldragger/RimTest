using System.Reflection;
using static RimTest.Assertion;
using static RimTest.Testing.Validator;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace RimTest.tests
{
    public class MockTests
    {
        protected static void NonPublicTest() { }
        public void NonStaticTest() { }
        public static bool NonVoidReturnTest() { return true; }
        public static void NonParameterFreeTest(bool value) { bool _ = value; }
        public static void ValidTest() { }
    }

    [TestSuite]
    public static class Testing
    {

        private static MethodInfo GetMethodInfo(string methodName)
        {
            return typeof(MockTests).GetTypeInfo().GetMethod(methodName);
        }

        [Test]
        public static void PassWhenPublic()
        {

            Assert(CheckTestIsPublic(GetMethodInfo("ValidTest"))).To.Be.True();
        }


        [Test]
        public static void PassWhenStatic()
        {
            Assert(CheckTestIsStatic(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenParameterFree()
        {
            Assert(CheckTestIsParameterFree(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenReturnsVoid()
        {
            Assert(CheckTestReturnsVoid(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenValid()
        {
            AssertFunc(delegate { IsValidTest(GetMethodInfo("ValidTest")); }).Not.To.Throw();
        }

        [Test]
        public static void IsValidTestThrowWhenNull()
        {
            AssertFunc(() => IsValidTest(null)).To.Throw();
        }

        [Test]
        public static void ChecksAreFalseWhenNull()
        {
            Assert(CheckTestIsPublic(null)).To.Be.False();
            Assert(CheckTestIsStatic(null)).To.Be.False();
            Assert(CheckTestReturnsVoid(null)).To.Be.False();
            Assert(CheckTestIsParameterFree(null)).To.Be.False();
        }

        [Test]
        public static void CheckIsFalseWhenNonPublic()
        {
            Assert(CheckTestIsPublic(GetMethodInfo("NonPublicTest"))).To.Be.False();
        }

        [Test]
        public static void CheckIsFalseWhenNonStatic()
        {

            Assert(CheckTestIsStatic(GetMethodInfo("NonStaticTest"))).To.Be.False();
        }
        [Test]
        public static void CheckIsFalseWhenNonVoidReturnType()
        {

            Assert(CheckTestReturnsVoid(GetMethodInfo("NonVoidReturnTest"))).To.Be.False();
        }
        [Test]
        public static void CheckIsFalseWhenAcceptsParameters()
        {

            Assert(CheckTestIsParameterFree(GetMethodInfo("NonParameterFreeTest"))).To.Be.False();
        }

        [Test]
        public static void ThrowWhenInvalid()
        {
            AssertFunc(() => IsValidTest(GetMethodInfo("NonParameterFreeTest"))).To.Throw();
            AssertFunc(() => IsValidTest(GetMethodInfo("NonVoidReturnTest"))).To.Throw();
            AssertFunc(() => IsValidTest(GetMethodInfo("NonStaticTest"))).To.Throw();
            AssertFunc(() => IsValidTest(GetMethodInfo("NonPublicTest"))).To.Throw();
        }
    }
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member