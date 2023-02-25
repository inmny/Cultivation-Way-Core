using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    [Serializable]
    public class CW_Element
    {
        public const int BASE_TYPE_WATER = 0;
        public const int BASE_TYPE_FIRE = 1;
        public const int BASE_TYPE_WOOD = 2;
        public const int BASE_TYPE_IRON = 3;
        public const int BASE_TYPE_GROUND = 4;
        private const string UNIFORM_TYPE = "CW_uniform";
        public int[] base_elements = new int[Others.CW_Constants.base_element_types];
        public string type_id;
        private static CW_BaseStats tmp_stats = new CW_BaseStats();
        internal CW_Element()
        {

        }
        /// <summary>
        /// 创建CW_Element对象
        /// </summary>
        /// <param name="base_elements">给定元素组合</param>
        /// <param name="normalize">是否规格化</param>
        /// <param name="normalize_ceil">规格化上界</param>
        /// <param name="comp_type">是否即时确定元素类型</param>
        public CW_Element(int[] base_elements, bool normalize = false, int normalize_ceil = 100, bool comp_type = true)
        {
            for(int i = 0; i < Others.CW_Constants.base_element_types; i++)
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
        public CW_Element(bool random_generate = true, bool normalize = true, int normalize_ceil = 100, bool comp_type = true, int[] prefer_elements = null, float prefer_scale = 0f)
        {
            base_elements = new int[Others.CW_Constants.base_element_types];
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
                if (comp_type) type_id = UNIFORM_TYPE;
            }
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
            for(int i = 0; i < base_elements.Length; i++)
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
        public Library.CW_Asset_Element get_type()
        {
            if (String.IsNullOrEmpty(this.type_id)) __comp_type();
            return Library.CW_Library_Manager.instance.elements.get(this.type_id);
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
        public CW_BaseStats comp_bonus_stats()
        {
            Library.CW_Asset_Element asset = get_type();
            float promot = asset.promot;
            CW_BaseStats combine_bonus = tmp_stats;
            combine_bonus.clear();
            combine_bonus.addStats(asset.bonus_stats);
            // 添加五元素的加成
            float real_content;
            // 火
            real_content = base_elements[BASE_TYPE_FIRE] + base_elements[BASE_TYPE_WOOD] - base_elements[BASE_TYPE_WATER];
            combine_bonus.base_stats.crit += real_content * 0.2f * promot;
            combine_bonus.base_stats.mod_crit += real_content * promot;
            combine_bonus.base_stats.damageCritMod += real_content * 1.5f * promot;
            // 土
            real_content = base_elements[BASE_TYPE_GROUND] + base_elements[BASE_TYPE_FIRE] - base_elements[BASE_TYPE_WOOD];
            combine_bonus.base_stats.mod_armor += real_content * promot;
            combine_bonus.mod_spell_armor += real_content * promot;
            // 金
            real_content = base_elements[BASE_TYPE_IRON] + base_elements[BASE_TYPE_GROUND] - base_elements[BASE_TYPE_FIRE];
            combine_bonus.base_stats.mod_damage += real_content * 2 * promot;
            combine_bonus.mod_anti_armor += real_content * 0.5f * promot;
            combine_bonus.mod_anti_spell_armor += real_content * 0.5f * promot;
            // 水
            real_content = base_elements[BASE_TYPE_WATER] + base_elements[BASE_TYPE_IRON] - base_elements[BASE_TYPE_GROUND];
            combine_bonus.mod_anti_crit += real_content * 1.2f * promot;  
            combine_bonus.anti_crit_damage += real_content * 1.7f * promot;
            combine_bonus.mod_wakan += real_content * 2f * promot;
            combine_bonus.base_stats.knockbackReduction += real_content * promot;
            // 木
            real_content = base_elements[BASE_TYPE_WOOD] + base_elements[BASE_TYPE_WATER] - base_elements[BASE_TYPE_IRON];
            combine_bonus.base_stats.mod_health += real_content * promot;
            combine_bonus.mod_health_regen += real_content * 1.5f * promot;
            combine_bonus.mod_wakan_regen += real_content * promot;

            return combine_bonus;
        }
        public void prefer_to(int[] prefer_elements, float scale)
        {
            __prefer_to(prefer_elements, scale);
        }
        /**
        private void __comp_type()
        {
            int i, j;
            List<Library.CW_Asset_Element> asset_list = Library.CW_Library_Manager.instance.elements.list;
            float m_1 = 0;
            for (i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                m_1 += this.base_elements[i];
            }
            m_1 /= Others.CW_Constants.base_element_types;
            float m_2;
            float min_val = 1000000f;
            float cur_val;
            for (i = 0; i < asset_list.Count; i++)
            {
                m_2 = 0; cur_val = 0;
                for(j = 0; j < Others.CW_Constants.base_element_types; j++)
                {
                    m_2 += asset_list[i].base_elements[j];
                }
                m_2 /= Others.CW_Constants.base_element_types;
                for(j=0;j<Others.CW_Constants.base_element_types; j++)
                {
                    cur_val += (this.base_elements[j] - m_1) * (asset_list[i].base_elements[j] - m_2);
                }
                if(cur_val >)
            }
        }
        */
        private void __comp_type()
        {
            int i, j;
            List<Library.CW_Asset_Element> asset_list = Library.CW_Library_Manager.instance.elements.list;
            int length = asset_list.Count;
            float min_no_similarity = 10000f;
            float tmp_no_similarity;
            for (i = 0; i < length; i++)
            {
                // 如果完全不同，那么tmp为1，完全一致则为0
                // 加上稀有度影响
                tmp_no_similarity = (1 - __get_similarity(asset_list[i].base_elements, this.base_elements)) * asset_list[i].rarity;
                if(tmp_no_similarity < min_no_similarity || (tmp_no_similarity==min_no_similarity && asset_list[i].rarity > Library.CW_Library_Manager.instance.elements.get(this.type_id).rarity))
                {
                    min_no_similarity = tmp_no_similarity;
                    this.type_id = asset_list[i].id;
                }
            }
        }
        private static float __get_similarity(int[] e1, int[] e2)
        {
            int mul_result = 0; int modulus_1 = 0; int modulus_2 = 0;
            for (int j = 0; j < Others.CW_Constants.base_element_types; j++)
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
            for(i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                co += this.base_elements[i];
            }
            co = normalize_ceil / co;
            for (i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                this.base_elements[i] = (int)(this.base_elements[i] * co+0.5f);
                normalize_ceil -= this.base_elements[i];
            }
            if(normalize_ceil > 0)
            {
                i = Toolbox.randomInt(0, Others.CW_Constants.base_element_types);
                this.base_elements[i] += normalize_ceil;
            }
        }
        private void __random_generate(int ceil = 100)
        {
            for(int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                base_elements[i] = Toolbox.randomInt(0, ceil + 1);
            }
        }
        private void __uniform_generate(int sum = 100)
        {
            int uniform_val = sum / Others.CW_Constants.base_element_types;
            for (int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                base_elements[i] = uniform_val;
            }
        }
        private void __prefer_to(int[] prefer_elements, float scale)
        {
            int j;
            int delta_abs = 0;
            int origin_total_val = 0;
            int[] delta_vals = new int[Others.CW_Constants.base_element_types];
            for (j = 0; j < Others.CW_Constants.base_element_types; j++)
            {
                delta_vals[j] = prefer_elements[j] - this.base_elements[j];
                delta_abs += Math.Abs(delta_vals[j]);
                origin_total_val += this.base_elements[j];
            }
            if (delta_abs == 0) return;
            for (j = 0; j < Others.CW_Constants.base_element_types; j++)
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
                j = stats_length - description.Length - value.Length*2;
                string_builder.Append(description);
                for(;j > 0; j--)
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
            for (int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                description.AppendLine(LocalizedTextManager.getText("$base_element_" + i + "$"));
                value.AppendLine(base_elements[i] + "%");
            }
        }
    }
}
