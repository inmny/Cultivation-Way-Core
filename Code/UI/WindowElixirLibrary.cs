using System;
using System.Collections.Generic;
using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowElixirLibrary : AutoLayoutWindow<WindowElixirLibrary>, ILibraryWindow<ElixirAsset>
{
    public List<ElixirAsset> Data { get; set; }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    protected override void Init()
    {
    }
}