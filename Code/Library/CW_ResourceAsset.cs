using Cultivation_Way.Core;
using Cultivation_Way.Library;

namespace Cultivation_Way;

public class CW_ResourceAsset : Asset
{
    /// <summary>
    ///     五行属性
    /// </summary>
    public CW_Element element;

    /// <summary>
    ///     原版的资源信息
    /// </summary>
    public ResourceAsset vanllia_asset;

    /// <summary>
    ///     阴阳属性, 0为阴, 1为阳
    /// </summary>
    public float yin_yang;

    internal CW_ResourceAsset(ResourceAsset vanllia_asset)
    {
        id = vanllia_asset.id;
        this.vanllia_asset = vanllia_asset;
        element = new CW_Element(new[] { 20, 20, 20, 20, 20 }, comp_type: false);
        yin_yang = 0.5f;
    }
}

public class CW_ResourceLibrary : CW_Library<CW_ResourceAsset>
{
    public override void init()
    {
        base.init();
        foreach (var resource in AssetManager.resources.dict.Values)
        {
            add(new CW_ResourceAsset(resource));
        }
    }

    public override void post_init()
    {
        base.post_init();
        foreach (CW_ResourceAsset resource_asset in list)
        {
            resource_asset.element.ComputeType();
        }
    }

    public override CW_ResourceAsset get(string pID)
    {
        if (dict.TryGetValue(pID, out CW_ResourceAsset value)) return value;
        if (AssetManager.resources.dict.ContainsKey(pID))
        {
            add(new CW_ResourceAsset(AssetManager.resources.get(pID)));
        }

        return base.get(pID);
    }
}