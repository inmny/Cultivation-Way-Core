using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    /// <summary>
    /// 血脉, id表示
    /// </summary>
    public class BloodNodeAsset : Asset
    {
        /// <summary>
        /// 存活后代数量
        /// </summary>
        public int alive_descendants_count;
    }
    public class BloodNodeLibrary : CW_DynamicLibrary<BloodNodeAsset>
    {

    }
}
