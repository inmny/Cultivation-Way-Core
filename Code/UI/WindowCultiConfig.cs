using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

/// <summary>
///     这个窗口是生物修炼的配置窗口（设置修炼体系，等级等）
/// </summary>
public class WindowCultiConfig : AutoLayoutWindow<WindowCultiConfig>
{
    protected override void Init()
    {
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}