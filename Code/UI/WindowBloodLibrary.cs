using System;
using System.Collections.Generic;
using Cultivation_Way.Library;
using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowBloodLibrary : AutoLayoutWindow<WindowBloodLibrary>, ILibraryWindow<BloodNodeAsset>
{
    public static WindowBloodLibrary Instance { get; }
    public List<BloodNodeAsset> Data { get; set; }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    public void PushData(BloodNodeAsset pData)
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