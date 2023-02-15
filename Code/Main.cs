using System;
using NCMS;
using UnityEngine;
using Cultivation_Way.Content;
using Cultivation_Way.Library;
using System.Collections.Generic;
using System.Reflection;
using Cultivation_Way.Animation;
namespace Cultivation_Way{
    public enum Load_Object_Reason
    {
        SPAWN,
        LOAD_SAVES
    }
    public enum Loading_Save_Type
    {
        ORIGIN,
        CW
    }
    public enum Destroy_Unit_Reason
    {
        KILL,
        CLEAR
    }

    public class ModState
    {
        public static ModState instance;
        public List<CW_Addon> addons;
        public bool registered = false;
        public string cur_language = "cz";
        public string map_mode = "";
        public NCMod mod_info;
        internal CW_Spell_Manager spell_manager;
        public CW_EffectManager effect_manager;
        public CW_Library_Manager library_manager;
        public Destroy_Unit_Reason destroy_unit_reason = Destroy_Unit_Reason.KILL;
        public Loading_Save_Type loading_save_type = Loading_Save_Type.CW;
        public Load_Object_Reason load_object_reason = Load_Object_Reason.LOAD_SAVES;
    }
    public class World_Data
    {
        public static World_Data instance;
        public CW_MapChunk_Manager map_chunk_manager;
    }
    [ModEntry]
    public class Main : MonoBehaviour{
        public static Main instance { get; private set; }
        public ModState mod_state;
        public World_Data world_data;
        public CW_WorldConfig world_config;
        internal bool initialized = false;
        private bool mod_state_prepared = false;
        private int last_month;
        void Awake(){
            instance = this;
            if (!mod_state_prepared)
            {
                mod_state_prepared = true;
                mod_state = new ModState();
                ModState.instance = mod_state;
                mod_state.addons = new List<CW_Addon>();
            }
            print("[CW Core]: Awake");

        }
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
                CW_Library_Manager.create();
                world_data = new World_Data();
                world_config = new CW_WorldConfig();
                
                World_Data.instance = world_data;
                foreach (NCMod ncmod in NCMS.ModLoader.Mods)
                {
                    if (ncmod.name == Others.CW_Constants.mod_name) { mod_state.mod_info = ncmod; break; }
                }
                mod_state.spell_manager = new CW_Spell_Manager();
                CW_Spell_Manager.instance = mod_state.spell_manager;
                mod_state.effect_manager = this.gameObject.AddComponent<CW_EffectManager>();
                mod_state.library_manager = CW_Library_Manager.instance;
                mod_state.library_manager.init();

                world_data.map_chunk_manager = new CW_MapChunk_Manager();
                
                Utils.CW_ItemTools.init();
                W_Content_Manager.add_content();

                world_data.map_chunk_manager.init(32, 32);
                //mod_state.library_manager.register();
                print("[CW Core]: Finish Initialization");
            }
            if (!mod_state.registered)
            {
                bool all_addons_loaded = true;
                foreach(CW_Addon addon in mod_state.addons)
                {
                    if (!addon.initialized) all_addons_loaded = false;
                }
                if (all_addons_loaded)
                {
                    mod_state.registered = true;
                    mod_state.library_manager.register();
                    W_Content_Tab.apply_buttons();
                }
            }
            if (last_month!=MapBox.instance.mapStats.month)
            {
                world_data.map_chunk_manager.update();
                last_month = MapBox.instance.mapStats.month;
            }
            
        }
        void OnDisable()
        {
            Window_Cultisys_Name_Setting.save_to_file();
            Window_Cultisys_Stats_Setting.save_to_file();
        }
        private void gc()
        {
            GC.Collect();
        }
    }
}
