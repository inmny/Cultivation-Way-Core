using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Element : Asset
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
        public int tag { get; }
        /// <summary>
        /// 元素组合提供的属性加成
        /// </summary>
        public CW_BaseStats bonus_stats;
        internal CW_Asset_Element(string id, int[] base_elements, float promot = 1.0f, float rarity = 1.0f, CW_BaseStats bonus_stats = null)
        {
            if (base_elements == null || base_elements.Length != Others.CW_Constants.base_element_types || promot < 0.01f || rarity < 0.99f) throw new Exception(String.Format("Init arguments error: {0},{1},{2}", base_elements==null?-1:base_elements.Length, promot, rarity));
            this.promot = promot;
            this.rarity = rarity;
            this.base_elements = base_elements;
            for(int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                if (base_elements[i] < 0 || base_elements[i] > 100) throw new Exception(String.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
            }
            this.id = id;
            if (bonus_stats == null) this.bonus_stats = bonus_stats;
            else this.bonus_stats = new CW_BaseStats();
        }
        internal CW_Asset_Element(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f, float rarity = 1.0f, CW_BaseStats bonus_stats = null)
        {
            if (promot < 0.01f || rarity < 0.99f) throw new Exception(String.Format("Init arguments error: {0},{1}", promot, rarity));
            this.promot = promot;
            this.rarity = rarity;
            this.base_elements = new int[Others.CW_Constants.base_element_types] { water, fire, wood, iron, ground};
            for (int i = 0; i < Others.CW_Constants.base_element_types; i++)
            {
                if (base_elements[i] < 0 || base_elements[i] > 100) throw new Exception(String.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
            }
            this.id = id;
            if (bonus_stats == null) this.bonus_stats = bonus_stats;
            else this.bonus_stats = new CW_BaseStats();
        }
    }
    public class CW_Library_Element:AssetLibrary<CW_Asset_Element>
    {
        public override void init()
        {
            base.init();
            add(new CW_Asset_Element("CW_water", 100, 0, 0, 0, 0, 1f, 6f));
            add(new CW_Asset_Element("CW_fire", 0, 100, 0, 0, 0, 1f, 6f));
            add(new CW_Asset_Element("CW_wood", 0, 0, 100, 0, 0, 1f, 6f));
            add(new CW_Asset_Element("CW_iron", 0, 0, 0, 100, 0, 1f, 6f));
            add(new CW_Asset_Element("CW_ground", 0, 0, 0, 0, 100, 1f, 6f));
            add(new CW_Asset_Element("CW_water_fire", 50, 50, 0, 0, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_water_wood", 50, 0, 50, 0, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_water_iron", 50, 0, 0, 50, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_water_ground", 50, 0, 0, 0, 50, 1f, 3f));
            add(new CW_Asset_Element("CW_fire_wood", 0, 50, 50, 0, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_fire_iron", 0, 50, 0, 50, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_fire_ground", 0, 50, 0, 0, 50, 1f, 3f));
            add(new CW_Asset_Element("CW_wood_iron", 0, 0, 50, 50, 0, 1f, 3f));
            add(new CW_Asset_Element("CW_wood_ground", 0, 0, 50, 0, 50, 1f, 3f));
            add(new CW_Asset_Element("CW_iron_ground", 0, 0, 0, 50, 50, 1f, 3f));
            add(new CW_Asset_Element("CW_common", 20, 20, 20, 20, 20, 1f, 1f));
            add(new CW_Asset_Element("CW_uniform", 20, 20, 20, 20, 20, 10f, 100f));
            
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
        public CW_Asset_Element add_element(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f, float rarity = 1.0f)
        {
            return add(new CW_Asset_Element(id, water, fire, wood, iron, ground, promot, rarity));
        }
        /// <summary>
        /// 添加新元素组合，所有元素含量应当不超过100
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="base_elements">元素组合，水火木金土</param>
        /// <param name="promot">加成系数</param>
        /// <param name="rarity">稀有度</param>
        /// <returns>创建的Asset</returns>
        public CW_Asset_Element add_element(string id, int[] base_elements, float promot = 1.0f, float rarity = 1.0f)
        {
            return add(new CW_Asset_Element(id, base_elements, promot, rarity));
        }
        internal void register()
        {

        }
    }
}
