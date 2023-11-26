﻿using System;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Others;
using Cultivation_Way.UI;

namespace Cultivation_Way.Library;

/// <summary>
///     修炼体系
/// </summary>
public class CultisysAsset : Asset
{
    /// <summary>
    ///     在list中的位置
    /// </summary>
    [NonSerialized] internal int pid;

    /// <summary>
    ///     能否修炼判定
    /// </summary>
    [NonSerialized] public CultisysJudge allow;

    /// <summary>
    ///     能否升级判定
    /// </summary>
    [NonSerialized] public CultisysJudge can_levelup;

    /// <summary>
    ///     当前修炼进度
    /// </summary>
    [NonSerialized] public CultisysCheck curr_progress;

    /// <summary>
    ///     [选填]额外升级奖励/惩罚, 返回值无所谓, 参数level为升级后的等级
    /// </summary>
    [NonSerialized] public CultisysCheck external_levelup_bonus;

    /// <summary>
    ///     最大等级
    /// </summary>
    public int max_level;

    /// <summary>
    ///     当前境界最大修炼进度
    /// </summary>
    [NonSerialized] public CultisysCheck max_progress;

    /// <summary>
    ///     [选填]月度更新
    /// </summary>
    [NonSerialized] public CultisysCheck monthly_update_action;

    /// <summary>
    ///     力量基数, b^l
    /// </summary>
    public float power_base = 1;

    /// <summary>
    ///     力量层次, b^l
    /// </summary>
    public float[] power_level { get; internal set; }

    /// <summary>
    ///     体系图标路径, 从根目录开始
    /// </summary>
    public string sprite_path;

    /// <summary>
    ///     获取额外的属性加成数据
    /// </summary>
    [NonSerialized] public CultisysStats stats_action;

    public CultisysAsset(string id, string culti_energy_id, CultisysType type, int max_level)
    {
        this.id = id;
        this.type = type;
        this.max_level = max_level;
        this.culti_energy_id = culti_energy_id;
        power_level = new float[max_level];
        bonus_stats = new BaseStats[max_level];
        for (int i = 0; i < max_level; i++)
        {
            bonus_stats[i] = new BaseStats();
            power_level[i] = 1;
        }
    }

    /// <summary>
    ///     体系类型
    /// </summary>
    public CultisysType type { get; internal set; }

    /// <summary>
    ///     修炼的能量
    /// </summary>
    public EnergyAsset culti_energy { get; internal set; }

    /// <summary>
    ///     临时存储的修炼能量ID
    /// </summary>
    internal string culti_energy_id { get; }

    /// <summary>
    ///     基础属性加成, 单境界, 不累加
    /// </summary>
    public BaseStats[] bonus_stats { get; internal set; }

    /// <summary>
    ///     获取修炼等级对应的力量
    /// </summary>
    public BaseStats get_bonus_stats(CW_Actor actor, int level)
    {
        if (stats_action == null) return bonus_stats[level];
        BaseStats ret = stats_action(actor, this);
        ret.mergeStats(bonus_stats[level]);
        return ret;
    }
}

public class CultisysLibrary : CW_Library<CultisysAsset>
{
    /// <summary>
    ///     添加必填项警告
    /// </summary>
    public override CultisysAsset add(CultisysAsset pAsset)
    {
        if (pAsset.allow == null) Logger.Warn($"Cultisys Asset {pAsset.id}: allow is null");
        if (pAsset.can_levelup == null) Logger.Warn($"Cultisys Asset {pAsset.id}: level_up is null");
        if (pAsset.curr_progress == null) Logger.Warn($"Cultisys Asset {pAsset.id}: curr_progress is null");
        if (pAsset.max_progress == null) Logger.Warn($"Cultisys Asset {pAsset.id}: max_progress is null");
        if (pAsset.sprite_path == null) Logger.Warn($"Cultisys Asset {pAsset.id}: sprite_path is null");
        return base.add(pAsset);
    }

    /// <summary>
    ///     默认allow和level_up返回false, 默认图标为"ui/Icons/iconCultiBook_immortal"
    /// </summary>
    public override void post_init()
    {
        base.post_init();
        int idx = 0;
        foreach (CultisysAsset cultisys in list)
        {
            cultisys.pid = idx++;
            cultisys.allow ??= (actor, culti) => false;
            cultisys.can_levelup ??= (actor, culti) => false;
            cultisys.curr_progress ??= (actor, culti, level) => 0;
            cultisys.max_progress ??= (actor, culti, level) => 1;
            cultisys.sprite_path ??= "ui/Icons/iconCultiSys";
            cultisys.culti_energy = Manager.energies.get(cultisys.culti_energy_id);

            General.AboutUI.WorldLaws.add_setting_law(
                $"worldlaw_config_{cultisys.id}", "worldlaw_cultisys_grid",
                cultisys.sprite_path, Constants.Core.cultisys_config_window,
                () => { WindowCultisysConfig.select_cultisys(cultisys); }
            );
        }
    }
}