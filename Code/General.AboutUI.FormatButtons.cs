using Cultivation_Way.UI;
using NCMS.Utils;

namespace Cultivation_Way.General.AboutUI;

public static class FormatButtons
{
    /// <summary>
    ///     添加生物到生物放置区域
    /// </summary>
    /// <param name="actor_id">需要放置的生物的id</param>
    /// <returns></returns>
    public static PowerButton add_actor_button(string actor_id)
    {
        ActorAsset actor_asset = AssetManager.actor_library.get(actor_id);
        if (actor_asset == null)
        {
            Logger.Warn($" Please add actor button after ActorAsset for {actor_id} created");
            return null;
        }

        GodPower god_power = AssetManager.powers.clone("spawn_" + actor_id, "_spawnActor");
        god_power.name = actor_asset.nameLocale;
        god_power.actor_asset_id = actor_id;
        god_power.click_action = AssetManager.powers.spawnUnit;
        PowerButton button = CWTab.create_button(
            god_power.id, "ui/Icons/" + actor_asset.icon,
            null, ButtonType.GodPower
        );
        CWTab.add_button(button);
        return button;
    }
}