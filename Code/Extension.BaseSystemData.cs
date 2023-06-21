using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extension
{
    public static class BaseSystemDataTools
    {
        public static void clear(this BaseSystemData data)
        {
            data.custom_data_bool?.dict?.Clear();
            data.custom_data_float?.dict?.Clear();
            data.custom_data_int?.dict?.Clear();
            data.custom_data_string?.dict?.Clear();
            data.custom_data_flags?.Clear();
        }
        /// <summary>
        /// 以key为key, 将@object JSON序列化后写入data
        /// </summary>
        public static void write_obj<T>(this BaseSystemData data, string key, T @object)
        {
            data.set(key, fastJSON.JSON.ToJSON(@object));
        }
        /// <summary>
        /// 以key为key, 从data中读取JSON, 并反序列化为T, 若不存在则会返回default(T)
        /// </summary>
        public static T read_obj<T>(this BaseSystemData data, string key)
        {
            data.get(key, out string obj_str);

            if (string.IsNullOrEmpty(obj_str)) return default;
            
            return fastJSON.JSON.ToObject<T>(obj_str);
        }
    }
}
