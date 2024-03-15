using Cultivation_Way.Abstract;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal sealed class Elements : ExtendedLibrary<ElementAsset, Elements>
{
    internal Elements()
    {
        Add(new ElementAsset(Constants.Core.mod_prefix + "water",        100, 0,   0, 0, 0, 1f, 4.5f, null, 2));
        Add(new ElementAsset(Constants.Core.mod_prefix + "fire",         0,   100, 0, 0, 0, 1f, 4.5f, null, 2));
        Add(new ElementAsset(Constants.Core.mod_prefix + "wood",         0,   0,   100, 0, 0, 1f, 4.5f, null, 2));
        Add(new ElementAsset(Constants.Core.mod_prefix + "iron",         0,   0,   0, 100, 0, 1f, 4.5f, null, 2));
        Add(new ElementAsset(Constants.Core.mod_prefix + "ground",       0,   0,   0, 0, 100, 1f, 4.5f, null, 2));
        Add(new ElementAsset(Constants.Core.mod_prefix + "water_fire",   50,  50,  0, 0, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "water_wood",   50,  0,   50, 0, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "water_iron",   50,  0,   0, 50, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "water_ground", 50,  0,   0, 0, 50, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "fire_wood",    0,   50,  50, 0, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "fire_iron",    0,   50,  0, 50, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "fire_ground",  0,   50,  0, 0, 50, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "wood_iron",    0,   0,   50, 50, 0, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "wood_ground",  0,   0,   50, 0, 50, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "iron_ground",  0,   0,   0, 50, 50, 1f, 2.5f, null, 0.5f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "common",       20,  20,  20, 20, 20, 1f, 3f));
        Add(new ElementAsset(Constants.Core.mod_prefix + "uniform",      20,  20,  20, 20, 20, 20f, 100f, null, -0.5f));
    }
}