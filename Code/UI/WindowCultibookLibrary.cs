using System;
using System.Collections.Generic;
using Cultivation_Way.Library;

namespace Cultivation_Way.UI;

public class WindowCultibookLibrary : AbstractWindow<WindowCultibookLibrary>, ILibraryWindow<Cultibook>
{
    public List<Cultibook> Data { get; set; }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }

    internal static void init()
    {
        base_init(Constants.Core.cultibook_library_window);
    }
}