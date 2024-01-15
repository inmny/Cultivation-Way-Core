using System;
using System.Collections.Generic;
using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowElixirLibrary : AutoLayoutWindow<WindowElixirLibrary>, ILibraryWindow<ElixirAsset>
{
    public static WindowElixirLibrary Instance { get; }
    public List<ElixirAsset> Data { get; set; }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    public void PushData(ElixirAsset pData)
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