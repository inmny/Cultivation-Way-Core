using Cultivation_Way.General.AboutUI;
using Cultivation_Way.Library;

namespace Cultivation_Way.Content;

internal static class Actors
{
    public static void init()
    {
        add_eastern_human();
        add_yaos();
    }

    private static void add_yaos()
    {
    }

    private static void add_eastern_human()
    {
        ActorAsset asset =
            AssetManager.actor_library.clone(
                Content_Constants.eastern_human_id, SA.unit_human
            );
        CW_ActorAsset cw_actor_asset = new(asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.immortal_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.bushido_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.soul_id);

        cw_actor_asset.culti_velo = 1;

        #region 原版设置

        asset.nameLocale = Content_Constants.eastern_human_name_locale;
        asset.race = Content_Constants.eastern_human_race;
        asset.icon = "iconEastern_Humans";
        asset.color = Toolbox.makeColor("#005E72");
        asset.body_separate_part_head = false;
        AssetManager.actor_library.t = asset;
        AssetManager.actor_library.addColorSet(S_SkinColor.human_default);
        AssetManager.actor_library.loadShadow(asset);

        #endregion

        FormatButtons.add_actor_button(asset.id);

        ActorAsset baby_asset =
            AssetManager.actor_library.clone(Content_Constants.eastern_human_id.Replace("unit", "baby"),
                Content_Constants.eastern_human_id);

        baby_asset.take_items = false;
        baby_asset.use_items = false;
        baby_asset.base_stats[S.speed] = 10f;
        baby_asset.can_turn_into_demon_in_age_of_chaos = false;
        baby_asset.years_to_grow_to_adult = 18;
        baby_asset.baby = true;
        baby_asset.growIntoID = Content_Constants.eastern_human_id;
        baby_asset.animation_idle = "walk_3";
        baby_asset.traits.Add("peaceful");
        AssetManager.actor_library.cloneColorSetFrom(Content_Constants.eastern_human_id);
        AssetManager.actor_library.loadShadow(baby_asset);
    }

    private static void add(CW_ActorAsset actor_asset)
    {
        Library.Manager.actors.add(actor_asset);
    }
}