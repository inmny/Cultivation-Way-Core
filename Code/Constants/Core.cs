﻿namespace Cultivation_Way.Constants;

public static class Core
{
    public const string uniform_type = mod_prefix + "uniform";
    public const string mod_prefix = "cw_";
    public const string title_suffix = "_title";
    public const string desc_suffix = "_description";
    public const string new_title_suffix = " Title";
    public const string new_desc_suffix = " Description";
    public const string mod_name = "启源核心";
    public const int save_version = 2;

    public const string modinfo_window = mod_prefix + "modinfo_window";
    public const string modconfig_window = mod_prefix + "modconfig_window";
    public const string worldlaw_window = mod_prefix + "worldlaw_window";
    public const string tops_window = mod_prefix + "tops_window";
    public const string actorasset_config_window = mod_prefix + "actorasset_config_window";
    public const string cultisys_config_window = mod_prefix + "cultisys_config_window";
    public const string cultisys_level_config_window = mod_prefix + "cultisys_level_config_window";
    public const string cultibook_library_window = mod_prefix + "cultibook_library_window";
    public const string item_library_window = mod_prefix + "item_library_window";

    public const string power_energy_increase = mod_prefix + "energy_increase";
    public const string power_energy_decrease = mod_prefix + "energy_decrease";

    public const string energy_maps_toggle_name = mod_prefix + "energy_maps";

    public const int BASE_TYPE_WATER = 0;
    public const int BASE_TYPE_FIRE = 1;
    public const int BASE_TYPE_WOOD = 2;
    public const int BASE_TYPE_IRON = 3;
    public const int BASE_TYPE_GROUND = 4;
    public const int element_type_nr = 5;
    public const int item_level_count = 45;
    public const int item_level_per_stage = 9;
    public const int item_level_up_res_search_times = 3;

    public const int cultibook_level_count = 36;
    public const int cultibook_level_per_stage = 9;

    public static readonly string[] element_str = new string[element_type_nr]
    {
        mod_prefix + "water_element", mod_prefix + "fire_element", mod_prefix + "wood_element",
        mod_prefix + "iron_element", mod_prefix + "ground_element"
    };
}