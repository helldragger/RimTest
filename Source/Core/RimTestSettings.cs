using Verse;
using UnityEngine;
using RimTest.Testing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using static RimTest.Testing.AssemblyExplorer;
using static RimTest.Testing.AssemblyStatusExtension;
using static RimTest.Testing.TestSuiteExplorer;
using static RimTest.Testing.TestSuiteStatusExtension;
using static RimTest.Testing.TestExplorer;
using static RimTest.Testing.TestStatusExtension;
using System.Text.RegularExpressions;
using Text = Verse.Text;

namespace RimTest.Core
{
    /// <summary>
    /// Main RimTest UI
    /// </summary>
    public class RimTestSettings : ModSettings
    {
        /// <summary>
        /// Decides if the mod run it's own tests or not
        /// </summary>
        public bool RunOwnTests = false;
        /// <summary>
        /// Decides if the mod runs all the available non filtered tests at launch or not
        /// </summary>
        public bool RunAtStartup = true;
        private IDictionary<Assembly, bool> assemblyVisibility = new Dictionary<Assembly, bool>();
        private IDictionary<Type, bool> testSuiteVisibility = new Dictionary<Type, bool>();

        private static int HEIGHT_ROW = 40;
        private static int HEIGHT_CONTROLS = 20;
        private static int WIDTH_ROW_RUN = 40;
        private static int WIDTH_TOGGLE_LABEL = 300;
        private static int WIDTH_TIME_LABEL = 40;
        private static int WIDTH_DETAILS_BTN = 80;
        private static int ERROR_CHAR_LIMIT = 120;
        private static int WIDTH_ERROR_LEVEL = 7;
        private static int WIDTH_LEVEL_INDENT = WIDTH_ERROR_LEVEL;

        private static int WIDTH_CONTROLS_ASM_LABEL = 80;
        private static int WIDTH_CONTROLS_TS_LABEL = WIDTH_CONTROLS_ASM_LABEL;

        private static int WIDTH_CONTROLS_ASM_COLLAPSE = 90;
        private static int WIDTH_CONTROLS_TS_COLLAPSE = WIDTH_CONTROLS_ASM_COLLAPSE;
        private static int WIDTH_CONTROLS_ASM_EXPAND = 90;
        private static int WIDTH_CONTROLS_TS_EXPAND = WIDTH_CONTROLS_ASM_EXPAND;

        private static int WIDTH_CONTROLS_SEARCH_LABEL = 150;
        private static int WIDTH_CONTROLS_VIEW_LABEL = 60;
        private static int WIDTH_CONTROLS_VIEW_CONTROL = 80;
        private static int WIDTH_CONTROLS_SECTION_GAP = 20;
        private static int WIDTH_CONTROLS_CONTROL_GAP = 10;

        private static Color COLOR_FAIL = new Color(0.808f, 0.216f, 0.549f);
        private static Color COLOR_WARN = new Color(1f, 0.639f, 0.267f);
        private static Color COLOR_PASS = new Color(0.706f, 0.933f, 0.251f);
        private static Color COLOR_UNKNOWN = new Color(0.184f, 0.533f, 0.631f);

        private string searchRegex = @"";

