using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowSpellLibrary : AutoLayoutWindow<WindowSpellLibrary>
{
    public static WindowSpellLibrary Instance { get; }

    protected override void Init()
    {
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}