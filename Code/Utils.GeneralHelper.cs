using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Utils
{
    public static class GeneralHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string to_json(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T from_json<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }
    }
}
