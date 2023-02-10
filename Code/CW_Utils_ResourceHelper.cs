using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

namespace Cultivation_Way.Utils
{
    public class CW_ResourceHelper
    {
        internal static Sprite[] load_sprites(string path)
        {
            throw new NotImplementedException();
        }
        private static string __encode_path(string path, string postfix) 
        { 
            return string.Format("{0}/GameResources/{1}{2}", ModState.instance.mod_info.path, path, postfix); 
        }
        /// <summary>
        /// 加载JSON为string，低效，请确保加载次数有限
        /// </summary>
        /// <param name="path">路径，从GameResources之下开始</param>
        /// <returns></returns>
        public static string load_json_once(string path)
        {
            return File.ReadAllText(__encode_path(path, ".json"));
        }
        public static void save_list_str(string path, List<string> content)
        {
            foreach(string item in content) MonoBehaviour.print(item);
            FileStream fs = new FileStream(__encode_path(path, ".txt"), FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Flush();
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            for (int i = 0; i < content.Count; i++) { sw.WriteLine(content[i]); }
            sw.Flush();
            sw.Close();
            fs.Close();
        }
        public static List<string> load_list_str(string path)
        {
            FileStream fs = new FileStream(__encode_path(path, ".txt"), FileMode.Open, FileAccess.Read);
            List<string> ret = new List<string>();
            StreamReader sr = new StreamReader(fs);
            sr.BaseStream.Seek(0, SeekOrigin.Begin);
            string tmp = sr.ReadLine();
            while (tmp != null)
            {
                ret.Add(tmp);
                tmp = sr.ReadLine();
            }
            sr.Close();
            fs.Close();
            return ret;
        }
    }
}
