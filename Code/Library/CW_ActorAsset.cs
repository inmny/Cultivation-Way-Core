using System.Collections.Generic;
using Cultivation_Way.Constants;
using NeoModLoader.api.attributes;
using NeoModLoader.General;

namespace Cultivation_Way.Library;

public class CW_ActorAsset : Asset
{
    /// <summary>
    ///     普攻是否额外造成元神伤害
    /// </summary>
    public bool addition_soul_damage = false;

    /// <summary>
    ///     允许的修炼体系
    /// </summary>
    internal List<CultisysAsset> allowed_cultisys = new();

    internal List<string> allowed_cultisys_ids = new();

    /// <summary>
    ///     反时间停止（非游戏暂停
    /// </summary>
    public bool anti_time_stop = false;

    /// <summary>
    ///     自带法术
    /// </summary>
    public List<string> born_spells = new();

    /// <summary>
    ///     此类生物修炼速度
    /// </summary>
    public float culti_velo;

    /// <summary>
    ///     预设名
    /// </summary>
    public string fixed_name = null;

    /// <summary>
    ///     强制的修炼体系，或关系
    /// </summary>
    internal List<CultisysAsset> force_cultisys = new();

    internal List<string> force_cultisys_ids = new();

    /// <summary>
    ///     偏好元素
    /// </summary>
    public int[] prefer_element;

    /// <summary>
    ///     元素偏好系数
    /// </summary>
    public float prefer_element_scale;

    /// <summary>
    ///     原版的生物信息
    /// </summary>
    public ActorAsset vanllia_asset;

    public CW_ActorAsset(ActorAsset vanllia_asset)
    {
        id = vanllia_asset.id;
        this.vanllia_asset = vanllia_asset;
        prefer_element = new int[5] { 20, 20, 20, 20, 20 };
        prefer_element_scale = 0f;
        born_spells = new List<string>();
        culti_velo = 1;
    }

    public void add_allowed_cultisys(string cultisys_id)
    {
        allowed_cultisys_ids.Add(cultisys_id);
    }

    public void add_force_cultisys(string cultisys_id)
    {
        force_cultisys_ids.Add(cultisys_id);
    }
}

public class CW_ActorAssetLibrary : CW_Library<CW_ActorAsset>
{
    internal List<string> added_actors = new();

    public override void init()
    {
        base.init();
        foreach (ActorAsset vanllia_asset in AssetManager.actor_library.list)
        {
            var asset = add(new CW_ActorAsset(vanllia_asset));
            asset.vanllia_asset.base_stats[CW_S.soul] = 10;
        }
    }

    /// <summary>
    ///     一般clone外添加了对vanilla_asset的clone
    /// </summary>
    public override CW_ActorAsset clone(string pNew, string pFrom)
    {
        CW_ActorAsset from_asset = get(pFrom);
        if (from_asset == null)
        {
            Logger.Warn($"CW_ActorAsset {pFrom} not found!");
            return null;
        }

        CW_ActorAsset new_asset = base.clone(pNew, pFrom);

        if (from_asset.vanllia_asset == null)
        {
            Logger.Warn($"CW_ActorAsset {pFrom} has no vanilla asset!");
            new_asset.vanllia_asset.id = pNew;
            AssetManager.actor_library.add(new_asset.vanllia_asset);
        }
        else
        {
            new_asset.vanllia_asset = AssetManager.actor_library.clone(pNew, pFrom);
        }

        // 由于访问权限问题, 需要手动拷贝
        new_asset.allowed_cultisys_ids = new List<string>();
        new_asset.force_cultisys_ids = new List<string>();
        new_asset.allowed_cultisys_ids.AddRange(from_asset.allowed_cultisys_ids);
        new_asset.force_cultisys_ids.AddRange(from_asset.force_cultisys_ids);
        // 由于clone是采用JSON实现的, 默认值会为null, 此时需要手动初始化
        new_asset.born_spells ??= new List<string>();
        new_asset.force_cultisys ??= new List<CultisysAsset>();
        new_asset.allowed_cultisys ??= new List<CultisysAsset>();

        return new_asset;
    }

    public override CW_ActorAsset add(CW_ActorAsset pAsset)
    {
        if (pAsset.vanllia_asset != null && !AssetManager.actor_library.dict.ContainsKey(pAsset.vanllia_asset.id))
        {
            AssetManager.actor_library.add(pAsset.vanllia_asset);
        }

        if (pAsset.vanllia_asset != null && (pAsset.vanllia_asset.traits == null ||
                                             (!pAsset.vanllia_asset.traits.Contains(Constants.Core.mod_prefix +
                                                  "positive_creature")
                                              && !pAsset.vanllia_asset.traits.Contains(Constants.Core.mod_prefix +
                                                  "negative_creature"))))
        {
            pAsset.vanllia_asset.traits ??= new List<string>();
            if (pAsset.vanllia_asset.race == SK.undead || pAsset.vanllia_asset.kingdom == SK.undead)
            {
                pAsset.vanllia_asset.traits.Add(Constants.Core.mod_prefix + "negative_creature");
            }
            else
            {
                pAsset.vanllia_asset.traits.Add(Constants.Core.mod_prefix + "positive_creature");
            }
        }

        return base.add(pAsset);
    }

