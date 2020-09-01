using RimTest.Testing;
using Verse;
namespace RimTest
{
    /// <summary>
    /// This is the mod entry point run when every assemblies of loaded mods are now loaded and available.
    /// </summary>
    /// <remarks>We can run test discovery at this point.</remarks>
    [StaticConstructorOnStartup]
    public static class RimTestMod
    {
        /// <summary>
        /// Said entry point
        /// </summary>
        static RimTestMod() //our constructor
        {
            Explorer.ExploreAndRegisterAssemblies();
            Runner.RunAllRegisteredTests();
            Viewer.LogTestsResults();
        }
    }
}
