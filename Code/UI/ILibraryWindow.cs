using System.Collections.Generic;

namespace Cultivation_Way.UI;

public interface ILibraryWindow<TData> where TData : class
{
    public List<TData> Data { get; set; }

    public void SaveData();
    public void LoadData();
}