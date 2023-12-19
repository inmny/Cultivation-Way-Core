using NeoModLoader.api;

namespace Cultivation_Way.Addon;

public abstract class CW_Addon<T> : BasicMod<T> where T : CW_Addon<T>
{
    internal bool initialized;

    private void Awake()
    {
        initialized = true;
    }

    protected override void OnModLoad()
    {
        CW_Core.mod_state.addons.Add(this);
    }
}