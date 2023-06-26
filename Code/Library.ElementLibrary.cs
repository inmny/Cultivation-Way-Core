using System;
using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;

public class ElementAsset : Asset
{
    internal uint _tag;

    /// <summary>
    ///     元素组合提供的属性加成
    /// </summary>
    public BaseStats base_stats;

    internal ElementAsset(string id, int[] base_elements, float promot = 1.0f, float rarity = 1.0f,
        BaseStats base_stats = null)
    {
        if (base_elements == null || base_elements.Length != Constants.Core.element_type_nr || promot < 0.01f ||
            rarity < 0.99f)
            throw new Exception(string.Format("Init arguments error: {0},{1},{2}",
                base_elements == null ? -1 : base_elements.Length, promot, rarity));
        this.promot = promot;
        this.rarity = rarity;
        this.base_elements = base_elements;
        for (int i = 0; i < Constants.Core.element_type_nr; i++)
        {
            if (base_elements[i] < 0 || base_elements[i] > 100)
                throw new Exception(string.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
        }

        this.id = id;
        this.base_stats = base_stats ?? new BaseStats();
    }

    internal ElementAsset(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f,
        float rarity = 1.0f, BaseStats base_stats = null, float mod_culti_velo = 0)
    {
        if (promot < 0.01f || rarity < 0.99f)
            throw new Exception(string.Format("Init arguments error: {0},{1}", promot, rarity));
        this.promot = promot;
        this.rarity = rarity;
        base_elements = new int[Constants.Core.element_type_nr] { water, fire, wood, iron, ground };
        for (int i = 0; i < Constants.Core.element_type_nr; i++)
        {
            if (base_elements[i] < 0 || base_elements[i] > 100)
                throw new Exception(string.Format("Error Base Elements at {0}, value {1}", i, base_elements[i]));
        }

        this.id = id;
        this.base_stats = base_stats ?? new BaseStats();
        this.base_stats[CW_S.mod_cultivelo] = mod_culti_velo;
    }

    /// <summary>
    ///     加成系数(>=0.01f)，在不同场景有不同的计算方法，请查阅手册对应内容
    /// </summary>
    public float promot { get; }

    /// <summary>
    ///     稀有度(>=1.0f)，影响归纳至此类的难度，值越大难度越大，具体计算方法见CW_Element.comp_type
    /// </summary>
    public float rarity { get; }

    /// <summary>
    ///     基础元素组成，长度必须为CW_Constants.base_element_types
    /// </summary>
    public int[] base_elements { get; }

    /// <summary>
    ///     标签，同时为Library中的idx
    /// </summary>
    public int tag { get; internal set; }
}

public class ElementLibrary : CW_Library<ElementAsset>
{
    /// <summary>
    ///     添加新元素组合，所有元素含量应当不超过100
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
    public ElementAsset add_element(string id, int water, int fire, int wood, int iron, int ground, float promot = 1.0f,
        float rarity = 1.0f)
    {
        return add(new ElementAsset(id, water, fire, wood, iron, ground, promot, rarity));
    }

    /// <summary>
    ///     添加新元素组合，所有元素含量应当不超过100
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
        for (i = 0; i < list.Count; i++)
        {
            list[i].tag = i;
            list[i]._tag = 1u << i;
        }
    }
}