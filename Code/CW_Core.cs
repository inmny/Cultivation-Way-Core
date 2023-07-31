using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cultivation_Way.Addon;
using Cultivation_Way.Animation;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.UI;
using ModDeclaration;
using NCMS;
using UnityEngine;

namespace Cultivation_Way;

[ModEntry]
public class CW_Core : MonoBehaviour
{
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
        addons = new List<CW_Addon>(),
        anim_manager = null,
        spell_manager = null,
        library_manager = null,
        map_chunk_manager = null
    };

    private void Awake()
    {
        instance = this;
        mod_state = state;
    }

    private int _last_year;

    private void Update()
    {
        if (!state.all_initialized)
        {
            if (!state.core_initialized)
            {
                // 初始化核心
                state.core_initialized = true;
                initialize();
                try_to_load_core_content();
            }
            else
            {
                // 等待附属初始化
                if (!state.addons_initialized)
                {
                    /* 检查附属是否初始化完全 */
                    /*一般加载流程为
                     * 核心Awake, 附属依次Awake, 将自身加入到核心的附属列表中
                     * 核心第一次Update
                     */
                    state.addons_initialized = true;
                    foreach (CW_Addon addon in state.addons.Where(addon => !addon.initialized))
                    {
                        state.addons_initialized = false;
                        break;
                    }
                }
                else
                {
                    // 在所有附属初始化完毕后, 进行后续处理
                    CWTab.post_init();
                    state.library_manager.post_init();
                    state.map_chunk_manager.init(World.world.tilesMap.GetLength(0),
                        World.world.tilesMap.GetLength(1));
                    Localizer.apply_localization(LocalizedTextManager.instance.localizedText,
                        LocalizedTextManager.instance.language);

                    state.all_initialized = true;
                }
            }

            return;
        }

        if (World.world.tilesMap != null && (World.world.tilesMap.GetLength(0) != state.map_chunk_manager.width
                                             || World.world.tilesMap.GetLength(1) != state.map_chunk_manager.height))
        {
            state.map_chunk_manager.reset(World.world.tilesMap.GetLength(0),
                World.world.tilesMap.GetLength(1));
        }

        int current_year = World.world.mapStats.getCurrentYear();
        if (current_year != _last_year)
        {
            update_year();
        }

        _last_year = current_year;

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

    /// <summary>
    ///     为了保证Content可拆卸, 通过反射调用Content的初始化方法
    /// </summary>
    private void try_to_load_core_content()
    {
        Type.GetType("Cultivation_Way.Content.Manager")
            ?.GetMethod("init", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            ?.Invoke(null, new object[] { });
    }

    private void initialize()
    {
        List<NCMod> mods = NCMS.ModLoader.Mods;
        foreach (NCMod mod in mods.Where(mod => mod.name == Constants.Core.mod_name))
        {
            state.mod_info =
                typeof(Info)
                    .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new[] { typeof(NCMod) },
                        null)
                    ?.Invoke(new object[] { mod }) as Info;
            break;
        }

        state.anim_manager = gameObject.AddComponent<EffectManager>();
        state.spell_manager = new SpellManager();
        state.library_manager = new Manager();
        state.map_chunk_manager = new CW_MapChunkManager();

        GameObject prefab_library_obj = new("CW_PrefabLibrary");
        prefab_library = prefab_library_obj.transform;
        prefab_library_obj.transform.SetParent(transform);

        configure();

        Factories.init();
        Localizer.init();
        Prefabs.init();
        FastVisit.init();
        HarmonySpace.Manager.init();
        state.library_manager.init();
        CWTab.init();
        init_windows();

        new Thread(() =>
        {
            while (true)
            {
                if (!state.map_chunk_manager.paused && mod_state.all_initialized)
                    state.map_chunk_manager.update_per_year();
                Thread.Sleep((int)(500 / Math.Min(Config.timeScale, 1)));
            }
        }).Start();
    }

    private void update_year()
    {
        //state.map_chunk_manager.update_per_year();
    }

    private void init_windows()
    {
        Type[] all_types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in all_types)
        {
            if (type.Namespace != $"{nameof(Cultivation_Way)}.{nameof(UI)}") continue;
            if (type.BaseType == null) continue;
            if (type.BaseType.Name.Contains("AbstractWindow"))
            {
                type.GetMethod("init", BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                    ?.Invoke(null, null);
            }
        }
    }

    private void configure()
    {
        //fastJSON.JSON.Parameters.UseExtensions = false;

        if (Environment.UserName != "Inmny") return;
        state.editor_inmny = true;
        Config.isEditor = true;
        Config.editor_maxim = true;
        Config.editor_mastef = true;
        Config.disableLocaleLogs = true;
    }

    public class ModState
    {
        internal List<CW_Addon> addons;
        public bool addons_initialized;
        public bool all_initialized;
        public EffectManager anim_manager;
        public bool core_initialized;
        internal bool editor_inmny;
        public Manager library_manager;
        public CW_MapChunkManager map_chunk_manager;
        internal Info mod_info;
        internal SpellManager spell_manager;
        internal long update_nr;
    }
}