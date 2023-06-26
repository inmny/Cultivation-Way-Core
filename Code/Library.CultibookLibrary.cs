using System;
using System.Collections.Generic;

namespace Cultivation_Way.Library;

public class Cultibook : Asset
{
    /// <summary>
    ///     作者名
    /// </summary>
    public string author_name = "";

    /// <summary>
    ///     属性加成
    /// </summary>
    public BaseStats bonus_stats = new();

    /// <summary>
    ///     功法描述
    /// </summary>
    public string description = "";

    /// <summary>
    ///     最后编者名
    /// </summary>
    public string editor_name = "";

    /// <summary>
    ///     功法等级
    /// </summary>
    public int level = 1;

    /// <summary>
    ///     最大可容纳法术数量
    /// </summary>
    public int max_spell_nr;

    /// <summary>
    ///     功法名
    /// </summary>
    public string name = "";

    /// <summary>
    ///     自带法术
    /// </summary>
    public List<string> spells = new();

    /// <summary>
    ///     当前使用人数
    /// </summary>
    public int cur_users { get; internal set; }

    /// <summary>
    ///     历史最大使用人数
    /// </summary>
    public int max_users { get; internal set; }

    /// <summary>
    ///     从from深拷贝基础数据（除使用人数与id)
    /// </summary>
    public void copy_from(Cultibook from, bool with_spells = true)
    {
        name = from.name;
        description = from.description;
        author_name = from.author_name;
        editor_name = from.editor_name;
        level = from.level;
        bonus_stats.mergeStats(from.bonus_stats);
        if (with_spells)
        {
            spells.AddRange(from.spells);
        }
    }

    /// <summary>
    ///     减少使用人数
    /// </summary>
    /// <exception cref="Exception">使用人数为负数</exception>
    public void decrease()
    {
        cur_users--;
        if (cur_users < 0 && Constants.Others.strict_mode)
        {
            max_users = Constants.Others.cultibook_lock_line;
            throw new Exception(
                $"Error current users {cur_users} for Cultibook {id}. Set its max_users up to remove line");
        }
    }

    /// <summary>
    ///     增加使用人数
    /// </summary>
    public void increase()
    {
        cur_users++;
        if (cur_users > max_users)
        {
            max_users = cur_users;
        }
    }
}

public class CultibookLibrary : CW_DynamicLibrary<Cultibook>
{
    /// <summary>
    ///     删除使用人数为0的功法
    /// </summary>
    public override void update()
    {
        base.update();
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].cur_users <= 0 && list[i].max_users < Constants.Others.cultibook_lock_line)
            {
                dict.Remove(list[i].id);
                list.RemoveAt(i);
                i--;
            }
        }
    }

    public override void post_init()
    {
        base.post_init();
    }

    /// <summary>
    ///     允许查询的id为null或空字符串
    /// </summary>
    public override Cultibook get(string pID)
    {
        if (string.IsNullOrEmpty(pID)) return null;

        return base.get(pID);
    }
}