    /// <summary>
    ///     除正常get外, 将其他mod添加的生物以默认方式加入
    /// </summary>
    public override CW_ActorAsset get(string pID)
    {
        if (!dict.ContainsKey(pID) && AssetManager.actor_library.dict.ContainsKey(pID))
        {
            add(new CW_ActorAsset(AssetManager.actor_library.get(pID)));
        }

        return base.get(pID);
    }

    /// <summary>
    ///     进行安全检查, 保证所有CW_ActorAsset都有对应的ActorAsset
    /// </summary>
    public override void post_init()
    {
        base.post_init();
        foreach (CW_ActorAsset cw_actor_asset in list)
        {
            if (cw_actor_asset.vanllia_asset == null)
            {
                Logger.Warn($"CW_ActorAsset {cw_actor_asset.id} has no vanilla asset!");
            }

            if (cw_actor_asset.vanllia_asset != null
                && !AssetManager.actor_library.dict.ContainsKey(cw_actor_asset.vanllia_asset.id))
            {
                AssetManager.actor_library.add(cw_actor_asset.vanllia_asset);
            }

            foreach (string allowed_cultisys_id in cw_actor_asset.allowed_cultisys_ids)
            {
                if (Manager.cultisys.get(allowed_cultisys_id) == null)
                {
                    Logger.Warn(
                        $"CW_ActorAsset {cw_actor_asset.id} has invalid allowed_cultisys {allowed_cultisys_id}!");
                    continue;
                }

                cw_actor_asset.allowed_cultisys.Add(
                    Manager.cultisys.get(allowed_cultisys_id)
                );
            }

            foreach (string force_cultisys_id in cw_actor_asset.force_cultisys_ids)
            {
                if (Manager.cultisys.get(force_cultisys_id) == null)
                {
                    Logger.Warn($"CW_ActorAsset {cw_actor_asset.id} has invalid force_cultisys {force_cultisys_id}!");
                    continue;
                }

                cw_actor_asset.force_cultisys.Add(
                    Manager.cultisys.get(force_cultisys_id)
                );
            }
        }
    }

    [Hotfixable]
    public HashSet<CW_ActorAsset> SearchByID(string pID)
    {
        string id = pID.ToLower();
        HashSet<CW_ActorAsset> res = new();
        foreach (var asset in AssetManager.actor_library.list)
        {
            if (asset.id.StartsWith("_")) continue;
            if (asset.id.ToLower().Contains(id))
            {
                res.Add(get(asset.id));
            }
        }

        return res;
    }

    [Hotfixable]
    public HashSet<CW_ActorAsset> SearchByName(string pName)
    {
        string name = pName.ToLower();
        HashSet<CW_ActorAsset> res = new();
        foreach (var asset in AssetManager.actor_library.list)
        {
            if (asset.id.StartsWith("_")) continue;
            if (LocalizedTextManager.stringExists(asset.nameLocale) &&
                LM.Get(asset.nameLocale).ToLower().Contains(name))
            {
                res.Add(get(asset.id));
            }
        }

        return res;
    }

    [Hotfixable]
    public HashSet<CW_ActorAsset> SearchByRaceID(string pRaceID)
    {
        string id = pRaceID.ToLower();
        HashSet<CW_ActorAsset> res = new();
        foreach (var asset in AssetManager.actor_library.list)
        {
            if (asset.id.StartsWith("_")) continue;
            if ((!string.IsNullOrEmpty(asset.race)) && asset.race.ToLower().Contains(id))
            {
                res.Add(get(asset.id));
            }
        }

        return res;
    }

    [Hotfixable]
    public HashSet<CW_ActorAsset> SearchByRaceName(string pRaceName)
    {
        string id = pRaceName.ToLower();
        HashSet<CW_ActorAsset> res = new();
        foreach (var asset in AssetManager.actor_library.list)
        {
            if (asset.id.StartsWith("_")) continue;
            if (string.IsNullOrEmpty(asset.race)) continue;
            if (!AssetManager.raceLibrary.dict.TryGetValue(asset.race, out Race race)) continue;
            if (string.IsNullOrEmpty(race.nameLocale) || !LocalizedTextManager.stringExists(race.nameLocale)) continue;
            if (LM.Get(race.nameLocale).ToLower().Contains(id))
            {
                res.Add(get(asset.id));
            }
        }

        return res;
    }
}