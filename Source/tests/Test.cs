using System.Reflection;
using static RimTest.Assertion;

namespace RimTest.tests
{
    public class MockTests
    {
        protected static void NonPublicTest() { }
        public void NonStaticTest() { }
        public static bool NonVoidReturnTest() { return true; }
        public static void NonParameterFreeTest(bool value) { bool _ = value;}
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

            Assert(RimTest.CheckTestIsPublic(GetMethodInfo("ValidTest"))).To.Be.True();
        }


        [Test]
        public static void PassWhenStatic()
        {
            Assert(RimTest.CheckTestIsStatic(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenParameterFree()
        {
            Assert(RimTest.CheckTestIsParameterFree(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenReturnsVoid()
        {
            Assert(RimTest.CheckTestReturnsVoid(GetMethodInfo("ValidTest"))).To.Be.True();
        }

        [Test]
        public static void PassWhenValid()
        {
            AssertFunc(delegate { RimTest.IsValidTest(GetMethodInfo("ValidTest")); }).Not.To.Throw();
        }

        [Test]
        public static void IsValidTestThrowWhenNull()
        {
            AssertFunc(() => RimTest.IsValidTest(null)).To.Throw();
        }

        [Test]
        public static void ChecksAreFalseWhenNull()
        {
            Assert(RimTest.CheckTestIsPublic(null)).To.Be.False();
            Assert(RimTest.CheckTestIsStatic(null)).To.Be.False();
            Assert(RimTest.CheckTestReturnsVoid(null)).To.Be.False();
            Assert(RimTest.CheckTestIsParameterFree(null)).To.Be.False();
        }

        [Test]
        public static void CheckIsFalseWhenNonPublic()
        {
            Assert(RimTest.CheckTestIsPublic(GetMethodInfo("NonPublicTest"))).To.Be.False();
        }

        [Test]
        public static void CheckIsFalseWhenNonStatic()
        {

            Assert(RimTest.CheckTestIsStatic(GetMethodInfo("NonStaticTest"))).To.Be.False();
        }
        [Test]
        public static void CheckIsFalseWhenNonVoidReturnType()
        {

            Assert(RimTest.CheckTestReturnsVoid(GetMethodInfo("NonVoidReturnTest"))).To.Be.False();
        }
        [Test]
        public static void CheckIsFalseWhenAcceptsParameters()
        {

            Assert(RimTest.CheckTestIsParameterFree(GetMethodInfo("NonParameterFreeTest"))).To.Be.False();
        }

        [Test]
        public static void ThrowWhenInvalid()
        {
            AssertFunc(() => RimTest.IsValidTest(GetMethodInfo("NonParameterFreeTest"))).To.Throw();
            AssertFunc(() => RimTest.IsValidTest(GetMethodInfo("NonVoidReturnTest"))).To.Throw();
            AssertFunc(() => RimTest.IsValidTest(GetMethodInfo("NonStaticTest"))).To.Throw();
            AssertFunc(() => RimTest.IsValidTest(GetMethodInfo("NonPublicTest"))).To.Throw();
        }
    }
}
