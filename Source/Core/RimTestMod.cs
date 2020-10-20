using RimTest.Testing;
using Verse;
using UnityEngine;
using RimTest.Core;

namespace RimTest
{
    /// <summary>
    /// This is the mod entry point run when every assemblies of loaded mods are now loaded and available.
    /// </summary>
    /// <remarks>We can run test discovery at this point.</remarks>
    public class RimTestMod : Mod
    {
        /// <summary>
        /// Said entry point
        /// </summary>
        public RimTestMod(ModContentPack content) : base(content) //our constructor
        {
            Settings = GetSettings<RimTestSettings>();
            Explorer.ExploreAndRegisterAssemblies();
            StatusExplorer.UpdateAllStatusCounts();

            if (Settings.RunAtStartup)
            {
                Runner.RunAllRegisteredTests();
                StatusExplorer.UpdateAllStatusCounts();
                Viewer.LogTestsResults();
            }
        }
        ///<summary>
        /// Settings getter
        ///</summary>
        public static RimTestSettings Settings { get; private set; }
        ///<summary>
        /// Settings UI drawing logic
        ///</summary>
        public override void DoSettingsWindowContents(Rect canvas)
        {
            base.DoSettingsWindowContents(canvas);
            Settings.DoWindowContents(canvas);
        }

        ///<summary>
        /// Mod header in the mod config UI
        ///</summary>
        public override string SettingsCategory() => "RimTest";
    }

    
}
