using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Energy : Asset
    {
        /// <summary>
        /// 索引
        /// </summary>
        public int idx;
        /// <summary>
        /// 升级的系数
        /// </summary>
        public float level_co;
        /// <summary>
        /// 与其他能量的兑换比例
        /// </summary>
        public float transform_co;
    }
    public class CW_Library_Energy : CW_Asset_Library<CW_Asset_Energy>
    {
        internal override void register()
        {
            for(int i = 0; i < this.list.Count; i++)
            {
                list[i].idx = i;
            }
        }
    }
}
