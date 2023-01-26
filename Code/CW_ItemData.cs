using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_ItemData : ItemData
    {
        public string spell;
        public CW_ItemData()
        {

        }
        public CW_ItemData(ItemData origin)
        {
            this.by = origin.by;
            this.from = origin.from;
            this.id = origin.id;
            this.kills = origin.kills;
            this.modifiers = origin.modifiers;
            this.name = origin.name;
            this.material = origin.material;
            this.spell = null;
            this.year = origin.year;
        }
    }
}
