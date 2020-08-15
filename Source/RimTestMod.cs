using Verse;

namespace RimTest
{
    [StaticConstructorOnStartup]
    public static class RimTestMod
    {
        static RimTestMod() //our constructor
        {
            RimTest.RunTests();
        }
    }
}
