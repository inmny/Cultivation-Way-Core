using System.Collections.Generic;
using Cultivation_Way.Constants;
using Cultivation_Way.UI;
using NCMS.Utils;

namespace Cultivation_Way.General.AboutUI;

public static class FormatButtons
{
    /// <summary>
    ///     添加生物到生物放置区域
    /// </summary>
    /// <param name="actor_id">需要放置的生物的id</param>
    /// <param name="pTargetType">按钮类型，建议: <see cref="ButtonContainerType.ACTOR"/>或<see cref="ButtonContainerType.RACE"/>或<see cref="ButtonContainerType.BOSS"/></param>
    /// <returns></returns>
    public static PowerButton add_actor_button(string actor_id,
        ButtonContainerType pTargetType = ButtonContainerType.ACTOR)
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
        CWTab.add_button(button, pTargetType);
        return button;
    }

    /// <summary>
    ///     添加一系列生物作为一个按钮到生物放置区域
    /// </summary>
    /// <param name="actor_ids">需要放置的生物的id</param>
    /// <param name="name_locale">按钮名的key</param>
    /// <param name="icon">图标名, 具体访问路径为$"ui/Icons/{icon}"</param>
    /// <param name="pTargetType">按钮类型，建议: <see cref="ButtonContainerType.ACTOR"/>或<see cref="ButtonContainerType.RACE"/>或<see cref="ButtonContainerType.BOSS"/></param>
    /// <returns></returns>
    public static PowerButton add_actors_button(List<string> actor_ids, string name_locale, string icon,
        ButtonContainerType pTargetType = ButtonContainerType.ACTOR)
    {
        foreach (string actor_id in actor_ids)
        {
            if (AssetManager.actor_library.get(actor_id) == null)
            {
                Logger.Warn($" Please add actor button after ActorAsset for {actor_id} created");
                return null;
            }
        }

        GodPower god_power = AssetManager.powers.clone("spawn_" + name_locale, "_spawnActor");
        god_power.name = name_locale;
        god_power.actor_asset_ids = new List<string>();
        god_power.actor_asset_ids.AddRange(actor_ids);
        god_power.click_action = AssetManager.powers.spawnUnit;
        PowerButton button = CWTab.create_button(
            god_power.id, "ui/Icons/" + icon,
            null, ButtonType.GodPower
        );
        CWTab.add_button(button, pTargetType);
        return button;
    }
}