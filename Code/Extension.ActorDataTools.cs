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
        /// <summary>
        /// 设置灵根比例
        /// </summary>
        public static void set_element(this ActorData data, CW_Element element)
        {
            for(int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                data.set(Constants.Core.element_str[i], element.base_elements[i]);
            }
            data.set(DataS.element_type_id, element.comp_type());
        }
        /// <summary>
        /// 获取灵根
        /// </summary>
        /// <returns>灵根的拷贝</returns>
        public static CW_Element get_element(this ActorData data)
        {
            CW_Element element = Factories.element_factory.get_item_to_fill();
            element.set(data);
            return Factories.element_factory.get_next(element);
        }
        /// <summary>
        /// 获取所有修炼体系的等级
        /// </summary>
        /// <returns>所有修炼体系等级的数组的拷贝</returns>
        public static int[] get_cultisys_level(this ActorData data)
        {
            int[] result = new int[Library.Manager.cultisys.size];
            for(int i=0; i < result.Length; i++)
            {
                data.get(Library.Manager.cultisys.list[i].id, out result[i], -1);
            }
            return result;
        }
        public static Dictionary<string, float> get_blood_nodes(this ActorData data)
        {
            return data.read_obj<Dictionary<string, float>>(DataS.blood_nodes);
        }
        public static void set_blood_nodes(this ActorData data, Dictionary<string, float> blood_nodes)
        {
            data.write_obj(DataS.blood_nodes, blood_nodes);
        }
    }
}
