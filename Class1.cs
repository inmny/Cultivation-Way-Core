using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Cultivation_Way
{
    internal class Class1
    {
        static List<List<string>> parts = new List<List<string>>();
        static Dictionary<int, List<KeyValuePair<int,int>>> colide_lists = new Dictionary<int, List<KeyValuePair<int,int>>>();
        static Dictionary<int, KeyValuePair<List<int>, List<int>>> relation_list_dict = new Dictionary<int, KeyValuePair<List<int>, List<int>>>();
        static List<List<int>> sorted_colide_lists = new List<List<int>>();
        static List<KeyValuePair<List<int>, List<int>>> sorted_relation_lists = new List<KeyValuePair<List<int>, List<int>>>();
        static void output_parts()
        {
            int i;
            int j;
            for (i = 0; i < parts.Count; i++)
            {
                Console.WriteLine(String.Format("Parts[{0}]:", i));
                for (j = 0; j < parts[i].Count; j++)
                {
                    Console.WriteLine(String.Format("[{0}]: '{1}'", j, parts[i][j]));
                }
            }
        }
        static void split_string(string str)
        {
            Console.WriteLine(string.Format("split '{0}' into:", str));
            string[] split_result = str.Split('{','}');
            int i = -1;
            foreach (string split_str in split_result)
            {
                if (string.IsNullOrEmpty(split_str)) { i++; continue; }
                if (parts.Count==i) parts.Add(new List<string>());
                string[] split_part = split_str.Split('[', ']');
                foreach(string split_part_str in split_part)
                {
                    if(!string.IsNullOrEmpty(split_part_str)) parts[i].Add(split_part_str);
                }
            }

        }
        static void split_part_into_lists(List<string> part)
        {
            int i;
            for(i=0; i < part.Count; i++)
            {
                if (!part[i].Contains(',')) break;
            }
            int k_idx = i; int k = Convert.ToInt32(part[k_idx]);
            for (i = 0; i < k_idx; i++)
            {
                string[] group_info = part[i].Split(',');
                int group_idx = Convert.ToInt32(group_info[0]);
                int relation = Convert.ToInt32(group_info[1]);
                if (!relation_list_dict.ContainsKey(group_idx))
                {
                    relation_list_dict[group_idx] = new KeyValuePair<List<int>, List<int>>(new List<int>(), new List<int>());
                }
                if (relation == 0)
                {
                    relation_list_dict[group_idx].Value.Add(k);
                }
                else
                {
                    relation_list_dict[group_idx].Key.Add(k);
                }
            }
            for (i = k_idx+1; i < part.Count; i++)
            {
                string[] group_info = part[i].Split(',');
                int group_idx = Convert.ToInt32(group_info[0]);
                int priority = Convert.ToInt32(group_info[1]);
                if (!colide_lists.ContainsKey(group_idx))
                {
                    colide_lists[group_idx] = new List<KeyValuePair<int,int>>();
                }
                colide_lists[group_idx].Add(new KeyValuePair<int, int>(k, priority));
            }
        }
        static void split_parts_into_lists()
        {
            foreach(List<string> part in parts)
            {
                split_part_into_lists(part);
            }
            int i = 0;
            foreach(List<KeyValuePair<int,int>> colide_list in colide_lists.Values)
            {
                // 按优先级降序
                colide_list.Sort((left, right) => { return right.Value.CompareTo(left.Value); });

                sorted_colide_lists.Add(new List<int>());
                foreach(KeyValuePair<int,int> pair in colide_list)
                {
                    sorted_colide_lists[i].Add(pair.Key);
                }

                i++;
            }
        }
        static List<string> words = new List<string> { "AAA","嗨嗨嗨"};
        static void save_list_str(string path, List<string> content)
        {
            File.WriteAllText(string.Format("{0}.json", path), JsonUtility.ToJson(content));
        }
        static List<string> load_list_str(string path)
        {
            return JsonUtility.FromJson<List<string>>(File.ReadAllText(string.Format("{0}.json", path)));
        }
        static void Main(string[] args)
        {
            //split_string("{[1,0][2,1]1[2,4][3,7]}{[2,0]8[2,7]}");
            //output_parts();
            //split_parts_into_lists();
            save_list_str("test", words);
            Console.ReadKey();
        }

        
    }
}
