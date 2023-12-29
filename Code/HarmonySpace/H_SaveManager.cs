using System.IO;
using Assets.SimpleZip;
using Cultivation_Way.Save;
using HarmonyLib;
using Newtonsoft.Json;
namespace Cultivation_Way.HarmonySpace;

internal static class H_SaveManager
{
    private static AbstractSavedData _saved_data;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.getMapFromPath))]
    public static void get_cw_saved_data(string pMainPath, SavedMap __result)
    {
        _saved_data = null;
        if (__result == null) return;
        string dir_path = SaveManager.folderPath(pMainPath);

        string file_path;

        file_path = dir_path + "cw_map.wbox";

        if (File.Exists(file_path))
        {
            SaveManager.fileInfo(file_path);

            read_save_data(file_path);
            return;
        }

        file_path = dir_path + "cw_map.wbax";
        if (File.Exists(file_path))
        {
            SaveManager.fileInfo(file_path);

            read_save_data(file_path, false);
            return;
        }

        _saved_data = new SavedDataVerCur();
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.loadData))]
    public static bool load_cw_data(SaveManager __instance, SavedMap pData)
    {
        if (_saved_data != null)
        {
            _saved_data.load_to_world(__instance, pData);
            return false;
        }
        return true;
    }

    private static void read_save_data(string file_path, bool is_compressed = true)
    {
        string save_text;
        if (is_compressed)
        {
            save_text = Zip.Decompress(File.ReadAllBytes(file_path));
        }
        else
        {
            save_text = File.ReadAllText(file_path);
        }

        _saved_data = JsonConvert.DeserializeObject<AbstractSavedData>(save_text);
        switch (_saved_data.cw_save_version)
        {
            case 1:
                _saved_data = JsonConvert.DeserializeObject<SavedDataVer1>(save_text);
                break;
            case Constants.Core.save_version:
                _saved_data = JsonConvert.DeserializeObject<SavedDataVerCur>(save_text);
                break;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.saveMapData))]
    public static void save_cw_data(string pFolder, bool pCompress)
    {
        string save_dir = SaveManager.folderPath(pFolder);
        string save_path;
        SavedDataVerCur saved_data = new();
        saved_data.initialize();
        if (pCompress)
        {
            save_path = save_dir + "cw_map.wbox";
            saved_data.save_to_as_zip(save_path);
        }
        else
        {
            save_path = save_dir + "cw_map.wbax";
            saved_data.save_to_as_json(save_path);
        }
    }
}