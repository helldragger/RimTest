namespace RimTest
{
    interface ITestRunner
    {
        void LoadTestMap();
        void RunBeforeTest();
        void RunTest();
        void RunAfterTest();
        void RegisterError(string cause);
        bool HasTestSucceeded();
        string GetTestError();
    }
}
