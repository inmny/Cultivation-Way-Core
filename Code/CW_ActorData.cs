using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_ActorData
    {
        public CW_ActorStatus status;
        public int[] cultisys_level;
        public CW_Element element;
        public List<string> spells;
        public Compose.CW_ComposeSetting compose_setting;
        public List<string> compose_objects_id;
        public string family_id;
        public string cultibook_id;
        public string special_body_id;
        public string family_name;
    }
}
