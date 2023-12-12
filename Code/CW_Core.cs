using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cultivation_Way.Addon;
using Cultivation_Way.Animation;
using Cultivation_Way.Core;
using Cultivation_Way.Implementation;
using Cultivation_Way.Others;
using Cultivation_Way.UI;
using ModDeclaration;
using NeoModLoader.api;
using NeoModLoader.api.attributes;
using UnityEngine;
using Manager = Cultivation_Way.Library.Manager;

namespace Cultivation_Way;

public class CW_Core : BasicMod<CW_Core>, IReloadable
{
    public static ModState mod_state;
    public static Transform ui_prefab_library;
    public static Transform actor_prefab_library;
    public static Transform anim_prefab_library;

    private int _last_year;

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
        energy_map_manager = null
    };

    private void Update()
    {
        if (!state.all_initialized)
        {
            if (!state.core_initialized) return;
            // 等待附属初始化
            if (!state.addons_initialized)
            {
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
                action_on_windows("post_init");
                state.library_manager.post_init();
                state.energy_map_manager.init(256, 256);

                state.all_initialized = true;
            }

            return;
        }

        if (World.world.tilesMap != null && (World.world.tilesMap.GetLength(0) != state.energy_map_manager.width
                                             || World.world.tilesMap.GetLength(1) != state.energy_map_manager.height))
        {
            state.energy_map_manager.reset(World.world.tilesMap.GetLength(0),
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

    [Hotfixable]
    public void Reload()
    {
        LogInfo("Reloaded");
        Manager.item_materials.init();
        Items.init();

        foreach (Actor actor in World.world.units)
        {
            if (!actor.isAlive()) continue;
            actor.setStatsDirty();
        }
    }

    /// <summary>
    ///     为了保证Implementation可拆卸, 通过反射调用Implementation的初始化方法
    /// </summary>
    private void try_to_load_core_content()
    {
        Type.GetType("Cultivation_Way.Implementation.Manager")
            ?.GetMethod("init", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            ?.Invoke(null, new object[] { });
    }

    private void initialize()
    {
        state.anim_manager = gameObject.AddComponent<EffectManager>();
        state.spell_manager = new SpellManager();
        state.library_manager = new Manager();
        state.energy_map_manager = new CW_EnergyMapManager();

        state.anim_manager.Init();

        GameObject ui_prefab_library_obj = new("UI_PrefabLibrary");
        GameObject actor_prefab_library_obj = new("Actor_PrefabLibrary");
        GameObject anim_prefab_library_obj = new("Animation_PrefabLibrary");
        ui_prefab_library = ui_prefab_library_obj.transform;
        actor_prefab_library = actor_prefab_library_obj.transform;
        anim_prefab_library = anim_prefab_library_obj.transform;
        ui_prefab_library_obj.transform.SetParent(transform);
        actor_prefab_library_obj.transform.SetParent(transform);
        anim_prefab_library_obj.transform.SetParent(transform);

        configure();

        Factories.init();
        Fonts.init();
        Prefabs.init();
        FastVisit.init();
        HarmonySpace.Manager.init();
        state.library_manager.init();
        CWTab.init();
        action_on_windows("init");

        new Thread(() =>
        {
            while (true)
            {
                if (!World.world.isPaused() && mod_state.all_initialized)
                    state.energy_map_manager.update_per_year();
                Thread.Sleep((int)(500 / Math.Max(Config.timeScale, 1)));
            }
        }).Start();
    }

    private void update_year()
    {
        //state.map_chunk_manager.update_per_year();
    }

    private void action_on_windows(string action_name)
    {
        Type[] all_types = Assembly.GetExecutingAssembly().GetTypes();
        foreach (Type type in all_types)
        {
            if (type.Namespace != $"{nameof(Cultivation_Way)}.{nameof(UI)}") continue;
            if (type.BaseType == null) continue;
            if (type.BaseType.Name.Contains("AbstractWindow"))
            {
                type.GetMethod(action_name, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
                    ?.Invoke(null, null);
            }
        }
    }

    private void configure()
    {
        //fastJSON.JSON.Parameters.UseExtensions = false;

        if (Environment.UserName != "Inmny") return;
        state.editor_inmny = true;
    }

    protected override void OnModLoad()
    {
        // 初始化核心
        mod_state = state;
        initialize();
        state.core_initialized = true;
        try_to_load_core_content();
    }

    [Hotfixable]
    private void new_reload_method()
    {
        LogInfo("new reload method: hello world!");
    }

    public class ModState
    {
        internal List<CW_Addon> addons;
        public bool addons_initialized;
        public bool all_initialized;
        public EffectManager anim_manager;
        public bool core_initialized;
        internal bool editor_inmny;
        public CW_EnergyMapManager energy_map_manager;
        public Manager library_manager;
        internal Info mod_info;
        internal SpellManager spell_manager;
        internal long update_nr;
    }
}