using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Constants;

internal static class Paths
{
    public static readonly string DataPath = Path.Combine(Application.persistentDataPath, Core.mod_name);
}
