namespace RimTest.Testing
{
    /*
    static class HarmonyPatches
    {

        static WidgetRow widgetRow(DebugWindowsOpener obj)
        {
            return (WidgetRow)obj.GetType().GetField("widgetRow", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
        }

        public static readonly Texture2D OpenInspectSettings = ContentFinder<Texture2D>.Get("UI/Buttons/DevRoot/OpenInspectSettings", true);

        static void DrawButtons_postfix(DebugWindowsOpener __instance)
        {
            if (widgetRow(__instance).ButtonIcon(OpenInspectSettings, "Open GuiMinilib tests list."))
            {
                //typeof(DebugWindowsOpener).GetMethod("ToggleDebugInspector", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, null);
                
            }
        }
    }

    public class Dialog_Testing
    {
        private int toolbarInt = 0;
        private string[] toolbarStrings = { "Overview", "Other", "maybe" };

        private Vector2 scrollViewVector = Vector2.zero;
        private string innerText = "I am inside the ScrollView";

        private string[] modlist = { "mod1", "mod2" };
        private string[] modtestsuites = { "testsuite1", "testsuite2" };
        private string[] modtestsuitetests = { "test1", "test2", "test3" };

        void onOverviewGUI()
        {
            // Begin the ScrollView
            scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);

            // Put something inside the ScrollView
            innerText = GUILayout.TextArea(innerText);

            foreach (var mod in modlist)
            {
                foreach (var testsuite in modtestsuites)
                {
                    foreach (var test in modtestsuitetests)
                    {
                        GUILayout.Label(test);
                    }
                }
            }

            // End the ScrollView
            GUILayout.EndScrollView();
        }

        void OnGUI(Rect window)
        {
            if (GUILayout.Button(new GUIContent("Run Tests", "If you have a lot of tests it might take from a second to a few minutes")))
                Debug.Log("Hello!");
            toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings);
            switch (toolbarInt)
            {
                case 0:
                    onOverviewGUI();
                    break;
                default:
                    break;
            }
        }
    }
    */
}
