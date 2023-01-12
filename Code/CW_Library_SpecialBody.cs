using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_SpecialBody : Asset
    {
        public string name;
        public string description;
        public string author_name;
        public CW_BaseStats bonus_stats;

    }
    public class CW_Library_SpecialBody : AssetLibrary<CW_Asset_SpecialBody>
    {
        internal void register()
        {
            throw new NotImplementedException();
        }
    }
}
