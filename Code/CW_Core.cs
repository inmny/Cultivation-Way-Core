using Cultivation_Way.Core;
using NCMS;
using UnityEngine;

namespace Cultivation_Way
{
    [ModEntry]
    public class CW_Core : MonoBehaviour
    {
        public struct ModState
        {
            public bool core_initialized;
            public bool addons_initialized;
            public bool all_initialized;
            internal long update_nr;
            public Library.Manager library_manager;
            public CW_MapChunkManager map_chunk_manager;
        };
        public static CW_Core instance;

        public ModState state = new()
        {
            core_initialized = false,
            addons_initialized = false,
            all_initialized = false,
            update_nr = 0,
            library_manager = null,
            map_chunk_manager = null
        };

        void Awake()
        {
            instance = this;
        }
        void Update()
        {
            if (!state.all_initialized)
            {
                if (!state.core_initialized)
                { // 初始化核心
                    state.core_initialized = true;
                    this.initialize();
                }
                else
                { // 等待附属初始化
                    if (!state.addons_initialized)
                    {
                        /* 检查附属是否初始化完全 */
                        state.addons_initialized = true;
                    }
                    else
                    {
                        // 在所有附属初始化完毕后, 进行后续处理
                        state.library_manager.post_init();
                        state.map_chunk_manager.init(World.world.mapChunkManager.amountX, World.world.mapChunkManager.amountY);


                        state.all_initialized = true;
                    }
                }

                return;
            }

            state.update_nr++;
            if (state.update_nr % 4 == 0)
            {
                Factories.recycle_items();
                if (state.update_nr % 1024 == 0)
                {
                    Factories.recycle_memory();
                    state.library_manager.update_per_while();
                }
            }
        }
        void initialize()
        {
            state.library_manager = new();
            state.map_chunk_manager = new();

            configure();
            Factories.init();
            Others.FastVisit.init();
            HarmonySpace.Manager.init();
            state.library_manager.init();
        }

        void configure()
        {
            fastJSON.JSON.Parameters.UseExtensions = false;
        }
    }
}
