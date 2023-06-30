namespace Cultivation_Way.Library;

internal static class CW_Options
{
    public static void init()
    {
        // Energy Map Toggle
        OptionAsset energy_maps_toggle = new();
        energy_maps_toggle.id = Constants.Core.energy_maps_toggle_name;
        energy_maps_toggle.default_bool = false;
        energy_maps_toggle.type = OptionType.Bool;
        add(energy_maps_toggle);
    }

    private static void add(OptionAsset option_asset)
    {
        AssetManager.options_library.add(option_asset);
    }
}