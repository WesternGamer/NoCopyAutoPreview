using NoCopyAutoPreview.Config;
using HarmonyLib;
using Sandbox.Game.SessionComponents.Clipboard;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NoCopyAutoPreview.Patches
{
    [HarmonyPatch(typeof(MyClipboardComponent), "Copy")]
    internal class MyClipboardComponent_Copy_Patch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var code = new List<CodeInstruction>(instructions);

            var target = code.FindIndex(i => i.opcode == OpCodes.Callvirt && i.operand is MethodInfo method && method.Name.Contains("Activate"));

            Label jump = il.DefineLabel();

            code[target + 1].labels.Add(jump);

            code.Insert(target - 2, new CodeInstruction(OpCodes.Brtrue_S, jump));

            code.Insert(target - 2, new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(IPluginConfig), nameof(IPluginConfig.Enabled))));
            code.Insert(target - 2, new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Config))));
            
            code.Insert(target - 2, new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))));


            Label jump2 = il.DefineLabel();

            code[target + 12].labels.Add(jump2);

            code.Insert(target + 9, new CodeInstruction(OpCodes.Brtrue_S, jump2));

            code.Insert(target + 9, new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(IPluginConfig), nameof(IPluginConfig.Enabled))));
            code.Insert(target + 9, new CodeInstruction(OpCodes.Callvirt, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Config))));

            code.Insert(target + 9, new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(Plugin), nameof(Plugin.Instance))));



            var target2 = code.FindIndex(i => i.opcode == OpCodes.Call && i.operand is MethodInfo method && method.Name.Contains("Activate"));

            code.RemoveAt(target2);
            code.RemoveAt(target2 - 1);

            return code;
        }
    }
}
