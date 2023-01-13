using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    public class CW_ResourceHelper
    {
        public static Sprite[] load_sprites(string path)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 加载JSON为string，低效，请确保加载次数有限
        /// </summary>
        /// <param name="path">路径，从GameResources之下开始</param>
        /// <returns></returns>
        public static string load_json_once(string path)
        {
            return File.ReadAllText(ModState.instance.mod_info.path+"/GameResources/"+path+".json");
        }
    }
}
