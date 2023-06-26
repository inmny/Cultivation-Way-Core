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
}