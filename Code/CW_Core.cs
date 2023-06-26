using Cultivation_Way.Core;
using NCMS;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Cultivation_Way.Animation;

namespace Cultivation_Way
{
    [ModEntry]
    public class CW_Core : MonoBehaviour
    {
        public class ModState
        {
            public bool core_initialized;
            public bool addons_initialized;
            public bool all_initialized;
            internal bool editor_inmny;
            internal long update_nr;
            internal ModDeclaration.Info mod_info;
            internal List<Addon.CW_Addon> addons;
            internal SpellManager spell_manager;
            public EffectManager anim_manager;
            public Library.Manager library_manager;
            public CW_MapChunkManager map_chunk_manager;
        };
        public static CW_Core instance;
        public static ModState mod_state;
        public static Transform prefab_library;

        public ModState state = new()
        {
            core_initialized = false,
            addons_initialized = false,
            all_initialized = false,
            editor_inmny = false,
            update_nr = 0,
            mod_info = null,
            addons = new(),
            anim_manager = null,
            spell_manager = null,
            library_manager = null,
            map_chunk_manager = null
        };
        static void Main()
        {

        }
        void Awake()
        {
            instance = this;
            mod_state = state;
        }
        void Update()
        {
            if (!state.all_initialized)
            {
                if (!state.core_initialized)
                { // 初始化核心
                    state.core_initialized = true;
                    this.initialize();
                    Content.Manager.init();
                }
                else
                { // 等待附属初始化
                    if (!state.addons_initialized)
                    {
                        /* 检查附属是否初始化完全 */
                        /**一般加载流程为
                         * 核心Awake, 附属依次Awake, 将自身加入到核心的附属列表中
                         * 核心第一次Update
                         */
                        state.addons_initialized = true;
                        foreach (Addon.CW_Addon addon in state.addons)
                        {
                            if (!addon.initialized)
                            {
                                state.addons_initialized = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 在所有附属初始化完毕后, 进行后续处理
                        state.library_manager.post_init();
                        state.map_chunk_manager.init(World.world.mapChunkManager.amountX, World.world.mapChunkManager.amountY);
                        
                        Localizer.apply_localization(LocalizedTextManager.instance.localizedText, LocalizedTextManager.instance.language);

                        state.all_initialized = true;
                    }
                }

                return;
            }

            state.update_nr++;
            state.spell_manager.deal_all();
            if (state.update_nr % 256 == 0)
            {
                Factories.recycle_items();
                state.spell_manager.update_per_while();
                state.library_manager.update_per_while();
                if (state.update_nr % (256 * 1024) == 0)
                {
                    Factories.recycle_memory();
                }
            }
        }
        void initialize()
        {
            List<NCMod> mods = NCMS.ModLoader.Mods;
            foreach(NCMod mod in mods)
            {
                if(mod.name == Constants.Core.mod_name)
                {
                    state.mod_info = typeof(ModDeclaration.Info).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(NCMod) }, null).Invoke(new object[] { mod }) as ModDeclaration.Info;
                    break;
                }
            }
            state.anim_manager = gameObject.AddComponent<EffectManager>();
            state.spell_manager = new();
            state.library_manager = new();
            state.map_chunk_manager = new();

            GameObject prefab_library_obj = new("CW_PrefabLibrary");
            prefab_library = prefab_library_obj.transform;
            prefab_library_obj.transform.SetParent(this.transform);

            configure();

            Factories.init();
            Localizer.init();
            UI.Prefabs.init();
            Others.FastVisit.init();
            HarmonySpace.Manager.init();
            state.library_manager.init();
        }

        void configure()
        {
            //fastJSON.JSON.Parameters.UseExtensions = false;

            if(Environment.UserName == "Inmny")
            {
                state.editor_inmny = true;
                Config.isEditor = true;
                Config.editor_maxim = true;
                Config.editor_mastef = true;
                Config.disableLocaleLogs = true;
            }
        }
    }
}