        private static Vector2 ScrollPosition = Vector2.zero;
       /// <summary>
       /// Data exposition for serialization and keeping settings saved between game runs
       /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref RunOwnTests, "RunOwnTests", true);
            Scribe_Values.Look(ref RunAtStartup, "RunAtStartup", true);

        }

        /// <summary>
        /// Draws the RimTest UI main canvas
        /// </summary>
        /// <param name="canvas"></param>
        public void DoWindowContents(Rect canvas)
        {
            StatusExplorer.UpdateAllStatusCounts();
            var options = new Listing_Standard();
            
            options.Begin(canvas);
            options.CheckboxLabeled("Include RimTests' own test suite", ref RunOwnTests, "if enabled, RimTest will run its' own test suite as well as any mod test suites.");
            options.CheckboxLabeled("Run tests at startup", ref RunAtStartup, "if enabled, RimTest will run every valid tests at the game startup.");
            options.GapLine();
            //if (options.ButtonText("Run everything"))
            //{
            //    Runner.RunAllRegisteredTests();
            //    StatusExplorer.UpdateAllStatusCounts();
            //}
            //if(options.ButtonText("Log all current results to Debug log"))
            //{
            //    Viewer.LogTestsResults();
            //}
            //options.GapLine();
            DrawControls(options);
            options.End(); 
            
            Rect GUIRect = canvas.BottomPartPixels(canvas.height - options.CurHeight);
            Rect viewRect = GUIRect.LeftPartPixels(GUIRect.width - 16).AtZero();
            viewRect.Set(viewRect.xMin, viewRect.yMin, viewRect.width, CalcTotalHeight()); // update height to accomodate to all the data

            var testlisting = new Listing_Standard();
            //testlisting.BeginScrollView(GUIRect, ref ScrollPosition, ref viewRect);
            Widgets.BeginScrollView(GUIRect, ref ScrollPosition, viewRect, true);
            GUI.BeginGroup(viewRect);
            testlisting.Begin(viewRect);
            DrawTests(testlisting);
            testlisting.End();
            GUI.EndGroup();
            //testlisting.EndScrollView(ref viewRect);
            Widgets.EndScrollView();

            //reset fonts
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            //Widgets.CustomButtonText(ref labelRect, "test assembly", Color.black, Color.white, Color.red);

            // RimTest -- displayA  displayB ...
            //                                 Status: ✘ERROR  !WARNING  ➥SKIPPED  ?NOT RAN  ✓PASS
            // Assemblies: |collapse| |expand |         [x] 2    [v] 1                [v] 1    [v] 1
            // TestSuites: |collapse| |expand |         [x] 3    [v] 1    [x] 1       [v] 12   [v] 4
            // Tests:      |run all | |log all|         [x] 10            [x] 4       [v] 23   [v] 16
            // filter regex: |_________________
            // V THIS PART V
            // |run| |time|   ||v [v] Assembly 1|       | 
            // |run| |time|   ||^ [x] Assembly 2|       | error msg
            // |run| |time|    ||v [v] TestSuite 2.1|   | 
            // |run| |time|    ||^ [x] TestSuite 2.2|   | error msg
            // |run| |time|     ||v [x] Test 2.2.1|     | error msg
            // |run| |time|     ||^ [v] Test 2.2.1|     | 
            // ...

            //TODO Status filter , utiliser colonnes + sliders
            //-----          [Skipped]  |  [Passed]
            //[Assemblies]   [x] [100]  |
            //[Test Suites][x][010] |
            //[Tests][x][000] |

            // TODO Statuses symbols
            // ?? pas ouf
            // !! pas ouf

            // TODO control rows size doivent être aussi gros que les checkbox.
        }

        private void DrawControls(Listing_Standard bar)
        {

            Rect rowAsm = bar.GetRect(HEIGHT_CONTROLS);
            Rect rowTS = bar.GetRect(HEIGHT_CONTROLS);
            Rect rowT = bar.GetRect(HEIGHT_CONTROLS);

            // ASM - COLLAPSE / EXPAND - STATUSES
            Rect asmLabelRect = rowAsm.LeftPartPixels(WIDTH_CONTROLS_ASM_LABEL);
            Rect asmCollapseRect = rowAsm.RightPartPixels(rowAsm.width - asmLabelRect.xMax).LeftPartPixels(WIDTH_CONTROLS_ASM_COLLAPSE);
            Rect asmExpandRect = rowAsm.RightPartPixels(rowAsm.width - asmCollapseRect.xMax).LeftPartPixels(WIDTH_CONTROLS_ASM_EXPAND);
            Widgets.Label(asmLabelRect, "Assemblies");
            if (Widgets.ButtonText(asmCollapseRect, "Collapse"))
            {
                CollapseAllAssemblies();
            }
            if (Widgets.ButtonText(asmExpandRect, "Expand"))
            {
                ExpandAllAssemblies();
            }
            Rect statusAsmLabelRect = rowAsm.RightPartPixels(rowAsm.width - asmExpandRect.xMax - WIDTH_CONTROLS_SECTION_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_LABEL);
            Rect statusAsmControllerRect = rowAsm.RightPartPixels(rowAsm.width - statusAsmLabelRect.xMax);

            Rect statusAsmFailRect = statusAsmControllerRect.LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusAsmWarnRect = statusAsmControllerRect.RightPartPixels(statusAsmControllerRect.width - statusAsmFailRect.width - WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusAsmSkipRect = statusAsmControllerRect.RightPartPixels(statusAsmControllerRect.width - 2 * statusAsmWarnRect.width - 2 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusAsmUnknownRect = statusAsmControllerRect.RightPartPixels(statusAsmControllerRect.width - 3 * statusAsmSkipRect.width - 3 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusAsmPassRect = statusAsmControllerRect.RightPartPixels(statusAsmControllerRect.width - 4 * statusAsmUnknownRect.width - 4 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);

            Widgets.CheckboxLabeled(statusAsmFailRect, $"{StatusExplorer.GetAssemblyStatusCount(AssemblyStatus.ERROR)} {StatusSymbol(AssemblyStatus.ERROR)}", ref FilteredExplorer.failEnabledAsm);

            Widgets.CheckboxLabeled(statusAsmWarnRect, $"{StatusExplorer.GetAssemblyStatusCount(AssemblyStatus.WARNING)} {StatusSymbol(AssemblyStatus.WARNING)}", ref FilteredExplorer.warningEnabledAsm);

            Widgets.CheckboxLabeled(statusAsmUnknownRect, $"{StatusExplorer.GetAssemblyStatusCount(AssemblyStatus.UNKNOWN)} {StatusSymbol(AssemblyStatus.UNKNOWN)}", ref FilteredExplorer.unknownEnabledAsm);

            Widgets.CheckboxLabeled(statusAsmPassRect, $"{StatusExplorer.GetAssemblyStatusCount(AssemblyStatus.PASS)} {StatusSymbol(AssemblyStatus.PASS)}", ref FilteredExplorer.passEnabledAsm);

            // TESTSUITE - COLLAPSE / EXPAND - STATUSES
            Rect tsLabelRect = rowTS.LeftPartPixels(WIDTH_CONTROLS_TS_LABEL);
            Rect tsCollapseRect = rowTS.RightPartPixels(rowTS.width - tsLabelRect.xMax).LeftPartPixels(WIDTH_CONTROLS_TS_COLLAPSE);
            Rect tsExpandRect = rowTS.RightPartPixels(rowTS.width - tsCollapseRect.xMax).LeftPartPixels(WIDTH_CONTROLS_TS_EXPAND);
            
            Widgets.Label(tsLabelRect, "Test Suites");
            if (Widgets.ButtonText(tsCollapseRect, "Collapse"))
            {
                CollapseAllTestSuites();
            }
            if (Widgets.ButtonText(tsExpandRect, "Expand"))
            {
                ExpandAllTestSuites();
            }
            Rect statusTSLabelRect = rowTS.RightPartPixels(rowTS.width - tsExpandRect.xMax - WIDTH_CONTROLS_SECTION_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_LABEL);
            Rect statusTSControllerRect = rowTS.RightPartPixels(rowTS.width - statusTSLabelRect.xMax);

            Rect statusTSFailRect = statusTSControllerRect.LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTSWarnRect = statusTSControllerRect.RightPartPixels(statusTSControllerRect.width - statusTSFailRect.width - WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTSSkipRect = statusTSControllerRect.RightPartPixels(statusTSControllerRect.width - 2 * statusTSWarnRect.width - 2 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTSUnknownRect = statusTSControllerRect.RightPartPixels(statusTSControllerRect.width - 3 * statusTSSkipRect.width - 3 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTSPassRect = statusTSControllerRect.RightPartPixels(statusTSControllerRect.width - 4 * statusTSUnknownRect.width - 4 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);

            Widgets.CheckboxLabeled(statusTSFailRect, $"{StatusExplorer.GetTestSuiteStatusCount(TestSuiteStatus.ERROR)} {StatusSymbol(TestSuiteStatus.ERROR)}", ref FilteredExplorer.failEnabledTS);

            Widgets.CheckboxLabeled(statusTSWarnRect, $"{StatusExplorer.GetTestSuiteStatusCount(TestSuiteStatus.WARNING)} {StatusSymbol(TestSuiteStatus.WARNING)}", ref FilteredExplorer.warningEnabledTS);

            Widgets.CheckboxLabeled(statusTSSkipRect, $"{StatusExplorer.GetTestSuiteStatusCount(TestSuiteStatus.SKIP)} {StatusSymbol(TestSuiteStatus.SKIP)}", ref FilteredExplorer.skipEnabledTS);

            Widgets.CheckboxLabeled(statusTSUnknownRect, $"{StatusExplorer.GetTestSuiteStatusCount(TestSuiteStatus.UNKNOWN)} {StatusSymbol(TestSuiteStatus.UNKNOWN)}", ref FilteredExplorer.unknownEnabledTS);

            Widgets.CheckboxLabeled(statusTSPassRect, $"{StatusExplorer.GetTestSuiteStatusCount(TestSuiteStatus.PASS)} {StatusSymbol(TestSuiteStatus.PASS)}", ref FilteredExplorer.passEnabledTS);

            // TEST - RUN ALL / LOG ALL - STATUSES

            Rect tLabelRect = rowT.LeftPartPixels(WIDTH_CONTROLS_TS_LABEL);
            Rect tRunAllRect = rowT.RightPartPixels(rowT.width - tLabelRect.xMax).LeftPartPixels(WIDTH_CONTROLS_TS_COLLAPSE);
            Rect tLogAllRect = rowT.RightPartPixels(rowT.width - tRunAllRect.xMax).LeftPartPixels(WIDTH_CONTROLS_TS_EXPAND);
            Widgets.Label(tLabelRect, "Tests");
            if (Widgets.ButtonText(tRunAllRect, "Run all"))
            {
                Runner.RunAllRegisteredTests();
                StatusExplorer.UpdateAllStatusCounts();
                TimeElapsedExplorer.UpdateAllAssembliesTimeElapsed();
            }
            if (Widgets.ButtonText(tLogAllRect, "Log all"))
            {
                Viewer.LogTestsResults();
            }

            Rect statusTLabelRect = rowT.RightPartPixels(rowT.width - tLogAllRect.xMax - WIDTH_CONTROLS_SECTION_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_LABEL);
            Rect statusTControllerRect = rowT.RightPartPixels(rowT.width - statusTLabelRect.xMax);

            Rect statusTFailRect = statusTControllerRect.LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTSkipRect = statusTControllerRect.RightPartPixels(statusTControllerRect.width - 2*statusTFailRect.width - 2*WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTUnknownRect = statusTControllerRect.RightPartPixels(statusTControllerRect.width - 3 * statusTSkipRect.width - 3 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);
            Rect statusTPassRect = statusTControllerRect.RightPartPixels(statusTControllerRect.width - 4 * statusTUnknownRect.width - 4 * WIDTH_CONTROLS_CONTROL_GAP).LeftPartPixels(WIDTH_CONTROLS_VIEW_CONTROL);

            Widgets.CheckboxLabeled(statusTFailRect, $"{StatusExplorer.GetTestStatusCount(TestStatus.ERROR)} {StatusSymbol(TestStatus.ERROR)}", ref FilteredExplorer.failEnabledT);

            Widgets.CheckboxLabeled(statusTSkipRect, $"{StatusExplorer.GetTestStatusCount(TestStatus.SKIP)} {StatusSymbol(TestStatus.SKIP)}", ref FilteredExplorer.skipEnabledT);

            Widgets.CheckboxLabeled(statusTUnknownRect, $"{StatusExplorer.GetTestStatusCount(TestStatus.UNKNOWN)} {StatusSymbol(TestStatus.UNKNOWN)}", ref FilteredExplorer.unknownEnabledT);

            Widgets.CheckboxLabeled(statusTPassRect, $"{StatusExplorer.GetTestStatusCount(TestStatus.PASS)} {StatusSymbol(TestStatus.PASS)}", ref FilteredExplorer.passEnabledT);

            //// SEARCH
            Rect rowSearch = bar.GetRect(HEIGHT_CONTROLS);
            Rect searchLabelRect = rowSearch.LeftPartPixels(WIDTH_CONTROLS_SEARCH_LABEL);
            Rect searchControlRect = rowSearch.RightPartPixels(rowSearch.width - searchLabelRect.xMax);

            Widgets.Label(searchLabelRect, "Search :");
            try
            {
                string searchRegexTmp = Widgets.TextField(searchControlRect, searchRegex);
                if (searchRegexTmp != searchRegex)
                {
                    searchRegex = searchRegexTmp;
                    FilteredExplorer.UpdateFilter(new Regex(searchRegex));
                    
                }
            }
            catch
            {
                FilteredExplorer.UpdateFilter(new Regex(@""));
            }
        }

        private int CalcTotalHeight()
        {
            int count = 0;
            foreach(Assembly asm in FilteredExplorer.GetFilteredAssemblies())
            {
                count += 1;
                if (!GetAssemblyVisibility(asm)) continue;
                
                foreach(Type ts in FilteredExplorer.GetFilteredTestSuites(asm))
                {
                    count += 1;
                    if (!GetTestSuiteVisibility(ts)) continue;
                    count += FilteredExplorer.GetFilteredTests(ts).EnumerableCount();
                }
            }
            return count * HEIGHT_ROW; 
        }

        private void ToggleAllAssemblies(bool toggle)
        {
            foreach (Assembly asm in FilteredExplorer.GetFilteredAssemblies())
            {
                assemblyVisibility[asm] = toggle;
            }
        }
        private void CollapseAllAssemblies()
        {
            ToggleAllAssemblies(false);
        }
        private void ExpandAllAssemblies()
        {
            ToggleAllAssemblies(true);
        }
        private void ToggleAllTestSuites(bool toggle)
        {
            foreach (Assembly asm in FilteredExplorer.GetFilteredAssemblies())
            {
                foreach(Type ts in FilteredExplorer.GetFilteredTestSuites(asm))
                {
                    testSuiteVisibility[ts] = toggle;
                }
            }
        }
        private void CollapseAllTestSuites()
        {
            ToggleAllTestSuites(false);
        }
        private void ExpandAllTestSuites()
        {
            ToggleAllTestSuites(true);
        }

        private void DrawTests(Listing_Standard listing)
        {
            foreach (Assembly asm in FilteredExplorer.GetFilteredAssemblies())
            {
                DrawAssemblyLine(listing, asm);
            }
        }

        private bool GetAssemblyVisibility(Assembly asm)
        {
            if (!assemblyVisibility.ContainsKey(asm)) assemblyVisibility[asm] = true;
            return assemblyVisibility[asm];
        }

        private void toggleAssemblyVisibility(Assembly asm)
        {
            assemblyVisibility[asm] = !GetAssemblyVisibility(asm);
        }

        private void DrawAssemblyLine(Listing_Standard listing, Assembly asm)
        {
            

            Rect row = listing.GetRect(HEIGHT_ROW);
            Rect runBtnRect = row.LeftPartPixels(WIDTH_ROW_RUN);
            Rect timeRect = row.RightPartPixels(row.width - runBtnRect.xMax).LeftPartPixels(WIDTH_TIME_LABEL);
            Rect errorRect = row.RightPartPixels(row.width - (timeRect.xMax + WIDTH_LEVEL_INDENT*0)).LeftPartPixels(WIDTH_ERROR_LEVEL);
            Rect toggleLabelRect = row.RightPartPixels(row.width - errorRect.xMax).LeftPartPixels(WIDTH_TOGGLE_LABEL);
            Rect messageRect = row.RightPartPixels(row.width - toggleLabelRect.xMax);
            messageRect = messageRect.LeftPartPixels(messageRect.width - WIDTH_DETAILS_BTN);
            Rect detailsRect = row.RightPartPixels(WIDTH_DETAILS_BTN);

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;
            // test run button
            if (Widgets.ButtonText(runBtnRect, "run"))
            //    if (Widgets.ButtonText(new Rect(new Vector2(row.xMin, row.yMin + currentRow * HEIGHT_ROW + HEIGHT_OPTIONS), SIZE_RUN_BTN), "run"))
            {
                Runner.RunAssembly(asm);
                TimeElapsedExplorer.UpdateAssemblyTimeElapsed(asm);
            }

            // Todo time data
            double timeElapsed = TimeElapsedExplorer.GetAssemblyTimeElapsed(asm);
            Widgets.Label(timeRect, $"{(timeElapsed == -1 ? "--" : $"{timeElapsed}")} ms");

            // error level tick
            Color color;
            switch (GetAssemblyStatus(asm))
            {
                case AssemblyStatus.ERROR:
                    color = COLOR_FAIL;
                    break;
                case AssemblyStatus.WARNING:
                    color = COLOR_WARN;
                    break;
                case AssemblyStatus.PASS:
                    color = COLOR_PASS;
                    break;
                case AssemblyStatus.UNKNOWN:
                default:
                    color = COLOR_UNKNOWN;
                    break;
            }
            Widgets.DrawBoxSolid(errorRect, color);

            // collapse / expand button
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.MiddleLeft;
            StringBuilder labelbuilder = new StringBuilder();
            labelbuilder.Append(GetAssemblyVisibility(asm) ? "[-] " : "[+] ");
            labelbuilder.Append($"{StatusSymbol(GetAssemblyStatus(asm))} ");
            labelbuilder.Append(asm.GetName().Name);
            if (Widgets.ButtonText(toggleLabelRect, labelbuilder.ToString(), false))
            {
                toggleAssemblyVisibility(asm);
            }

            // results
            string text = GetAssemblyError(asm) != null ? GetAssemblyError(asm).ToString() : "";
            DrawErrorZone(messageRect, detailsRect, text);

            // next lines: test suites
            if (GetAssemblyVisibility(asm))
            {
                foreach(var ts in FilteredExplorer.GetFilteredTestSuites(asm))
                {
                    DrawTestSuiteLine(listing, ts);
                }
            }
        }


        private bool GetTestSuiteVisibility(Type ts)
        {
            if (!testSuiteVisibility.ContainsKey(ts)) testSuiteVisibility[ts] = true;
            return testSuiteVisibility[ts];
        }

        private void toggleTestSuiteVisibility(Type ts)
        {
            testSuiteVisibility[ts] = !GetTestSuiteVisibility(ts);
        }

        private void DrawTestSuiteLine(Listing_Standard listing, Type ts)
        {
            Rect row = listing.GetRect(HEIGHT_ROW);
            Rect runBtnRect = row.LeftPartPixels(WIDTH_ROW_RUN);
            Rect timeRect = row.RightPartPixels(row.width - runBtnRect.xMax).LeftPartPixels(WIDTH_TIME_LABEL);
            Rect errorRect = row.RightPartPixels(row.width - (timeRect.xMax + WIDTH_LEVEL_INDENT * 1)).LeftPartPixels(WIDTH_ERROR_LEVEL);
            Rect toggleLabelRect = row.RightPartPixels(row.width - errorRect.xMax).LeftPartPixels(WIDTH_TOGGLE_LABEL);
            Rect messageRect = row.RightPartPixels(row.width - toggleLabelRect.xMax);
            messageRect = messageRect.LeftPartPixels(messageRect.width - WIDTH_DETAILS_BTN);
            Rect detailsRect = row.RightPartPixels(WIDTH_DETAILS_BTN);

            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;
            // test run button
            if (Widgets.ButtonText(runBtnRect, "run"))
            {

                Runner.RunTestSuite(ts);
                TimeElapsedExplorer.UpdateAssemblyTimeElapsed(ts.Assembly);
            }
            // Todo time data
            double timeElapsed = TimeElapsedExplorer.GetTestSuiteTimeElapsed(ts);
            Widgets.Label(timeRect, $"{(timeElapsed == -1 ? "--" : $"{timeElapsed}")} ms");
            Color color;

            switch (GetTestSuiteStatus(ts))
            {
                case TestSuiteStatus.WARNING:
                case TestSuiteStatus.SKIP:
                    color = COLOR_WARN;
                    break;
                case TestSuiteStatus.ERROR:
                    color = COLOR_FAIL;
                    break;
                case TestSuiteStatus.PASS:
                    color = COLOR_PASS;
                    break;
                case TestSuiteStatus.UNKNOWN:
                default:
                    color = COLOR_UNKNOWN;
                    break;
            }
            Widgets.DrawBoxSolid(errorRect, color);


            // collapse / expand button
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleLeft;
            StringBuilder labelbuilder = new StringBuilder();
            labelbuilder.Append(GetTestSuiteVisibility(ts) ? "[-] " : "[+] ");
            labelbuilder.Append($"{StatusSymbol(GetTestSuiteStatus(ts))} ");
            labelbuilder.Append(ts.Name);
            if (Widgets.ButtonText(toggleLabelRect, labelbuilder.ToString(), false))
            {
                toggleTestSuiteVisibility(ts);
            }

            // results

            string text = GetTestSuiteError(ts) != null ? GetTestSuiteError(ts).ToString() : "";
            DrawErrorZone(messageRect, detailsRect, text);

            // next lines: test suites
            if (GetTestSuiteVisibility(ts))
            {
                foreach (var test in FilteredExplorer.GetFilteredTests(ts))
                {
                    DrawTestLine(listing, test);
                }
            }
        }


        private void DrawTestLine(Listing_Standard listing, MethodInfo test)
        {
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;

            Rect row = listing.GetRect(HEIGHT_ROW);
            Rect runBtnRect = row.LeftPartPixels(WIDTH_ROW_RUN);
            Rect timeRect = row.RightPartPixels(row.width - runBtnRect.xMax).LeftPartPixels(WIDTH_TIME_LABEL);
            Rect errorRect = row.RightPartPixels(row.width - (timeRect.xMax + WIDTH_LEVEL_INDENT * 2)).LeftPartPixels(WIDTH_ERROR_LEVEL);
            Rect toggleLabelRect = row.RightPartPixels(row.width - errorRect.xMax).LeftPartPixels(WIDTH_TOGGLE_LABEL);
            Rect messageRect = row.RightPartPixels(row.width - toggleLabelRect.xMax);
            messageRect = messageRect.LeftPartPixels(messageRect.width-WIDTH_DETAILS_BTN);
            Rect detailsRect = row.RightPartPixels(WIDTH_DETAILS_BTN);

            // test run button
            if (Widgets.ButtonText(runBtnRect, "run"))
            {
                Runner.RunTest(test);
                TimeElapsedExplorer.UpdateAssemblyTimeElapsed(test.DeclaringType.Assembly);
            }
            // Todo time data
            double timeElapsed = TimeElapsedExplorer.GetTestTimeElapsed(test);
            Widgets.Label(timeRect, $"{(timeElapsed == -1 ? "--": $"{timeElapsed}")} ms");
            Color color;
            switch (GetTestStatus(test))
            {
                case TestStatus.SKIP:
                    color = COLOR_WARN;
                    break;
                case TestStatus.ERROR:
                    color = COLOR_FAIL;
                    break;
                case TestStatus.PASS:
                    color = COLOR_PASS;
                    break;
                case TestStatus.UNKNOWN:
                default:
                    color = COLOR_UNKNOWN;
                    break;
            }
            Widgets.DrawBoxSolid(errorRect, color);

            // collapse / expand button
            Text.Anchor = TextAnchor.MiddleLeft;
            StringBuilder labelbuilder = new StringBuilder();
            labelbuilder.Append($"{StatusSymbol(GetTestStatus(test))} ");
            labelbuilder.Append(test.Name);
            Widgets.Label(toggleLabelRect, labelbuilder.ToString());

            // results
            string text = GetTestError(test) != null ? GetTestError(test).ToString() : "";
            DrawErrorZone(messageRect, detailsRect, text);
        }

        private void DrawErrorZone(Rect messageRect, Rect detailsRect, string text)
        {
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleLeft;
            if (text.Length > 0)
            {
                if (text.Length > ERROR_CHAR_LIMIT)
                {
                    if (Widgets.ButtonText(detailsRect, "To debug log"))
                    {
                        // TODO
                        Log.Error(text);
                    }
                    text = text.Substring(0, ERROR_CHAR_LIMIT - 5) + "[...]";
                }
                Widgets.TextArea(messageRect, text.Replace("\n", " "), true);
            }
        }
    }
}
