using System.Collections.Generic;

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
        public List<Library.CW_Family_Member_Info> children_info;
    }
}
