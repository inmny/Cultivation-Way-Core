using System.IO;
using Assets.SimpleZip;
using Cultivation_Way.Save;
using Cultivation_Way.Utils;
using HarmonyLib;
using Newtonsoft.Json;

namespace Cultivation_Way.HarmonySpace;

internal static class H_SaveManager
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.getMapFromPath))]
    public static void get_cw_saved_data(string pMainPath, SavedMap __result)
    {
        if (__result == null) return;
        string dir_path = SaveManager.folderPath(pMainPath);

        var file_path = dir_path + "cw_map.wbox";

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

        CW_SaveManager._data = new SavedDataVerCur();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.loadData))]
    public static void load_cw_data(SaveManager __instance, SavedMap pData)
    {
        CW_SaveManager._data ??= new SavedDataVerCur();
        CW_SaveManager.PatchLoadMethods(__instance, pData, CW_SaveManager._data);
    }

    private static void read_save_data(string file_path, bool is_compressed = true)
    {
        string save_text;
        save_text = is_compressed ? Zip.Decompress(File.ReadAllBytes(file_path)) : File.ReadAllText(file_path);

        CW_SaveManager._data = JsonConvert.DeserializeObject<EmptySavedData>(save_text);
        switch (CW_SaveManager._data.cw_save_version)
        {
            case 1:
                CW_SaveManager._data = GeneralHelper.from_json<SavedDataVer1>(save_text, true);
                break;
            case Constants.Core.save_version:
                CW_SaveManager._data = GeneralHelper.from_json<SavedDataVerCur>(save_text, true);
                break;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.saveMapData))]
    public static void save_cw_data(string pFolder, bool pCompress, SavedMap __result)
    {
        string save_dir = SaveManager.folderPath(pFolder);
        string save_path;
        SavedDataVerCur saved_data = new();
        saved_data.Initialize(__result);
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