using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using Cultivation_Way.Core;
using Cultivation_Way.Constants;
namespace Cultivation_Way.Extension
{
    public static class ActorDataTools
    {
        public static void set_element(this ActorData data, CW_Element element)
        {
            for(int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                data.set(Constants.Core.element_str[i], element.base_elements[i]);
            }
            data.set(DataS.element_type_id, element.comp_type());
        }
        public static CW_Element get_element(this ActorData data)
        {
            CW_Element element = Factories.element_factory.get_item_to_fill();
            element.set(data);
            return Factories.element_factory.create_item(element);
        }
        public static int[] get_cultisys_level(this ActorData data)
        {
            int[] result = new int[Library.Manager.cultisys.size()];
            for(int i=0; i < result.Length; i++)
            {
                data.get(Library.Manager.cultisys.list[i].id, out result[i], -1);
            }
            return result;
        }
    }
}
