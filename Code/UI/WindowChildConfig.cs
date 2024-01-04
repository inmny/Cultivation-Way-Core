using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowChildConfig : AutoLayoutWindow<WindowChildConfig>
{
    protected override void Init()
    {
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}