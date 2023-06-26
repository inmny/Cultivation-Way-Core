﻿using System.Collections.Generic;

namespace Cultivation_Way.Library;

public class Manager
{
    public static Manager instance;
    public static readonly CW_ActorAssetLibrary actors = new();
    public static readonly BloodNodeLibrary bloods = new();
    public static readonly CultibookLibrary cultibooks = new();
    public static readonly CultisysLibrary cultisys = new();
    public static readonly ElementLibrary elements = new();
    public static readonly EnergyLibrary energies = new();
    public static readonly CW_StatusEffectLibrary statuses = new();
    public static readonly CW_SpellLibrary spells = new();
    public static readonly Dictionary<string, BaseAssetLibrary> libraries = new();

    public void init()
    {
        instance = this;
        CW_BaseStatsLibrary.init();
        add(actors, "actors");
        add(bloods, "bloods");
        add(cultibooks, "cultibooks");
        add(cultisys, "cultisys");
        add(elements, "elements");
        add(energies, "energies");
        add(statuses, "statuses");
        add(spells, "spells");
    }

    public void update_per_while()
    {
        bloods.update();
        cultibooks.update();
    }

    public void post_init()
    {
        foreach (BaseAssetLibrary library in libraries.Values)
        {
            library.post_init();
        }
    }

    private void add(BaseAssetLibrary library, string name = "")
    {
        if (string.IsNullOrEmpty(name)) name = library.GetType().Name;
        library.init();
        library.id = name;
        libraries.Add(name, library);
    }
}