using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_WorldEvent : Asset
    {
        public int happen_per_month;
        public Others.CW_Delegates.CW_WorldEvent_Action action;
    }
    public class CW_Library_WorldEvent : AssetLibrary<CW_Asset_WorldEvent>
    {
        internal void register()
        {

        }
    }
}
