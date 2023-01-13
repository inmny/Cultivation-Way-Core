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
        public int level;
        public CW_BaseStats bonus_stats;

    }
    public class CW_Library_SpecialBody : CW_Dynamic_Library<CW_Asset_SpecialBody>
    {
        internal override void register()
        {
            throw new NotImplementedException();
        }
        internal override void reset()
        {
            throw new NotImplementedException();
        }
        internal string select_better(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b;
            if (string.IsNullOrEmpty(b)) return a;
            CW_Asset_SpecialBody A = get(a);
            CW_Asset_SpecialBody B = get(b);
            if (A == null) return B == null ? null : b;
            if (B == null) return a;
            return A.level >= B.level ? a : b;
        }
    }
}
