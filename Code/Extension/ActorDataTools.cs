﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using UnityEngine;

namespace Cultivation_Way.Extension;

public static class ActorDataTools
{
    public static float GetSpellImprintExp(this ActorData pData)
    {
        pData.get(DataS.spell_imprint_exp, out var exp, 0f);
        return exp;
    }

    public static void IncreaseSpellImprintExp(this ActorData pData, float pExp)
    {
        pData.get(DataS.spell_imprint_exp, out var exp, 0f);
        pData.set(DataS.spell_imprint_exp, exp + pExp);
    }

    /// <summary>
    ///     设置灵根比例
    /// </summary>
    public static void SetElement(this ActorData pData, CW_Element pElement)
    {
        for (int i = 0; i < Constants.Core.element_type_nr; i++)
        {
            pData.set(Constants.Core.element_str[i], pElement.BaseElements[i]);
        }

        pData.set(DataS.element_type_id, pElement.ComputeType());
    }

    /// <summary>
    ///     读取灵根
    /// </summary>
    /// <returns>灵根的拷贝</returns>
    public static CW_Element GetElement(this ActorData pData)
    {
        CW_Element element = Factories.element_factory.get_item_to_fill();
        element.Set(pData);
        return Factories.element_factory.get_next(element);
    }

    /// <summary>
    ///     读取修炼功法
    /// </summary>
    public static Cultibook GetCultibook(this ActorData pData)
    {
        pData.get(DataS.cultibook_id, out string cultibook_id, "");
        return Manager.cultibooks.get(cultibook_id);
    }

    /// <summary>
    ///     设置修炼功法, 自动增加/减少 新/旧修炼功法的引用计数
    /// </summary>
    public static void SetCultibook(this ActorData pData, Cultibook pCultibook)
    {
        Cultibook old_cultibook = pData.GetCultibook();
        old_cultibook?.decrease();
        pCultibook.increase();
        pData.set(DataS.cultibook_id, pCultibook.id);
    }

    /// <summary>
    ///     清除修炼功法, 自动减少旧修炼功法的引用计数
    /// </summary>
    public static void ClearCultibook(this ActorData pData)
    {
        Cultibook old_cultibook = pData.GetCultibook();
        old_cultibook?.decrease();
        pData.removeString(DataS.cultibook_id);
    }

    /// <summary>
    ///     读取所有修炼体系的等级
    /// </summary>
    /// <returns>所有修炼体系等级的数组的拷贝</returns>
    public static int[] GetAllCultisysLevels(this ActorData pData)
    {
        int[] result = new int[Manager.cultisys.size];
        if (pData == null)
        {
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = -1;
            }

            return result;
        }

        for (int i = 0; i < result.Length; i++)
        {
            pData.get(Manager.cultisys.list[i].id, out result[i], -1);
        }

