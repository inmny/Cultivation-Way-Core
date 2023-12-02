using System;
using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutUI;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal static class Actors
{
    public static readonly List<string> yao_ids = new();

    public static void init()
    {
        add_eastern_human();
        add_yaos();
        add_ming();
        add_wu();
    }

    private static void add_wu()
    {
        ActorAsset asset =
            AssetManager.actor_library.clone(
                Content_Constants.wu_id, SA.unit_human
            );
        CW_ActorAsset cw_actor_asset = new(asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.bushido_id);

        cw_actor_asset.culti_velo = 1;

        #region 原版设置

#if 一米_中文名
        asset.nameTemplate = "wu_name";
#endif

        asset.base_stats[CW_S.soul] = 10;
        asset.nameLocale = Content_Constants.wu_name_locale;
        asset.race = Content_Constants.wu_race;
        asset.icon = "iconWus";
        asset.color = Toolbox.makeColor("#005E72");
        asset.body_separate_part_head = true;
        asset.heads = 6;
        asset.useSkinColors = true;
        AssetManager.actor_library.t = asset;
        AssetManager.actor_library.addColorSet(S_SkinColor.human_default);
        AssetManager.actor_library.loadShadow(asset);

        #endregion

        FormatButtons.add_actor_button(asset.id);

        ActorAsset baby_asset =
            AssetManager.actor_library.clone(Content_Constants.wu_id.Replace("unit", "baby"),
                Content_Constants.wu_id);

        baby_asset.take_items = false;
        baby_asset.use_items = false;
        baby_asset.base_stats[S.speed] = 10f;
        baby_asset.can_turn_into_demon_in_age_of_chaos = false;
        baby_asset.years_to_grow_to_adult = 18;
        baby_asset.baby = true;
        baby_asset.growIntoID = Content_Constants.wu_id;
        baby_asset.animation_idle = "walk_3";
        baby_asset.traits.Add("peaceful");

        cw_actor_asset = new CW_ActorAsset(baby_asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.bushido_id);
        AssetManager.actor_library.cloneColorSetFrom(Content_Constants.wu_id);
        AssetManager.actor_library.loadShadow(baby_asset);
    }

    private static void add_ming()
    {
        ActorAsset asset =
            AssetManager.actor_library.clone(
                Content_Constants.ming_id, SA.unit_human
            );
        CW_ActorAsset cw_actor_asset = new(asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.immortal_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.soul_id);

        cw_actor_asset.culti_velo = 1;

        #region 原版设置

#if 一米_中文名
        asset.nameTemplate = "ming_name";
#endif

        asset.base_stats[CW_S.soul] = 50;
        asset.base_stats[CW_S.mod_soul] = 0.5f;
        asset.nameLocale = Content_Constants.ming_name_locale;
        asset.race = Content_Constants.ming_race;
        asset.icon = "iconMings";
        asset.useSkinColors = false;
        asset.base_stats[S.armor] = 1e5f;
        asset.color = Toolbox.makeColor("#005E72");
        asset.body_separate_part_head = false;
        asset.traits = new List<string>
        {
            Constants.Core.mod_prefix + "negative_creature",
            Constants.Core.mod_prefix + "curse_immune"
        };
        AssetManager.actor_library.t = asset;
        AssetManager.actor_library.addColorSet(S_SkinColor.human_default);
        AssetManager.actor_library.loadShadow(asset);

        #endregion

        FormatButtons.add_actor_button(asset.id);

        ActorAsset baby_asset =
            AssetManager.actor_library.clone(Content_Constants.ming_id.Replace("unit", "baby"),
                Content_Constants.ming_id);

        baby_asset.take_items = false;
        baby_asset.use_items = false;
        baby_asset.base_stats[S.speed] = 10f;
        baby_asset.base_stats[S.armor] = 1e4f;
        baby_asset.traits = new List<string>
        {
            Constants.Core.mod_prefix + "negative_creature",
            Constants.Core.mod_prefix + "curse_immune"
        };
        baby_asset.can_turn_into_demon_in_age_of_chaos = false;
        baby_asset.years_to_grow_to_adult = 18;
        baby_asset.baby = true;
        baby_asset.growIntoID = Content_Constants.ming_id;
        baby_asset.animation_idle = "walk_3";
        baby_asset.traits.Add("peaceful");

        cw_actor_asset = new CW_ActorAsset(baby_asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.immortal_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.soul_id);
        AssetManager.actor_library.cloneColorSetFrom(Content_Constants.ming_id);
        AssetManager.actor_library.loadShadow(baby_asset);
    }

    private static void add_yaos()
    {
        ActorAsset _yao_asset =
            AssetManager.actor_library.clone(
                Content_Constants.yao_postfix, SA.unit_human
            );
        CW_ActorAsset _cw_yao_asset = new(_yao_asset);
        add(_cw_yao_asset);
        _cw_yao_asset.add_allowed_cultisys(Content_Constants.immortal_id);
        _cw_yao_asset.add_allowed_cultisys(Content_Constants.bushido_id);
        _cw_yao_asset.culti_velo = 0.85f;
        _cw_yao_asset.born_spells.Add("brutalize");

        #region 原版设置

        _yao_asset.setBaseStats(300, 30, 50, 20, 5, 80);
        _yao_asset.base_stats[CW_S.mod_health_regen] = 0.2f;
        _yao_asset.base_stats[S.mod_armor] = 0.4f;
        _yao_asset.base_stats[S.mod_health] = 0.5f;
        _yao_asset.base_stats[S.max_age] = 300;

        _yao_asset.race = Content_Constants.yao_race;
        _yao_asset.icon = "iconYaos";
        _yao_asset.nameLocale = Content_Constants.yao_name_locale;
        _yao_asset.color = Toolbox.makeColor("#005E72");
        _yao_asset.body_separate_part_head = false;
        if (Environment.UserName == "94508")
        {
            _yao_asset.nameTemplate = "easternhuman_name";
        }

        AssetManager.actor_library.t = _yao_asset;
        AssetManager.actor_library.addColorSet(S_SkinColor.human_default);
        AssetManager.actor_library.loadShadow(_yao_asset);

        #endregion

        #region 加入具体妖族单位

        add_yao("bear"); // 熊
        add_yao("buffalo"); // 野牛
        add_yao("cat"); // 猫
        add_yao("chicken"); // 鸡
        add_yao("cow"); // 牛
        //add_yao("crab");      // 螃蟹
        //add_yao("crocodile");   // 鳄鱼
        //add_yao("deer");        // 鹿
        add_yao("dog"); // 狗
        add_yao("fox"); // 狐狸
        add_yao("frog"); // 青蛙
        //add_yao("horse");       // 马
        add_yao("hyena"); // 鬣狗
        //add_yao("lion");        // 狮
        add_yao("monkey"); // 猴
        add_yao("penguin"); // 企鹅
        //add_yao("pig");         // 猪
        add_yao("rabbit"); // 兔
        add_yao("rat"); // 鼠
        add_yao("ratKing"); // 鼠王
        add_yao("rhino"); // 犀牛
        //add_yao("rooster");     // 公鸡
        add_yao("sheep"); // 羊
        add_yao("snake"); // 蛇
        //add_yao("tiger");       // 虎
        //add_yao("turtle");      // 龟
        //add_yao("wild_boar");   // 野猪
        add_yao("wolf"); // 狼

        FormatButtons.add_actors_button(
            yao_ids.Select(yao_id => yao_id + Content_Constants.yao_postfix).ToList(),
            _yao_asset.nameLocale, _yao_asset.icon
        );

        #endregion
    }

    private static CW_ActorAsset add_yao(string id)
    {
        CW_ActorAsset cw_yao_asset =
            Library.Manager.actors.clone(id + Content_Constants.yao_postfix, Content_Constants.yao_postfix);

        cw_yao_asset.vanllia_asset.base_stats[CW_S.soul] = 10;
        cw_yao_asset.vanllia_asset.texture_path = $"yaos/t_{cw_yao_asset.id}";
        cw_yao_asset.vanllia_asset.icon = AssetManager.actor_library.dict.ContainsKey(id)
            ? AssetManager.actor_library.get(id).icon
            : cw_yao_asset.vanllia_asset.icon;
        cw_yao_asset.vanllia_asset.nameLocale = AssetManager.actor_library.dict.ContainsKey(id)
            ? AssetManager.actor_library.get(id).nameLocale
            : cw_yao_asset.vanllia_asset.nameLocale;

#if 一米_中文名
        cw_yao_asset.vanllia_asset.nameTemplate = "yao_name";
#endif
        AssetManager.actor_library.t = cw_yao_asset.vanllia_asset;
        AssetManager.actor_library.addColorSet(S_SkinColor.human_default);
        AssetManager.actor_library.loadShadow(cw_yao_asset.vanllia_asset);

        yao_ids.Add(id);
        return cw_yao_asset;
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

        #if 一米_中文名
            asset.nameTemplate = "eastern_human_name";
        #endif

        asset.base_stats[CW_S.soul] = 10;
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

        cw_actor_asset = new CW_ActorAsset(baby_asset);
        add(cw_actor_asset);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.immortal_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.bushido_id);
        cw_actor_asset.add_allowed_cultisys(Content_Constants.soul_id);
        AssetManager.actor_library.cloneColorSetFrom(Content_Constants.eastern_human_id);
        AssetManager.actor_library.loadShadow(baby_asset);
    }

    private static void add(CW_ActorAsset actor_asset)
    {
        Library.Manager.actors.add(actor_asset);
    }
}