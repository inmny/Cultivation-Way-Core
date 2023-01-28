using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Library
{
    public class CW_Template_Elm
    {
        public string id;
        public string words_id;
        public float free_val;
        public Others.CW_Delegates.CW_Name_Template_Select select_from_objects;
        public Others.CW_Delegates.CW_Name_Template_Decode get_part_from_objects;
        public CW_Template_Elm()
        {
            free_val = 1f;
            select_from_objects = Actions.CW_NameTemplateActions.random_select;
            get_part_from_objects = Actions.CW_NameTemplateActions.random_decode;
        }
    }
    
    public class CW_Template
    {
        public string id;
        public float weight;
        /**关系模板
         * {k}              表示children中索引为k的元素
         * {k[n,i]}         表示在第n冲突组中优先级为i
         * {[m+]k}           表示在k必须在m存在时才会尝试判断
         * 规则介绍:
         * 在一冲突组中，至多只有一个元素会被采纳，在所有会被采纳的元素中，选取优先级最大的元素
         * 在一关系组中，只有所有的前置都被采纳，后置才会被采纳
         * 一个花括号中可以包含多个冲突组和多个关系组
         */
        public string format;
        public List<CW_Template_Elm> children = new List<CW_Template_Elm>();

        private Dictionary<int, List<KeyValuePair<int, int>>> __colide_lists = new Dictionary<int, List<KeyValuePair<int, int>>>();
        private List<KeyValuePair<int, int>> __relation_list = new List<KeyValuePair<int, int>>();

        private List<int> __topologiccal_order; // 拓扑顺序
        private List<List<int>> __post_dependencies_list; // 直接依赖于k的
        private List<List<string>> __parts;
        private List<bool> __tmp_active_list = new List<bool>();
        private List<CW_Template_Elm> __tmp_to_ret = new List<CW_Template_Elm>();
        internal class Node
        {
            public int idx;
            public int in_degree;
            public List<int> out_node_idxs;
        }
        internal void apply_format()
        {
            __parts = new List<List<string>>();
            __split_format();
            foreach(List<string> part in __parts)
            {
                __split_part_into_lists(part);
            }
            __parts = null;
            foreach(List<KeyValuePair<int, int>> colide_group in __colide_lists.Values)
            {
                // 按照优先级降序排序
                colide_group.Sort((left, right) => { return right.Value.CompareTo(left.Value); });
                foreach(KeyValuePair<int, int> part in colide_group)
                {
                    MonoBehaviour.print(string.Format("<{0},{1}>", part.Key, part.Value));
                }
            }
            // TODO: 采用正经的拓扑排序算法，将依赖关系进行拓扑排序
            __topologiccal_order = new List<int>();
            __post_dependencies_list = new List<List<int>>();
            int i,j,k;
            // 构建图
            Node[] nodes = new Node[children.Count];
            int valid_node_nr = 0;
            for (i = 0; i < children.Count; i++)
            {
                nodes[i] = new Node()
                {
                    idx = i,
                    out_node_idxs = new List<int>(),
                    in_degree = 0
                };
            }
            bool[][] map = new bool[children.Count][];
            for (i = 0; i < children.Count; i++)
            {
                map[i] = new bool[children.Count];
                for(j=0;j< children.Count; j++)
                {
                    map[i][j] = false;
                }
            }
            foreach(KeyValuePair<int,int> edge in __relation_list)
            {
                MonoBehaviour.print(string.Format("map[{0}][{1}] = true", edge.Key, edge.Value));
                map[edge.Key][edge.Value] = true;
                nodes[edge.Key].out_node_idxs.Add(edge.Value);
                nodes[edge.Value].in_degree += 1;
            }
            for(i = 0; i < children.Count; i++)
            {
                if (nodes[i].out_node_idxs.Count > 0 || nodes[i].in_degree > 0) valid_node_nr++;
                else
                {
                    nodes[i] = null;
                }
            }
            // 拓扑排序
            while (valid_node_nr > 0)
            {
                for (i = 0; i < children.Count; i++)
                {
                    if (nodes[i] != null && nodes[i].in_degree == 0)
                    {
                        __topologiccal_order.Add(nodes[i].idx);
                        for (j = 0; j < nodes[i].out_node_idxs.Count; j++)
                        {
                            if (map[nodes[i].idx][nodes[i].out_node_idxs[j]] && nodes[nodes[i].out_node_idxs[j]] != null) nodes[nodes[i].out_node_idxs[j]].in_degree--;
                        }
                        valid_node_nr--;
                        nodes[i] = null;
                    }
                }
            }
            for (i = 0; i < __topologiccal_order.Count; i++)
            {
                __post_dependencies_list.Add(new List<int>());
                for (j = 0; j < children.Count; j++)
                {
                    if (map[__topologiccal_order[i]][j])
                    {
                        __post_dependencies_list[i].Add(j);
                    }
                }
            }
            __relation_list.Clear();
            __relation_list = null;
        }
        private void __split_part_into_lists(List<string> part)
        {
            int i;
            for (i = 0; i < part.Count; i++)
            {
                if (!part[i].Contains('+')) break;
            }
            int k_idx = i; int k = Convert.ToInt32(part[k_idx]);
            for (i = 0; i < k_idx; i++)
            {
                int dependency_idx = Convert.ToInt32(part[i].Replace('+', '\0'));
                __relation_list.Add(new KeyValuePair<int, int>(dependency_idx, k));
            }
            for (i = k_idx + 1; i < part.Count; i++)
            {
                string[] group_info = part[i].Split(',');
                int group_idx = Convert.ToInt32(group_info[0]);
                int priority = Convert.ToInt32(group_info[1]);
                if (!__colide_lists.ContainsKey(group_idx))
                {
                    __colide_lists[group_idx] = new List<KeyValuePair<int, int>>();
                }
                __colide_lists[group_idx].Add(new KeyValuePair<int, int>(k, priority));
            }
        }
        private void __split_format()
        {
            string[] split_result = format.Split('{', '}');
            int i = -1;
            foreach (string split_str in split_result)
            {
                if (string.IsNullOrEmpty(split_str)) { i++; continue; }
                if (__parts.Count == i) __parts.Add(new List<string>());
                string[] split_part = split_str.Split('[', ']');
                foreach (string split_part_str in split_part)
                {
                    if (!string.IsNullOrEmpty(split_part_str)) 
                    {
                        //WorldBoxConsole.Console.print(split_part_str);
                        __parts[i].Add(split_part_str);
                    }
                }
            }
        }
        public List<CW_Template_Elm> get_format_to_fill(CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null)
        {
            int i;
            while (__tmp_active_list.Count < children.Count)
            {
                __tmp_active_list.Add(true);
            }
            for (i = 0; i < children.Count; i++)
            {
                __tmp_active_list[i] = true;
            }
            for (i = 0; i < children.Count; i++)
            {
                __tmp_active_list[i] = children[i].select_from_objects(children[i], cw_actor, city, kingdom, culture, tile);
            }
            // 排除冲突部分
            foreach(List<KeyValuePair<int,int>> colide_group in __colide_lists.Values)
            {
                for (i = 0; i < colide_group.Count; i++)
                {
                    if (__tmp_active_list[colide_group[i].Key]) break;
                }
                for(i = i+1; i < colide_group.Count; i++)
                {
                    __tmp_active_list[colide_group[i].Key] = false;
                }
            }
            // 按照生成的依赖关系
            for (i = 0; i < __topologiccal_order.Count; i++)
            {
                if (!__tmp_active_list[__topologiccal_order[i]])
                {
                    foreach(int post_depend_idx in __post_dependencies_list[i])
                    {
                        __tmp_active_list[post_depend_idx] = false;
                    }
                }
            }
            // tmp_active_list中剩余的是最终的结果
            __tmp_to_ret.Clear();
            for (i = 0; i < children.Count; i++)
            {
                if (__tmp_active_list[i]) __tmp_to_ret.Add(children[i]);
            }
            return __tmp_to_ret;
        }
    }
    public class CW_Asset_NameGenerator : Asset
    {
        // public string id; 模板的id，用于匹配
        private float max_weight;
        private List<float> chance_list;
        private List<CW_Template> template_list;
        private Dictionary<string, CW_Template> name_templates;
        internal void register()
        {
            foreach(CW_Template template in template_list)
            {
                template.apply_format();
            }
        }
        public CW_Asset_NameGenerator(string id)
        {
            this.id = id;
            chance_list = new List<float>();
            template_list = new List<CW_Template>();
            name_templates = new Dictionary<string, CW_Template>();
        }
        public void add_template(CW_Template template)
        {
            this.template_list.Add(template);
            this.name_templates.Add(template.id, template);
            chance_list.Add(max_weight);
            max_weight += template.weight;
        }
        public CW_Template choose_template_randomly()
        {
            float random_float = Toolbox.randomFloat(0, max_weight);
            int i;
            for(i = 0; i < chance_list.Count; i++)
            {
                if (chance_list[i] >= random_float) break;
            }
            --i;
            return template_list[i];
        }
        public string gen_name(CW_Template template = null, CW_Actor cw_actor = null, City city = null, Kingdom kingdom = null, Culture culture = null, WorldTile tile = null)
        {
            if (template == null) template = choose_template_randomly();
            List<CW_Template_Elm> name_format = template.get_format_to_fill(cw_actor, city, kingdom, culture, tile);
            StringBuilder string_builder = new StringBuilder();
            foreach(CW_Template_Elm elm in name_format)
            {
                string_builder.Append(elm.get_part_from_objects(elm, cw_actor, city, kingdom, culture, tile));
            }
            return string_builder.ToString();
        }
    }
    public class CW_Asset_Words : Asset
    {
        // public string id; 词库的id
        public List<string> words = new List<string>();
        public void load(string path_to_file)
        {
            this.words = Utils.CW_ResourceHelper.load_list_str(path_to_file);
        }
        public void append(string path_to_file)
        {
            this.words.AddRange(Utils.CW_ResourceHelper.load_list_str(path_to_file));
        }
        public void append_word(string word)
        {
            this.words.Add(word);
        }
        public void append(List<string> words)
        {
            this.words.AddRange(words);
        }
    }
    public class CW_Library_Words : CW_Dynamic_Library<CW_Asset_Words>
    {
        public void load_words(string id, string path_to_file)
        {
            this.add(new CW_Asset_Words() { id = id });
            this.t.load(path_to_file);
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }

        internal override void reset()
        {
            throw new NotImplementedException();
        }
    }
    public class CW_Library_NameGenerator : CW_Asset_Library<CW_Asset_NameGenerator>
    {
        internal override void register()
        {
            foreach(CW_Asset_NameGenerator name_generator in this.list)
            {
                name_generator.register();
            }
        }
    }

}
