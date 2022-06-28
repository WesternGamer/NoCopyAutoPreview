using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoCopyAutoPreview.Patches
{
    [HarmonyPatch("Sandbox.Game.Entities.MyFloatingObjectClipboard", "CopyfloatingObject")]
    internal class MyFloatingObjectClipboard_CopyfloatingObject_Patch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = new List<CodeInstruction>(instructions);

            var target = code.FindLastIndex(i => i.opcode == OpCodes.Call && i.operand is MethodInfo method && method.Name.Contains("Activate"));

            code.RemoveAt(target);
            code.RemoveAt(target - 1);

            return code;
        }
    }
}
