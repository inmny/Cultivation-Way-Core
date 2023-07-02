using System.Collections.Generic;

namespace Cultivation_Way.Library;

public class CW_ActorAsset : Asset
{
    /// <summary>
    ///     允许的修炼体系
    /// </summary>
    internal List<CultisysAsset> allowed_cultisys = new();

    /// <summary>
    ///     反时间停止（非游戏暂停
    /// </summary>
    public bool anti_time_stop = false;

    /// <summary>
    ///     自带法术
    /// </summary>
    public List<string> born_spells;

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

    internal CW_ActorAsset(ActorAsset vanllia_asset)
    {
        id = vanllia_asset.id;
        this.vanllia_asset = vanllia_asset;
        prefer_element = new int[5] { 20, 20, 20, 20, 20 };
        prefer_element_scale = 0f;
        born_spells = new List<string>();
        culti_velo = 1;
    }

    internal readonly List<string> allowed_cultisys_ids = new();

    public void add_allowed_cultisys(string cultisys_id)
    {
        allowed_cultisys_ids.Add(cultisys_id);
    }

    internal readonly List<string> force_cultisys_ids = new();

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
            add(new CW_ActorAsset(vanllia_asset));
        }
    }

    public override CW_ActorAsset add(CW_ActorAsset pAsset)
    {
        if (pAsset.vanllia_asset != null && !AssetManager.actor_library.dict.ContainsKey(pAsset.vanllia_asset.id))
        {
            AssetManager.actor_library.add(pAsset.vanllia_asset);
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
}