        return result;
    }

    public static float GetMaxPower(this ActorData pData)
    {
        float max_power = 1;
        var cultisys = pData.GetCultisys(CultisysType.WAKAN);
        if (cultisys != null)
        {
            pData.get(cultisys.id, out int level);
            max_power = Math.Max(max_power, Mathf.Pow(cultisys.power_base, cultisys.power_level[level]));
        }

        cultisys = pData.GetCultisys(CultisysType.BODY);
        if (cultisys != null)
        {
            pData.get(cultisys.id, out int level);
            max_power = Math.Max(max_power, Mathf.Pow(cultisys.power_base, cultisys.power_level[level]));
        }

        cultisys = pData.GetCultisys(CultisysType.SOUL);
        if (cultisys != null)
        {
            pData.get(cultisys.id, out int level);
            max_power = Math.Max(max_power, Mathf.Pow(cultisys.power_base, cultisys.power_level[level]));
        }

        return max_power;
    }

    public static CultisysAsset GetCultisys(this ActorData pData, CultisysType pType)
    {
        string pTypeStr = pType switch
        {
            CultisysType.BODY  => "BODY",
            CultisysType.SOUL  => "SOUL",
            CultisysType.WAKAN => "WAKAN",
            CultisysType.BLOOD => "BLOOD",
            CultisysType.HIDDEN => "HIDDEN",
            _ => throw new ArgumentOutOfRangeException(nameof(pType), pType, null)
        };
        pData.get(pTypeStr, out string cultisys_id);
        return string.IsNullOrEmpty(cultisys_id) ? null : Manager.cultisys.get(cultisys_id);
    }

    /// <summary>
    ///     读取所有法术, 注意!无法术时返回null
    /// </summary>
    /// <returns>读取所有法术的集合的拷贝</returns>
    public static HashSet<string> GetSpells(this ActorData pData)
    {
        return pData.ReadObj<HashSet<string>>(DataS.spells);
    }

    /// <summary>
    ///     写入一个法术集合
    /// </summary>
    public static void SetSpells(this ActorData pData, HashSet<string> pSpells)
    {
        pData.WriteObj(DataS.spells, pSpells);
    }

    /// <summary>
    ///     添加一个法术
    /// </summary>
    public static void AddSpell(this ActorData pData, string pSpell)
    {
        HashSet<string> curr_spells = pData.GetSpells();
        curr_spells ??= new HashSet<string>();
        curr_spells.Add(pSpell);
        pData.WriteObj(DataS.spells, curr_spells);
    }

    /// <summary>
    ///     读取所有血脉节点
    /// </summary>
    /// <returns>血脉节点词典拷贝</returns>
    public static Dictionary<string, float> GetBloodNodes(this ActorData pData)
    {
        return pData.ReadObj<Dictionary<string, float>>(DataS.blood_nodes);
    }

    /// <summary>
    ///     设置所有血脉节点, 并设置占优血脉
    /// </summary>
    public static void SetBloodNodes(this ActorData pData, Dictionary<string, float> pBloodNodes)
    {
        Dictionary<string, float> old_blood_nodes = pData.GetBloodNodes();
        if (old_blood_nodes != null)
        {
            foreach (string key in old_blood_nodes.Keys)
            {
                Manager.bloods.get(key).decrease();
            }
        }

        /* 删除低占比血脉, 并normalize至其和为1 */
        List<string> keys = pBloodNodes.Keys.ToList();

        float sum_at_first = keys.Sum(key => pBloodNodes[key]);

        float curr_sum = sum_at_first;


        foreach (string key in keys
                     .Where(key => !(pBloodNodes[key] / sum_at_first >= Constants.Others.blood_ignore_line)))
        {
            curr_sum -= pBloodNodes[key];
            pBloodNodes.Remove(key);
        }

        keys.Clear();
        keys.AddRange(pBloodNodes.Keys);
        foreach (string key in keys)
        {
            pBloodNodes[key] /= curr_sum;
        }


        foreach (string key in keys)
        {
            Manager.bloods.get(key).increase();
        }

/*
        foreach (string key in keys)
        {
            Logger.Log($"{key}: {pBloodNodes[key]}");
        }
*/
        pData.WriteObj(DataS.blood_nodes, pBloodNodes);
        if (pBloodNodes.Count > 0)
        {
            var main_blood = pBloodNodes.Aggregate((max, cur) => max.Value > cur.Value ? max : cur);
            pData.set(DataS.main_blood_id, main_blood.Key);
            pData.set(DataS.main_blood_purity, main_blood.Value);
        }
        else
        {
            pData.set(DataS.main_blood_id, "");
        }
    }

    /// <summary>
    ///     仅设置所有血脉节点, 不更新血脉使用情况
    /// </summary>
    internal static void set_blood_nodes_only_save(this ActorData data, Dictionary<string, float> blood_nodes)
    {
        Dictionary<string, float> old_blood_nodes = data.GetBloodNodes();
        if (old_blood_nodes is { Keys.Count: > 0 } && Constants.Others.strict_mode)
            throw new Exception("only_save should not be true when old blood nodes exist");

        /* 删除低占比血脉, 并normalize至其和为1 */

        List<string> keys = blood_nodes.Keys.ToList();

        float sum_at_first = keys.Sum(key => blood_nodes[key]);

        float curr_sum = sum_at_first;

        foreach (string key in keys
                     .Where(key => !(blood_nodes[key] / sum_at_first >= Constants.Others.blood_ignore_line)))
        {
            curr_sum -= blood_nodes[key];
            blood_nodes.Remove(key);
        }

        if (blood_nodes.Count == 0)
        {
            return;
        }

        keys.Clear();
        keys.AddRange(blood_nodes.Keys);
        foreach (string key in keys)
        {
            blood_nodes[key] /= curr_sum;
        }

        data.WriteObj(DataS.blood_nodes, blood_nodes);
        var main_blood = blood_nodes.Aggregate((max, cur) => max.Value > cur.Value ? max : cur);
        data.set(DataS.main_blood_id, main_blood.Key);
        data.set(DataS.main_blood_purity, main_blood.Value);
    }

    /// <summary>
    ///     清除所有血脉
    /// </summary>
    internal static void clear_blood_nodes(this ActorData data)
    {
        Dictionary<string, float> old_blood_nodes = data.GetBloodNodes();
        if (old_blood_nodes == null || old_blood_nodes.Keys.Count == 0) return;

        foreach (string key in old_blood_nodes.Keys)
        {
            Manager.bloods.get(key).decrease();
        }

        data.removeString(DataS.blood_nodes);
        data.removeString(DataS.main_blood_id);
        data.removeString(DataS.main_blood_purity);
    }

    /// <summary>
    ///     读取占优血脉节点id
    /// </summary>
    public static string GetMainBloodID(this ActorData pData)
    {
        pData.get(DataS.main_blood_id, out string result, "");
        return result;
    }

    /// <summary>
    ///     读取占优血脉节点纯度
    /// </summary>
    public static float GetMainBloodPurity(this ActorData pData)
    {
        pData.get(DataS.main_blood_purity, out float result);
        return result;
    }

    /// <summary>
    ///     读取占优血脉节点
    /// </summary>
    public static BloodNodeAsset GetMainBlood(this ActorData pData)
    {
        return Manager.bloods.get(pData.GetMainBloodID());
    }
}