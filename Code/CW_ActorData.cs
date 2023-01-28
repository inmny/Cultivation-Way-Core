using System;
using System.Collections.Generic;
using Cultivation_Way.Library;
namespace Cultivation_Way
{
    [Serializable]
    public class CW_ActorData
    {
        public CW_ActorStatus status;
        public int[] cultisys_level;
        public string[] jobs;
        public int[] job_levels;
        internal uint cultisys;
        public CW_Element element;
        public List<string> spells;
        public Compose.CW_ComposeSetting compose_setting;
        public List<string> compose_objects_id;
        public string family_id;
        public string cultibook_id;
        public string special_body_id;
        public string family_name;
        public string pope_id;
        public List<Library.CW_Family_Member_Info> children_info;
        ~CW_ActorData()
        {
            WorldBoxConsole.Console.print("Destruct CW_ActorData");
        }
        internal void pre_learn_cultibook(CW_Asset_CultiBook cultibook)
        {
            if (cultibook == null) return;
            //if (!Others.CW_Constants.cultibook_force_learn && cultisys == 0) return;

            cultibook.cur_culti_nr++;
            cultibook.histroy_culti_nr++;

            this.cultibook_id = cultibook.id;
            //WorldBoxConsole.Console.print(string.Format("Pre-learn cultibook {0}, current culti_nr: {1}", cultibook.id, cultibook.cur_culti_nr));
        }
        public void deepcopy_to(CW_ActorData cw_actor_data)
        {
            element.deepcopy_to(cw_actor_data.element);
            status.deepcopy_to(cw_actor_data.status);
            int i;
            for (i = 0; i < cultisys_level.Length; i++)
            {
                 cw_actor_data.cultisys_level[i] = cultisys_level[i];
            }
            cw_actor_data.cultisys = cultisys;
            if (compose_setting != null) cw_actor_data.compose_setting = compose_setting.deepcopy();
            if (compose_objects_id != null)
            {
                cw_actor_data.compose_objects_id = new List<string>(compose_objects_id.Count);
                for (i = 0; i < cw_actor_data.compose_objects_id.Count; i++)
                {
                    cw_actor_data.compose_objects_id.Add(compose_objects_id[i]);
                }
            }
            cw_actor_data.family_id = family_id;
            cw_actor_data.cultibook_id = cultibook_id;
            cw_actor_data.special_body_id = special_body_id;
            cw_actor_data.family_name = family_name;
            cw_actor_data.pope_id = pope_id;
        }
    }
}
