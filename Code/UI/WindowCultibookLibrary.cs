using System;
using System.Collections.Generic;
using Cultivation_Way.Library;
using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowCultibookLibrary : AutoLayoutWindow<WindowCultibookLibrary>, ILibraryWindow<Cultibook>
{
    public static WindowCultibookLibrary Instance { get; }
    public List<Cultibook> Data { get; set; }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    public void PushData(Cultibook pData)
    {
        throw new NotImplementedException();
    }

    protected override void Init()
    {
    }

    public override void OnNormalDisable()
    {
        CW_Core.mod_state.is_awarding = false;
    }
}