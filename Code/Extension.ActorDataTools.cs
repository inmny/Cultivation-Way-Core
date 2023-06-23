﻿using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using System.Collections.Generic;
using System.Linq;

namespace Cultivation_Way.Extension
{
    public static class ActorDataTools
    {
        /// <summary>
        /// 设置灵根比例
        /// </summary>
        public static void set_element(this ActorData data, CW_Element element)
        {
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                data.set(Constants.Core.element_str[i], element.base_elements[i]);
            }
            data.set(DataS.element_type_id, element.comp_type());
        }
        /// <summary>
        /// 读取灵根
        /// </summary>
        /// <returns>灵根的拷贝</returns>
        public static CW_Element get_element(this ActorData data)
        {
            CW_Element element = Factories.element_factory.get_item_to_fill();
            element.set(data);
            return Factories.element_factory.get_next(element);
        }
        /// <summary>
        /// 读取修炼功法
        /// </summary>
        public static Cultibook get_cultibook(this ActorData data)
        {
            data.get(DataS.cultibook_id, out string cultibook_id, "");
            return Library.Manager.cultibooks.get(cultibook_id);
        }
        /// <summary>
        /// 设置修炼功法, 自动增加/减少 新/旧修炼功法的引用计数
        /// </summary>
        public static void set_cultibook(this ActorData data, Cultibook cultibook)
        {
            Cultibook old_cultibook = data.get_cultibook();
            old_cultibook?.decrease();
            cultibook.increase();
            data.set(DataS.cultibook_id, cultibook.id);
        }
        /// <summary>
        /// 读取所有修炼体系的等级
        /// </summary>
        /// <returns>所有修炼体系等级的数组的拷贝</returns>
        public static int[] get_cultisys_level(this ActorData data)
        {
            int[] result = new int[Library.Manager.cultisys.size];
            for (int i = 0; i < result.Length; i++)
            {
                data.get(Library.Manager.cultisys.list[i].id, out result[i], -1);
            }
            return result;
        }
        /// <summary>
        /// 读取所有法术
        /// </summary>
        /// <returns>读取所有法术的集合的拷贝</returns>
        public static HashSet<string> get_spells(this ActorData data)
        {
            return data.read_obj<HashSet<string>>(DataS.spells);
        }
        /// <summary>
        /// 写入一个法术集合
        /// </summary>
        public static void set_spells(this ActorData data, HashSet<string> spells)
        {
            data.write_obj(DataS.spells, spells);
        }
        /// <summary>
        /// 添加一个法术
        /// </summary>
        public static void add_spell(this ActorData data, string spell)
        {
            HashSet<string> curr_spells = data.get_spells();
            curr_spells.Add(spell);
            data.write_obj(DataS.spells, curr_spells);
        }
        /// <summary>
        /// 读取所有血脉节点
        /// </summary>
        /// <returns>血脉节点词典拷贝</returns>
        public static Dictionary<string, float> get_blood_nodes(this ActorData data)
        {
            return data.read_obj<Dictionary<string, float>>(DataS.blood_nodes);
        }
        /// <summary>
        /// 设置所有血脉节点, 并设置占优血脉
        /// </summary>
        public static void set_blood_nodes(this ActorData data, Dictionary<string, float> blood_nodes)
        {
            Dictionary<string, float> old_blood_nodes = data.get_blood_nodes();

            foreach (string key in old_blood_nodes.Keys)
            {
                Library.Manager.bloods.get(key).decrease();
            }
            /* 删除低占比血脉, 并normalize至其和为1 */
            float sum_at_first = 0;
            foreach (string key in blood_nodes.Keys)
            {
                sum_at_first += blood_nodes[key];
            }
            float curr_sum = sum_at_first;

            List<string> keys = blood_nodes.Keys.ToList();

            foreach (string key in keys)
            {
                if (blood_nodes[key] / sum_at_first >= Constants.Others.blood_ignore_line) continue;

                curr_sum -= blood_nodes[key];
                blood_nodes.Remove(key);
            }
            foreach (string key in blood_nodes.Keys)
            {
                blood_nodes[key] /= curr_sum;
            }


            foreach (string key in blood_nodes.Keys)
            {
                Library.Manager.bloods.get(key).increase();
            }

            data.write_obj(DataS.blood_nodes, blood_nodes);
            data.set(DataS.main_blood_id, blood_nodes.Aggregate((max, cur) => max.Value > cur.Value ? max : cur).Key);
        }
        /// <summary>
        /// 读取占优血脉节点id
        /// </summary>
        public static string get_main_blood_id(this ActorData data)
        {
            data.get(DataS.main_blood_id, out string result, "");
            return result;
        }
        /// <summary>
        /// 读取占优血脉节点
        /// </summary>
        public static BloodNodeAsset get_main_blood(this ActorData data)
        {
            return Library.Manager.bloods.get(data.get_main_blood_id());
        }
    }
}