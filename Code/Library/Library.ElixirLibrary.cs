using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Others;

namespace Cultivation_Way;

public class ElixirAsset : Asset
{
    /// <summary>
    ///     阴阳属性, 0为阴, 1为阳
    /// </summary>
    public float yin_yang;

    /// <summary>
    ///     五行属性
    /// </summary>
    public CW_Element element;

    /// <summary>
    ///     丹药效果
    /// </summary>
    public ElixirAction action;
}

public class ElixirLibrary : CW_Library<ElixirAsset>
{
}