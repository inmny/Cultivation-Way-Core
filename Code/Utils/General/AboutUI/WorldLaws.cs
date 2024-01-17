using System;
using System.Collections.Generic;
using Cultivation_Way.UI;

namespace Cultivation_Way.Utils.General.AboutUI;

public static class WorldLaws
{
    internal static Dictionary<string, bool> switch_laws = new();


    public static void add_switch_law(string id, string grid_id, string icon_path, bool default_value)
    {
        switch_laws.Add(id, default_value);
        WindowWorldLaw.instance.add_switch_law_button(id, grid_id, icon_path);
    }

    public static void add_setting_law(string id, string grid_id, string icon_path, string window_id,
        Action addition_action = null)
    {
        WindowWorldLaw.instance.add_setting_law_button(id, grid_id, icon_path, window_id, addition_action);
    }
}