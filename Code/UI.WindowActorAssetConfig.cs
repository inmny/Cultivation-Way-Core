namespace Cultivation_Way.UI;

public class WindowActorAssetConfig : AbstractWindow<WindowActorAssetConfig>
{
    internal static void init()
    {
        base_init(Constants.Core.actorasset_config_window);
    }

    internal static void post_init()
    {
        General.AboutUI.WorldLaws.add_setting_law(Constants.Core.actorasset_config_window, "worldlaw_creature_grid",
            "ui/icons/iconYaos", Constants.Core.actorasset_config_window);
    }
}