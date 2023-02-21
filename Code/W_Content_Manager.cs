using System;
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
            add_harmony();
            W_Content_Helper.init();
            add_localized_text();
            add_actors();
            add_buildings();
            add_cultisys();
            add_drops();
            add_item_accesory_materials();
            add_item_armor_materials();
            add_item_weapon_materials();
            add_jobs();
            add_kingdoms_and_races();
            add_special_bodies();
            add_spells();
            add_status_effects();
            add_traits();
            add_god_powers();
            add_world_events();
            add_world_laws();

            add_words_libraries();
            add_name_generators();

            others_init();
            create_tab();
        }

        private static void add_world_laws()
        {
            W_Content_WindowWorldLaw.init();
            W_Content_WorldLaws.add_world_laws();
            W_Content_WorldLaws.add_world_settings();
            W_Content_WorldLaws.add_others_settings();
            try
            {
                Harmony.W_Harmony_WorldLaw.worldLaws_init(MapBox.instance.worldLaws);
            }
            catch(NullReferenceException e)
            {
                MapBox.instance.worldLaws.check();
            }
        }

        private static void others_init()
        {
            W_Content_SpectateUnit.init();
            WindowWorldInspect.init();
        }

        private static void add_jobs()
        {
            W_Content_ActorJob.add_actor_jobs();
            W_Content_ActorJob.modify_actor_jobs();
            W_Content_ActorTask.add_actor_tasks();
        }

        private static void add_name_generators()
        {
            W_Content_NameGenerator.add_name_generators();
        }

        private static void add_words_libraries()
        {
            W_Content_Words_Library.add_words_libraries();
        }

        private static void add_drops()
        {
            W_Content_Drop.add_drops();
        }

        private static void add_god_powers()
        {
            W_Content_GodPower.add_god_powers();
        }

        private static void create_tab()
        {
            W_Content_Tab.create_tab();
            W_Content_Tab.add_buttons();
        }

        private static void add_status_effects()
        {
            W_Content_StatusEffect.add_status_effects();
        }

        private static void add_localized_text()
        {
            Harmony.W_Harmony_Others.setLanguage_Postfix(ReflectionUtility.Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "language") as string);
        }

        private static void add_cultisys()
        {
            W_Content_Cultisys.add_cultisys();
        }

        private static void add_actors()
        {
            W_Content_Actor.add_actors();
        }
        private static void add_kingdoms_and_races()
        {
            W_Content_KingdomAndRace.add_kingdoms_and_races();
        }
        private static void add_buildings()
        {
            W_Content_Building.add_buildings();
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
            W_Content_Spell.add_spells();
        }
        private static void add_traits()
        {
            W_Content_Trait.add_traits();
        }
        private static void add_world_events()
        {
            W_Content_WorldEvent.add_events();
        }
        private static void add_harmony()
        {
            HarmonyLib.Harmony harmony = new HarmonyLib.Harmony(Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Actor), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Banner), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Building), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_City), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Culture), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Item), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_MapMode), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Window), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Spell), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_StatusEffect), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Others), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_Save), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_ChunkInfo), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_WorldLaw), Others.CW_Constants.mod_id);
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Harmony.W_Harmony_FixOrigin), Others.CW_Constants.mod_id);
            /**
            Type NCMS_AllModsWindow = HarmonyLib.AccessTools.TypeByName("AllModsWindow");
            harmony.Patch(
                NCMS_AllModsWindow.GetMethod("setName", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
                ), 
                new HarmonyLib.HarmonyMethod(typeof(Harmony.W_Harmony_Others)
                .GetMethod(nameof(Harmony.W_Harmony_Others.allmodswindow_setName), 
                System.Reflection.BindingFlags.Static|System.Reflection.BindingFlags.Public)
                )
            );
            */
            WorldBoxConsole.Console.print("Finish Harmony");
        }
    }
}
