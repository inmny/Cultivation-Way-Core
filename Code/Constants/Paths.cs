using System.IO;
using UnityEngine;

namespace Cultivation_Way.Constants;

internal static class Paths
{
    public static readonly string DataPath             = Path.Combine(Application.persistentDataPath, Core.mod_name);
    public static readonly string ItemLibraryPath      = Path.Combine(DataPath, "item_library.json");
    public static readonly string CultibookLibraryPath = Path.Combine(DataPath, "cultibook_library.json");
    public static readonly string BloodLibraryPath     = Path.Combine(DataPath, "blood_library.json");
    public static readonly string BloodNodeLibraryPath = Path.Combine(DataPath, "blood_node_library.json");
}