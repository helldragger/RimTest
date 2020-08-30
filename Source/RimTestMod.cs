using UnityEngine;
using Verse;

namespace RimTest
{
    /// <summary>
    /// This is the mod entry point run when every assemblies of loaded mods are now loaded and available.
    /// </summary>
    /// <remarks>We can run test discovery at this point.</remarks>
    public class RimTestMod: Mod
    {
        /// <summary>
        /// Said entry point
        /// </summary>
        public RimTestMod( ModContentPack content ) : base( content )
        {
            Settings = GetSettings<RimTestSettings>();
            RimTest.RunTests();
        }

        public static   RimTestSettings Settings { get; private set; }
        public override void            DoSettingsWindowContents( Rect canvas )
        {
            base.DoSettingsWindowContents( canvas );
            Settings.DoWindowContents( canvas );
        }

        public override string SettingsCategory() => "RimTest";
    }

    public class RimTestSettings : ModSettings
    {
        public bool RunOwnTests = true;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look( ref RunOwnTests, "RunOwnTests", true );
        }

        public void DoWindowContents( Rect canvas )
        {
            var options = new Listing_Standard();

            options.Begin( canvas );
            options.CheckboxLabeled("Include RimTests' own test suite", ref RunOwnTests, "if enabled, RimTest will run its' own test suite as well as any mod test suites.");
            options.GapLine();
            if ( options.ButtonText( "Run tests now" ) )
            {
                RimTest.RunTests();
            }
            options.End();
        }
    }
}
