//Original patch from hugslib

namespace RimTest.Patches
{
    /*
	[HarmonyPatch(typeof(DebugWindowsOpener))]
	[HarmonyPatch("DrawButtons")]
	internal class DebugWindowsOpener_Patch
	{
		private static bool patched;

		[HarmonyPrepare]
		public static void Prepare()
		{
			LongEventHandler.ExecuteWhenFinished((Action)delegate
			{
				if (!patched)
				{
					Log.Warning("DebugWindowsOpener_Patch could not be applied.");
				}
			});
		}

		[HarmonyTranspiler]
		public static IEnumerable<CodeInstruction> DrawAdditionalButtons(IEnumerable<CodeInstruction> instructions)
		{

			patched = false;
			CodeInstruction[] instructionsArr = instructions.ToArray();
			int widgetRowIndex = TryGetLocalIndexOfConstructedObject(instructionsArr, typeof(WidgetRow));
			CodeInstruction[] array = instructionsArr;
			foreach (CodeInstruction inst in array)
			{
				if (!patched && widgetRowIndex >= 0 && inst.opcode == OpCodes.Bne_Un)
				{
					yield return new CodeInstruction(OpCodes.Ldloc, widgetRowIndex);
					yield return new CodeInstruction(OpCodes.Call, new Action<WidgetRow>(TestingController.DrawDebugToolbarButton).Method);
					patched = true;
				}
				yield return inst;
			}
		}

		private static int TryGetLocalIndexOfConstructedObject(IEnumerable<CodeInstruction> instructions, Type constructedType, Type[] constructorParams = null)
		{
			ConstructorInfo constructorInfo = AccessTools.Constructor(constructedType, constructorParams);
			int num = -1;
			if (constructorInfo == null)
			{
				Log.Error($"Could not reflect constructor for type {constructedType}: {Environment.StackTrace}");
				return num;
			}
			CodeInstruction codeInstruction = null;
			foreach (CodeInstruction instruction in instructions)
			{
				if (codeInstruction != null && codeInstruction.opcode == OpCodes.Newobj && constructorInfo.Equals(codeInstruction.operand))
				{
					if (instruction.opcode == OpCodes.Stloc_0)
					{
						num = 0;
					}
					else if (instruction.opcode == OpCodes.Stloc_1)
					{
						num = 1;
					}
					else if (instruction.opcode == OpCodes.Stloc_2)
					{
						num = 2;
					}
					else if (instruction.opcode == OpCodes.Stloc_3)
					{
						num = 3;
					}
					else if (instruction.opcode == OpCodes.Stloc && instruction.operand is int)
					{
						num = (int)instruction.operand;
					}
					if (num >= 0)
					{
						break;
					}
				}
				codeInstruction = instruction;
			}
			if (num < 0)
			{
				Log.Error($"Could not determine local index for constructed type {constructedType}: {Environment.StackTrace}");
			}
			return num;
		}
	}
	*/
}