using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using Cultivation_Way.Library;
using Cultivation_Way.Constants;
using Cultivation_Way.Factory;

namespace Cultivation_Way.Core
{
    public class CW_Element : FactoryItem<CW_Element>
    {
        public int[] base_elements = new int[Constants.Core.element_type_nr];
        private string type_id;
        private static BaseStats tmp_stats;
        private CW_Element()
        {
        }
        /// <summary>
        /// 创建CW_Element对象
        /// </summary>
        /// <param name="base_elements">给定元素组合</param>
        /// <param name="normalize">是否规格化</param>
        /// <param name="normalize_ceil">规格化上界</param>
        /// <param name="comp_type">是否即时确定元素类型</param>
        private CW_Element(int[] base_elements, bool normalize = false, int normalize_ceil = 100, bool comp_type = true)
        {
            for (int i = 0; i < Constants.Core.element_str.Length; i++)
            {
                this.base_elements[i] = base_elements[i];
            }
            if (normalize) __normalize(normalize_ceil);
            if (comp_type) __comp_type();
        }
        /// <summary>
        /// 创建CW_Element对象
        /// </summary>
        /// <param name="random_generate">是否随机生成</param>
        /// <param name="normalize">是否规格化</param>
        /// <param name="normalize_ceil">规格化上界</param>
        /// <param name="comp_type">是否即时确定元素类型</param>
        /// <param name="prefer_elements">偏好元素</param>
        /// <param name="prefer_scale">偏好系数</param>
        internal CW_Element(bool random_generate = true, bool normalize = true, int normalize_ceil = 100, bool comp_type = true, int[] prefer_elements = null, float prefer_scale = 0f)
        {
            base_elements = new int[Constants.Core.element_str.Length];
            if (random_generate)
            {
                __random_generate(normalize_ceil);
                if (prefer_elements != null && prefer_scale >= 0.01f) __prefer_to(prefer_elements, prefer_scale);
                if (normalize) __normalize(normalize_ceil);
                if (comp_type) __comp_type();
            }
            else
            {
                __uniform_generate(normalize_ceil);
                if (comp_type) type_id = Constants.Core.uniform_type;
            }
        }
        /// <summary>
        /// 获取用来初始化人物数据用的临时元素
        /// </summary>
        /// <param name="prefer_elements"></param>
        /// <param name="prefer_scale"></param>
        /// <returns></returns>
        internal static CW_Element get_element_for_set_data(int[] prefer_elements = null, float prefer_scale = 0f)
        {
            CW_Element tmp_elm_for_set_data = Factories.element_factory.get_item_to_fill();
            tmp_elm_for_set_data.re_random(true, 100, true, prefer_elements, prefer_scale);
            return Factories.element_factory.get_next(tmp_elm_for_set_data);
        }
        /// <summary>
        /// 获取A与B的平均
        /// </summary>
        /// <param name="element_a"></param>
        /// <param name="element_b"></param>
        /// <returns></returns>
        public static CW_Element get_middle(CW_Element element_a, CW_Element element_b)
        {
            CW_Element middle = new CW_Element();
            int i;
            for (i = 0; i < middle.base_elements.Length; i++)
            {
                middle.base_elements[i] = (element_a.base_elements[i] + element_b.base_elements[i]) >> 1;
            }
            middle.normalize();
            return middle;
        }
        public CW_Element deepcopy()
        {
            CW_Element copy = new CW_Element();
            for (int i = 0; i < base_elements.Length; i++)
            {
                copy.base_elements[i] = this.base_elements[i];
            }
            copy.type_id = this.type_id;
            return copy;
        }
        public void deepcopy_to(CW_Element element)
        {
            int i;
            for (i = 0; i < base_elements.Length; i++)
            {
                element.base_elements[i] = this.base_elements[i];
            }
            element.type_id = this.type_id;
        }
        /// <summary>
        /// 计算类别，未来将支持自定义算法
        /// </summary>
        public string comp_type()
        {
            __comp_type();
            return this.type_id;
        }
        /// <summary>
        /// 获取该元素组合的类别
        /// </summary>
        /// <returns>类别对应的Asset</returns>
        public ElementAsset get_type()
        {
            if (String.IsNullOrEmpty(this.type_id)) __comp_type();
            return Manager.elements.get(this.type_id);
        }
        /// <summary>
        /// 重新随机
        /// </summary>
        /// <param name="normalize">是否规格化</param>
        /// <param name="normalize_ceil">规格化上界</param>
        /// <param name="comp_type">是否即时确定元素类型</param>
        /// <param name="prefer_elements">偏好元素</param>
        /// <param name="prefer_scale">偏好系数</param>
        public void re_random(bool normalize = true, int normalize_ceil = 100, bool comp_type = true, int[] prefer_elements = null, float prefer_scale = 0f)
        {
            __random_generate(normalize_ceil);
            if (prefer_elements != null && prefer_scale >= 0.01f) __prefer_to(prefer_elements, prefer_scale);
            if (normalize) __normalize(normalize_ceil);
            if (comp_type) __comp_type();
        }
        /// <summary>
        /// 将元素含量规格化
        /// </summary>
        /// <param name="normalize_ceil">规格化上界</param>
        public void normalize(int normalize_ceil = 100)
        {
            this.__normalize(normalize_ceil);
        }
        public BaseStats comp_bonus_stats()
        {
            ElementAsset asset = get_type();
            float promot = asset.promot;
            BaseStats combine_bonus = tmp_stats;
            combine_bonus.clear();
            combine_bonus.mergeStats(asset.base_stats);
            // 添加五元素的加成
            float real_content;
            // 火
            real_content = base_elements[Constants.Core.BASE_TYPE_FIRE] + base_elements[Constants.Core.BASE_TYPE_WOOD] - base_elements[Constants.Core.BASE_TYPE_WATER];
            combine_bonus[S.critical_chance] += real_content * 0.2f * promot;
            combine_bonus[S.mod_crit] += real_content * promot;
            combine_bonus[S.critical_damage_multiplier] += real_content * 1.5f * promot;
            // 土
            real_content = base_elements[Constants.Core.BASE_TYPE_GROUND] + base_elements[Constants.Core.BASE_TYPE_FIRE] - base_elements[Constants.Core.BASE_TYPE_WOOD];
            combine_bonus[S.mod_armor] += real_content * promot;
            combine_bonus[CW_S.mod_spell_armor] += real_content * promot;
            // 金
            real_content = base_elements[Constants.Core.BASE_TYPE_IRON] + base_elements[Constants.Core.BASE_TYPE_GROUND] - base_elements[Constants.Core.BASE_TYPE_FIRE];
            combine_bonus[S.mod_damage] += real_content * 2 * promot;
            // 水
            real_content = base_elements[Constants.Core.BASE_TYPE_WATER] + base_elements[Constants.Core.BASE_TYPE_IRON] - base_elements[Constants.Core.BASE_TYPE_GROUND];
            combine_bonus[CW_S.mod_shield] += real_content * 2f * promot;
            combine_bonus[CW_S.mod_shield_regen] += real_content * promot;
            combine_bonus[S.knockback_reduction] += real_content * promot;
            // 木
            real_content = base_elements[Constants.Core.BASE_TYPE_WOOD] + base_elements[Constants.Core.BASE_TYPE_WATER] - base_elements[Constants.Core.BASE_TYPE_IRON];
            combine_bonus[S.mod_health] += real_content * promot;
            combine_bonus[CW_S.mod_health_regen] += real_content * 1.5f * promot;

            return combine_bonus;
        }
        public void prefer_to(int[] prefer_elements, float scale)
        {
            __prefer_to(prefer_elements, scale);
        }
        private void __comp_type()
        {
            int i, j;
            List<ElementAsset> asset_list = Manager.elements.list;
            int length = asset_list.Count;
            float min_no_similarity = 10000f;
            float tmp_no_similarity;
            for (i = 0; i < length; i++)
            {
                // 如果完全不同，那么tmp为1，完全一致则为0
                // 加上稀有度影响
                tmp_no_similarity = (1 - __get_similarity(asset_list[i].base_elements, this.base_elements)) * asset_list[i].rarity;
                if (tmp_no_similarity < min_no_similarity || (tmp_no_similarity == min_no_similarity && asset_list[i].rarity > Manager.elements.get(this.type_id).rarity))
                {
                    min_no_similarity = tmp_no_similarity;
                    this.type_id = asset_list[i].id;
                }
            }
        }
        private static float __get_similarity(int[] e1, int[] e2)
        {
            int mul_result = 0; int modulus_1 = 0; int modulus_2 = 0;
            for (int j = 0; j < Constants.Core.element_str.Length; j++)
            {
                mul_result += e1[j] * e2[j];
                modulus_1 += e1[j] * e1[j];
                modulus_2 += e2[j] * e2[j];
            }
            //if (modulus_1 * modulus_2 == 0) return 1;
            return (mul_result * mul_result / (float)(modulus_1 * modulus_2));
        }
        public static float get_similarity(CW_Element e1, CW_Element e2)
        {
            return __get_similarity(e1.base_elements, e2.base_elements);
        }
        private void __normalize(int normalize_ceil)
        {
            float co = 0f;
            int i;
            for (i = 0; i < Constants.Core.element_str.Length; i++)
            {
                co += this.base_elements[i];
            }
            co = normalize_ceil / co;
            for (i = 0; i < Constants.Core.element_str.Length; i++)
            {
                this.base_elements[i] = (int)(this.base_elements[i] * co + 0.5f);
                normalize_ceil -= this.base_elements[i];
            }
            if (normalize_ceil > 0)
            {
                i = Toolbox.randomInt(0, Constants.Core.element_str.Length);
                this.base_elements[i] += normalize_ceil;
            }
        }
        private void __random_generate(int ceil = 100)
        {
            for (int i = 0; i < Constants.Core.element_str.Length; i++)
            {
                base_elements[i] = Toolbox.randomInt(0, ceil + 1);
            }
        }
        private void __uniform_generate(int sum = 100)
        {
            int uniform_val = sum / Constants.Core.element_str.Length;
            for (int i = 0; i < Constants.Core.element_str.Length; i++)
            {
                base_elements[i] = uniform_val;
            }
        }
        private void __prefer_to(int[] prefer_elements, float scale)
        {
            int j;
            int delta_abs = 0;
            int origin_total_val = 0;
            int[] delta_vals = new int[Constants.Core.element_str.Length];
            for (j = 0; j < Constants.Core.element_str.Length; j++)
            {
                delta_vals[j] = prefer_elements[j] - this.base_elements[j];
                delta_abs += Math.Abs(delta_vals[j]);
                origin_total_val += this.base_elements[j];
            }
            if (delta_abs == 0) return;
            for (j = 0; j < Constants.Core.element_str.Length; j++)
            {
                //Debug.Log($"Begin:[{j}]:{this.base_elements[j]},delta_val:{delta_vals[j]},scale:{scale},delta_abs:{delta_abs}");
                //this.base_elements[j] += (int)(Math.Sign(delta_vals[j]) * delta_vals[j] * delta_vals[j] * scale / delta_abs);
                this.base_elements[j] += (int)(delta_vals[j] * scale);
                //Debug.Log($"Then:[{j}]:{this.base_elements[j]}");
            }
            this.__normalize(origin_total_val);
        }
        private const int stats_length = 15;
        internal string __to_string()
        {
            StringBuilder string_builder = new StringBuilder();
            int i = 0; int j;
            for (i = 0; i < base_elements.Length; i++)
            {
                string description = LocalizedTextManager.getText("$base_element_" + i + "$");
                string value = base_elements[i] + "%\n";
                j = stats_length - description.Length - value.Length * 2;
                string_builder.Append(description);
                for (; j > 0; j--)
                {
                    string_builder.Append(" ");
                }
                string_builder.Append(value);
            }
            return string_builder.ToString();
        }
        internal void fill_string_builder(StringBuilder description, StringBuilder value)
        {
            description.Clear(); value.Clear();
            for (int i = 0; i < Constants.Core.element_str.Length; i++)
            {
                description.AppendLine(LocalizedTextManager.getText("$base_element_" + i + "$"));
                value.AppendLine(base_elements[i] + "%");
            }
        }
        public void set(int[] base_elements, bool normalize = false, int normalize_ceil = 100, bool comp_type = true)
        {
            for (int i = 0; i < Constants.Core.element_str.Length; i++)
            {
                this.base_elements[i] = base_elements[i];
            }
            if (normalize) __normalize(normalize_ceil);
            if (comp_type) __comp_type();
        }
        public void set(CW_Element item)
        {
            for(int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                base_elements[i] = item.base_elements[i];
            }
            type_id = item.type_id;
        }
        public void set(ActorData data)
        {
            int data_receiver;
            for(int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                data.get(Constants.DataS.element_list[i], out data_receiver, -1);
                this.base_elements[i] = data_receiver;
            }
            data.get(Constants.DataS.element_type_id, out this.type_id, Constants.Core.uniform_type);
        }
        public void clear()
        {
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                base_elements[i] = 20;
            }
            type_id = Constants.Core.uniform_type;
        }
    }
}
