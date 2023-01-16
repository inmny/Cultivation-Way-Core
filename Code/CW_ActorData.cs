using System;
using System.Collections.Generic;
using Cultivation_Way.Library;
namespace Cultivation_Way
{
    public class CW_ActorData
    {
        public CW_ActorStatus status;
        public int[] cultisys_level;
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
            CW_Asset_CultiBook cultibook = CW_Library_Manager.instance.cultibooks.get(cultibook_id);
            if (cultibook != null && (--cultibook.cur_culti_nr) == 0) { cultibook.try_deprecate();  }
            WorldBoxConsole.Console.print("Destruct");
            if (cultibook != null) WorldBoxConsole.Console.print(string.Format("Cultibook {0}:{1} in destruct cw_actordata", cultibook.id, cultibook.cur_culti_nr));
        }
        internal void pre_learn_cultibook(CW_Asset_CultiBook cultibook)
        {
            if (cultibook == null) return;
            if (!Others.CW_Constants.cultibook_force_learn && cultisys == 0) return;

            cultibook.cur_culti_nr++;
            cultibook.histroy_culti_nr++;
            CW_Asset_CultiBook prev_cultibook = CW_Library_Manager.instance.cultibooks.get(cultibook_id);
            if (prev_cultibook != null && --prev_cultibook.cur_culti_nr == 0) prev_cultibook.try_deprecate();

            this.cultibook_id = cultibook.id;
            //WorldBoxConsole.Console.print(string.Format("Pre-learn cultibook {0}, current culti_nr: {1}", cultibook.id, cultibook.cur_culti_nr));
        }
        public void deepcopy_to(CW_ActorData cw_actor_data)
        {
            cw_actor_data.element.deepcopy_to(element);
            cw_actor_data.status.deepcopy_to(status);
            int i;
            for (i = 0; i < cultisys_level.Length; i++)
            {
                cultisys_level[i] = cw_actor_data.cultisys_level[i];
            }
            cultisys = cw_actor_data.cultisys;
            if (cw_actor_data.compose_setting != null) compose_setting = cw_actor_data.compose_setting.deepcopy();
            if (cw_actor_data.compose_objects_id != null)
            {
                this.compose_objects_id = new List<string>(cw_actor_data.compose_objects_id.Count);
                for (i = 0; i < cw_actor_data.compose_objects_id.Count; i++)
                {
                    compose_objects_id.Add(cw_actor_data.compose_objects_id[i]);
                }
            }
            family_id = cw_actor_data.family_id;
            cultibook_id = cw_actor_data.cultibook_id;
            special_body_id = cw_actor_data.special_body_id;
            family_name = cw_actor_data.family_name;
            pope_id = cw_actor_data.pope_id;
        }
    }
}
