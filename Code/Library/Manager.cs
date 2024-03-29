﻿using System.Collections.Generic;
using Cultivation_Way.AI;

namespace Cultivation_Way.Library;

public class Manager
{
    public static          Manager                              instance;
    public static readonly CW_ActorAssetLibrary                 actors         = new();
    public static readonly BloodNodeLibrary                     bloods         = new();
    public static readonly CultibookLibrary                     cultibooks     = new();
    public static readonly CultisysLibrary                      cultisys       = new();
    public static readonly ElementLibrary                       elements       = new();
    public static readonly ElixirLibrary                        elixirs        = new();
    public static readonly EnergyLibrary                        energies       = new();
    public static readonly CW_ItemLibrary                       items          = new();
    public static readonly CW_ItemMaterialLibrary               item_materials = new();
    public static readonly CW_ResourceLibrary                   resources      = new();
    public static readonly CW_StatusEffectLibrary               statuses       = new();
    public static readonly CW_SpellLibrary                      spells         = new();
    public static readonly Dictionary<string, BaseAssetLibrary> libraries      = new();

    public void init()
    {
        instance = this;
        CW_ActorJobLibrary.init();
        CW_ActorTraits.init();
        CW_BaseStatsLibrary.init();
        CW_BehaviourTaskActorLibrary.init();
        CW_BehaviourTaskCityLibrary.init();
        CW_CitizenJobLibrary.init();
        CW_CultureTechLibrary.init();
        CW_DebugTools.init();
        CW_GodPowers.init();
        CW_MapIconLibrary.init();
        CW_NameGenerators.init();
        CW_Drops.init();
        CW_Options.init();
        CW_TooltipAssets.init();
        add(actors,         "actors");
        add(bloods,         "bloods");
        add(cultibooks,     "cultibooks");
        add(cultisys,       "cultisys");
        add(elements,       "elements");
        add(elixirs,        "elixirs");
        add(energies,       "energies");
        add(items,          "items");
        add(item_materials, "item_materials");
        add(resources,      "resources");
        add(statuses,       "statuses");
        add(spells,         "spells");
    }

    public void update_per_while()
    {
        bloods.update();
        cultibooks.update();
    }

    public void post_init()
    {
        CW_Options.post_init();
        CW_GodPowers.post_init();
        foreach (BaseAssetLibrary library in libraries.Values)
        {
            library.post_init();
        }
    }

    private void add(BaseAssetLibrary library, string name = "")
    {
        if (string.IsNullOrEmpty(name)) name = library.GetType().Name;
        name = Constants.Core.mod_prefix + name;
        AssetManager.instance.add(library, name);
        libraries.Add(name, library);
    }
}