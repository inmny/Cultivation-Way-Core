using Cultivation_Way.Core;

namespace Cultivation_Way.Implementation;

internal static class Resources
{
    public static void init()
    {
        CW_ResourceAsset asset;
        asset = Library.Manager.resources.get(SR.common_metals);
        asset.element = new CW_Element(new[] { 20, 20, 20, 20, 20 }, comp_type: false);

        asset = Library.Manager.resources.get(SR.silver);
        asset.element = new CW_Element(new[] { 35, 10, 10, 35, 10 }, comp_type: false);

        asset = Library.Manager.resources.get(SR.mythril);
        asset.element = new CW_Element(new[] { 25, 25, 5, 40, 5 }, comp_type: false);

        asset = Library.Manager.resources.get(SR.adamantine);
        asset.element = new CW_Element(new[] { 0, 30, 0, 40, 30 }, comp_type: false);
    }
}