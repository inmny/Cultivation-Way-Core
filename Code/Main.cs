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
    public class ModState
    {
        public static ModState instance;
        public List<CW_Addon> addons;
        public bool registered = false;
        public string cur_language = "cz";
        public NCMod mod_info;
        public CW_EffectManager effect_manager;
        public CW_Library_Manager library_manager;
        public Load_Object_Reason load_object_reason = Load_Object_Reason.SPAWN;
    }
    public class World_Data
    {
        public static World_Data instance;
        public CW_MapChunk_Manager map_chunk_manager;
    }
    [ModEntry]
    class Main : MonoBehaviour{
        public static Main instance { get; private set; }
        public ModState mod_state;
        public World_Data world_data;
        public CW_WorldConfig world_config;
        private bool initialized = false;
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
                mod_state.effect_manager = this.gameObject.AddComponent<CW_EffectManager>();
                mod_state.library_manager = CW_Library_Manager.instance;
                mod_state.library_manager.init();

                world_data.map_chunk_manager = new CW_MapChunk_Manager();
                world_data.map_chunk_manager.init(32,32);
                
                Utils.CW_ItemTools.init();
                W_Content_Manager.add_content();

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
                }
            }
            if (last_month!=MapBox.instance.mapStats.month)
            {
                world_data.map_chunk_manager.update();
                last_month = MapBox.instance.mapStats.month;
            }
            
        }
        private void gc()
        {
            GC.Collect();
        }
    }
}
