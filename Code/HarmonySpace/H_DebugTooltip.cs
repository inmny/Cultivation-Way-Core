using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Others;
using HarmonyLib;
using NeoModLoader.api.attributes;
using NeoModLoader.General;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way.HarmonySpace;

internal static class H_DebugTooltip
{
    private static GroupSpriteObject last_object;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(TooltipLibrary), nameof(TooltipLibrary.showActor))]
    public static void showActor_postfix(string pTitle, Tooltip pTooltip, TooltipData pData)
    {
        CW_Actor actor = (CW_Actor)pData.actor;

        CW_Element element = actor.data.GetElement();
        if (element != null)
        {
            pTooltip.addLineText("cw_element", LocalizedTextManager.getText(element.GetElementType().id),
                Toolbox.colorToHex(element.GetColor()));
        }
    }

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(DebugTextGroupSystem), nameof(DebugTextGroupSystem.update))]
    private static IEnumerable<CodeInstruction> update_postfix(IEnumerable<CodeInstruction> inst)
    {
        var codes = new List<CodeInstruction>(inst);

        int index = codes.FindIndex(code =>
            code.opcode == OpCodes.Call && ((MethodInfo)code.operand).Name == "checkZones");
        CW_Core.LogWarning("index: " + index);
        index++;
        codes.Insert(index++, new CodeInstruction(OpCodes.Ldarg_0));
        codes.Insert(index++, new CodeInstruction(OpCodes.Call,
            AccessTools.Method(typeof(H_DebugTooltip), nameof(check_tile_energy))));

        return codes.AsEnumerable();
    }

    [Hotfixable]
    private static void check_tile_energy(DebugTextGroupSystem pInstance)
    {
        if (last_object != null) last_object.setScale(1);
        if (!PlayerConfig.optionBoolEnabled(Constants.Core.energy_maps_toggle_name)) return;
        if (World.world.worldLaws.dict.TryGetValue(CW_Laws.show_detailed_energy_info, out PlayerOptionData data) &&
            !data.boolVal) return;

        WorldTile tile = MapBox.instance.getMouseTilePos();
        if (tile == null) return;

        string energy_id = CW_Core.mod_state.energy_map_manager.current_map_id;

        CW_EnergyMapTile energy_tile = tile.GetEnergyTile(energy_id);

        GroupSpriteObject @object = pInstance.CallMethod("getNext") as GroupSpriteObject;
        DebugWorldText world_text = @object.gameObject.GetComponent<DebugWorldText>();

        world_text.prepare(LM.Get(energy_id), Toolbox.colorToHex(Colors.default_color), 0.2f);
        world_text.add(LM.Get("cw_energy_density"), (int)(100 * energy_tile.density) / 100f);
        world_text.add(LM.Get("cw_energy_value"), energy_tile.value);
        world_text.fin();

        Vector3 pos = MapBox.instance.getMousePos();

        float scale = World.world.qualityChanger.currentZoom / 5;
        pos.y += scale;
        pos.y = Math.Min(pos.y, Config.TILES_IN_CHUNK * World.world.mapChunkManager.amountY + 16);

        @object.setPosOnly(ref pos);
        @object.setScale(scale);

        last_object = @object;
    }
}