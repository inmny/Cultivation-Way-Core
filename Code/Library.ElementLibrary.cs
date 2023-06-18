using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class ElementAsset : Asset
    {
        /// <summary>
        /// 加成系数(>=0.01f)，在不同场景有不同的计算方法，请查阅手册对应内容
        /// </summary>
        public float promot { get; }
        /// <summary>
        /// 稀有度(>=1.0f)，影响归纳至此类的难度，值越大难度越大，具体计算方法见CW_Element.comp_type
        /// </summary>
        public float rarity { get; }
        /// <summary>
        /// 基础元素组成，长度必须为CW_Constants.base_element_types
        /// </summary>
        public int[] base_elements { get; }
        /// <summary>
        /// 标签，同时为Library中的idx
        /// </summary>
        public int tag { get; internal set; }
        internal uint _tag;
        /// <summary>
        /// 元素组合提供的属性加成
        /// </summary>
        public BaseStats base_stats;
        internal ElementAsset(string id, int[] base_elements, float promot = 1.0f, float rarity = 1.0f, BaseStats base_stats = null)
        {
            if (base_elements == null || base_elements.Length != Constants.Core.element_type_nr || promot < 0.01f || rarity < 0.99f) throw new Exception(String.Format("Init arguments error: {0},{1},{2}", base_elements == null ? -1 : base_elements.Length, promot, rarity));
            this.promot = promot;
            this.rarity = rarity;
            this.base_elements = base_elements;
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                if (base_elements[i] < 0 || base_elements[i] > 100) throw new Exception(String.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
            }
            this.id = id;
            this.base_stats = base_stats ?? new BaseStats();
        }
        internal ElementAsset(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f, float rarity = 1.0f, BaseStats base_stats = null, float mod_culti_velo = 0)
        {
            if (promot < 0.01f || rarity < 0.99f) throw new Exception(String.Format("Init arguments error: {0},{1}", promot, rarity));
            this.promot = promot;
            this.rarity = rarity;
            this.base_elements = new int[Constants.Core.element_type_nr] { water, fire, wood, iron, ground };
            for (int i = 0; i < Constants.Core.element_type_nr; i++)
            {
                if (base_elements[i] < 0 || base_elements[i] > 100) throw new Exception(String.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
            }
            this.id = id;
            this.base_stats = base_stats ?? new BaseStats();
            this.base_stats[Constants.CW_S.mod_cultivelo] = mod_culti_velo;
        }
    }
    public class ElementLibrary:CW_Library<ElementAsset>
    {
        public override void init()
        {
            base.init();
            add(new ElementAsset("CW_water", 100, 0, 0, 0, 0, 1f, 4.5f, null, 200));
            add(new ElementAsset("CW_fire", 0, 100, 0, 0, 0, 1f, 4.5f, null, 200));
            add(new ElementAsset("CW_wood", 0, 0, 100, 0, 0, 1f, 4.5f, null, 200));
            add(new ElementAsset("CW_iron", 0, 0, 0, 100, 0, 1f, 4.5f, null, 200));
            add(new ElementAsset("CW_ground", 0, 0, 0, 0, 100, 1f, 4.5f, null, 200));
            add(new ElementAsset("CW_water_fire", 50, 50, 0, 0, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_water_wood", 50, 0, 50, 0, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_water_iron", 50, 0, 0, 50, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_water_ground", 50, 0, 0, 0, 50, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_fire_wood", 0, 50, 50, 0, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_fire_iron", 0, 50, 0, 50, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_fire_ground", 0, 50, 0, 0, 50, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_wood_iron", 0, 0, 50, 50, 0, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_wood_ground", 0, 0, 50, 0, 50, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_iron_ground", 0, 0, 0, 50, 50, 1f, 2.5f, null, 50));
            add(new ElementAsset("CW_common", 20, 20, 20, 20, 20, 1f, 3f, null, 0));
            add(new ElementAsset("CW_uniform", 20, 20, 20, 20, 20, 20f, 100f, null, -50));
        }
        /// <summary>
        /// 添加新元素组合，所有元素含量应当不超过100
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="water">水</param>
        /// <param name="fire">火</param>
        /// <param name="wood">木</param>
        /// <param name="iron">金</param>
        /// <param name="ground">土</param>
        /// <param name="promot">加成系数</param>
        /// <param name="rarity">稀有度</param>
        /// <returns>创建的Asset</returns>
        public ElementAsset add_element(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f, float rarity = 1.0f)
        {
            return add(new ElementAsset(id, water, fire, wood, iron, ground, promot, rarity));
        }
        /// <summary>
        /// 添加新元素组合，所有元素含量应当不超过100
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="base_elements">元素组合，水火木金土</param>
        /// <param name="promot">加成系数</param>
        /// <param name="rarity">稀有度</param>
        /// <returns>创建的Asset</returns>
        public ElementAsset add_element(string id, int[] base_elements, float promot = 1.0f, float rarity = 1.0f)
        {
            return add(new ElementAsset(id, base_elements, promot, rarity));
        }
        public override void post_init()
        {
            int i;
            for (i = 0; i < this.list.Count; i++)
            {
                list[i].tag = i;
                list[i]._tag = 1u << i;
            }
        }
    }
}
