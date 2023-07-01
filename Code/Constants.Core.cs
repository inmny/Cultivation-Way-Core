namespace Cultivation_Way.Constants;

public static class Core
{
    public const string uniform_type = mod_prefix + "uniform";
    public const string mod_prefix = "cw_";
    public const string title_suffix = "_title";
    public const string desc_suffix = "_description";
    public const string mod_name = "启源核心";
    public const int save_version = 2;

    public const string modinfo_window = mod_prefix + "modinfo_window";
    public const string tops_window = mod_prefix + "tops_window";

    public const string energy_maps_toggle_name = mod_prefix + "energy_maps";

    public const int BASE_TYPE_WATER = 0;
    public const int BASE_TYPE_FIRE = 1;
    public const int BASE_TYPE_WOOD = 2;
    public const int BASE_TYPE_IRON = 3;
    public const int BASE_TYPE_GROUND = 4;
    public const int element_type_nr = 5;

    public static readonly string[] element_str = new string[element_type_nr]
    {
        mod_prefix + "water_element", mod_prefix + "fire_element", mod_prefix + "wood_element",
        mod_prefix + "iron_element", mod_prefix + "ground_element"
    };
}