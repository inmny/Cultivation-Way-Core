using System;
using NCMS;
using UnityEngine;
using Cultivation_Way.Content;
using Cultivation_Way.Library;
using System.Collections.Generic;
using System.Reflection;
using Cultivation_Way.Animation;
namespace Cultivation_Way{
    public enum Load_Unit_Reason
    {
        CITY_SPAWN,
        LOAD_SAVES
    }
    public class ModState
    {
        public static ModState instance;
        public bool initialized = false;
        public bool addons_loaded_all = false;
        public bool registered = false;
        public NCMod mod_info;
        public CW_EffectManager effect_manager;
        public CW_Library_Manager library_manager;
        public Load_Unit_Reason load_unit_reason = Load_Unit_Reason.CITY_SPAWN;
    }
    [ModEntry]
    class Main : MonoBehaviour{
        public static Main instance { get; private set; }
        public ModState mod_state;
        public CW_WorldConfig world_config;
        private bool initialized = false;
        void Awake(){
            instance = this;
            print("[CW Core]: Awake");
        }
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
                CW_Library_Manager.create();
                mod_state = new ModState();
                world_config = new CW_WorldConfig();
                ModState.instance = mod_state;

                foreach (NCMod ncmod in NCMS.ModLoader.Mods)
                {
                    if (ncmod.name == Others.CW_Constants.mod_name) { mod_state.mod_info = ncmod; break; }
                }
                mod_state.effect_manager = this.gameObject.AddComponent<CW_EffectManager>();
                mod_state.library_manager = CW_Library_Manager.instance;
                mod_state.library_manager.init();
                Utils.CW_ItemTools.init();
                W_Content_Manager.add_content();

                mod_state.library_manager.register();
                print("[CW Core]: Finish Initialization");
            }
        }
    }
}