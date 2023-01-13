﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Content
{
    internal static class W_Content_Manager
    {
        internal static void add_content()
        {
            W_Content_Helper.init();
            add_prefabs();
            add_actors();
            add_buildings();
            add_cultisys();
            add_item_accesory_materials();
            add_item_armor_materials();
            add_item_weapon_materials();
            add_localized_text();
            add_kingdoms();
            add_races();
            add_special_bodies();
            add_spells();
            add_traits();
            add_world_events();
            add_harmony();
        }

        private static void add_localized_text()
        {
            Harmony.W_Harmony_Others.setLanguage_Postfix(ReflectionUtility.Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "language") as string);
        }

        private static void add_cultisys()
        {
            W_Content_Cultisys.add_cultisys();
        }

        private static void add_prefabs()
        {

        }

        private static void add_actors()
        {

        }
        private static void add_races()
        {

        }
        private static void add_kingdoms()
        {

        }
        private static void add_buildings()
        {

        }
        private static void add_item_accesory_materials()
        {

        }
        private static void add_item_armor_materials()
        {

        }
        private static void add_item_weapon_materials()
        {

        }
        private static void add_special_bodies()
        {

        }
        private static void add_spells()
        {

        }
        private static void add_traits()
        {

        }
        private static void add_world_events()
        {

        }
        private static void add_harmony()
        {
            //HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Actor), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_City), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Item), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_WindowCreatureInfo), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Spell), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Others), Others.CW_Constants.mod_id);
            WorldBoxConsole.Console.print("Finish Harmony");
        }
    }
}
