using System;
using System.Collections.Generic;
using NeoModLoader.General.UI.Window;

namespace Cultivation_Way.UI;

public class WindowActorLibrary : AutoLayoutWindow<WindowActorLibrary>, ILibraryWindow<ActorData>
{
    public List<ActorData> Data { get; set; }

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