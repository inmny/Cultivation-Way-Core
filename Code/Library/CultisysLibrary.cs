using System;
using System.IO;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Others;
using Cultivation_Way.UI;
using Cultivation_Way.Utils;
using NeoModLoader.General;
using Newtonsoft.Json;

namespace Cultivation_Way.Library;

/// <summary>
///     修炼体系
/// </summary>
public class CultisysAsset : Asset
{
    /// <summary>
    ///     能否修炼判定
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysJudge allow;

    /// <summary>
    ///     基础属性加成, 单境界, 不累加
    /// </summary>
    public BaseStats[] bonus_stats;

    /// <summary>
    ///     能否升级判定
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysJudge can_levelup;

    /// <summary>
    ///     当前修炼进度
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysCheck curr_progress;

    /// <summary>
    ///     各个境界的突破难度
    /// </summary>
    public float[] difficulty_per_level;

    /// <summary>
    ///     [选填]额外升级奖励/惩罚, 返回值无所谓, 参数level为升级后的等级
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysCheck external_levelup_bonus;

    /// <summary>
    ///     最大等级
    /// </summary>
    public int max_level;

    /// <summary>
    ///     当前境界最大修炼进度
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysCheck max_progress;

    /// <summary>
    ///     [选填]月度更新
    /// </summary>
    [NonSerialized, JsonIgnore] public CultisysCheck monthly_update_action;

    /// <summary>
    ///     各个境界人数限制
    /// </summary>
    public int[] number_limit_per_level;

    /// <summary>
    ///     各个境界当前人数
    /// </summary>
    [JsonIgnore] public int[] number_per_level;

    /// <summary>
    ///     在list中的位置
    /// </summary>
    [NonSerialized, JsonIgnore] internal int pid;

    /// <summary>
    ///     力量基数, b^l
    /// </summary>
    public float power_base = 1;

    /// <summary>
    ///     体系图标路径, 从根目录开始
    /// </summary>
    [JsonIgnore] public string sprite_path;

    /// <summary>
    ///     获取额外的属性加成数据
    /// </summary>
    [NonSerialized] public CultisysStats stats_action;

    public CultisysAsset(string id, string culti_energy_id, CultisysType type, int max_level,
        CultisysInit init_action = null)
    {
        this.id = id;
        this.type = type;
        this.max_level = max_level;
        this.culti_energy_id = culti_energy_id;
        power_level = new float[max_level];
        bonus_stats = new BaseStats[max_level];
        number_limit_per_level = new int[max_level];
        number_per_level = new int[max_level];
        difficulty_per_level = new float[max_level];
        for (int i = 0; i < max_level; i++)
        {
            bonus_stats[i] = new BaseStats();
            power_level[i] = 1;
            number_limit_per_level[i] = int.MaxValue;
            number_per_level[i] = 0;
            difficulty_per_level[i] = 1;
        }

        this.init_action = init_action;
    }

    /// <summary>
    ///     力量层次, b^l
    /// </summary>
    public float[] power_level { get; internal set; }

    /// <summary>
    ///     体系类型
    /// </summary>
    [JsonIgnore]
    public CultisysType type { get; internal set; }

    /// <summary>
    ///     修炼的能量
    /// </summary>
    [JsonIgnore]
    public EnergyAsset culti_energy { get; internal set; }

    /// <summary>
    ///     临时存储的修炼能量ID
    /// </summary>
    [JsonIgnore]
    internal string culti_energy_id { get; }

    [JsonIgnore] public CultisysInit init_action { get; internal set; }

    public bool CanLevelUp(CW_Actor pActor, CultisysAsset pAsset, int pLevel)
    {
        if (pLevel >= number_per_level.Length || number_per_level[pLevel] >= number_limit_per_level[pLevel])
        {
            return false;
        }

        if (difficulty_per_level[pLevel] >= 1 && !Toolbox.randomChance(1 / difficulty_per_level[pLevel]))
        {
            return false;
        }

        return can_levelup(pActor, pAsset, pLevel);
    }

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
            cultisys.allow ??= (actor, culti, level) => false;
            cultisys.can_levelup ??= (actor, culti, level) => false;
            cultisys.curr_progress ??= (actor, culti, level) => 0;
            cultisys.max_progress ??= (actor, culti, level) => 1;
            cultisys.sprite_path ??= "ui/Icons/iconCultiSys";
            cultisys.culti_energy = Manager.energies.get(cultisys.culti_energy_id);
            cultisys.init_action?.Invoke(cultisys);

            try_to_load_cultisys_config(cultisys);

            Utils.General.AboutUI.WorldLaws.add_setting_law(
                $"worldlaw_config_{cultisys.id}", "worldlaw_cultisys_grid",
                cultisys.sprite_path, Constants.Core.cultisys_config_window,
                () => { WindowCultisysConfig.select_cultisys(cultisys); }
            );
        }

        LM.ApplyLocale();
    }

    private void try_to_load_cultisys_config(CultisysAsset cultisys)
    {
        string cultisys_path = Path.Combine(Paths.DataPath, $"{cultisys.id}.json");
        string cultisys_locale_path = Path.Combine(Paths.DataPath, $"{cultisys.id}_locale.json");
        if (File.Exists(cultisys_locale_path))
        {
            LM.LoadLocale(LocalizedTextManager.instance.language, cultisys_locale_path);
        }

        if (!File.Exists(cultisys_path))
        {
            return;
        }

        CultisysAsset loaded = GeneralHelper.from_json<CultisysAsset>(File.ReadAllText(cultisys_path), true);
        if (loaded == null) return;
        foreach (BaseStats stats in loaded.bonus_stats)
        {
            stats.AfterDeserialize();
        }

        cultisys.bonus_stats = loaded.bonus_stats;
        cultisys.difficulty_per_level = loaded.difficulty_per_level;
        cultisys.number_limit_per_level = loaded.number_limit_per_level;
        cultisys.power_base = loaded.power_base;
        cultisys.power_level = loaded.power_level;
    }

    internal void ClearForNewGame()
    {
        foreach (var cultisys in list)
            for (var i = 0; i < cultisys.number_per_level.Length; i++)
                cultisys.number_per_level[i] = 0;
    }
}