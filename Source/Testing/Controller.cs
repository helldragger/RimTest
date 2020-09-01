namespace RimTest.Testing
{
    /*
    class TestingController
	{
		private static Texture2D testingIconTex;
		internal static void Initialize()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				testingIconTex = ContentFinder<Texture2D>.Get("testingIcon", true);
			});
			//EnumerateMapSizes();
			//if (Prefs.DevMode)
			//{
			//	LongEventHandler.QueueLongEvent((Action)SetupForTesting, (string)null, false, (Action<Exception>)null);
			//}
		}

		internal static void DrawDebugToolbarButton(WidgetRow widgets)
		{
			if (widgets.ButtonIcon(testingIconTex, "Open the testing settings.\n\nThis lets you automatically run tests or load an existing save when the game is started.\nShift-click to quick-run the tests.", null))
			{
				WindowStack windowStack = Find.WindowStack;
				if (HugsLibUtility.ShiftIsHeld)
				{
					windowStack.TryRemove(typeof(Dialog_Testing), true);
					RimTest.RunTests();
				}
				else if (windowStack.IsOpen<Dialog_Testing>())
				{
					windowStack.TryRemove(typeof(Dialog_Testing), true);
				}
				else
				{
					windowStack.Add((Window)(object)new Dialog_Testing());
				}
			}
		}
	}
	*/
}
