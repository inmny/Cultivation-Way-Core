using NeoModLoader.api;

namespace Cultivation_Way.Addon;

public abstract class CW_Addon : BasicMod<CW_Addon>
{
    internal bool initialized;

    private void Awake()
    {
        initialized = true;
    }
}