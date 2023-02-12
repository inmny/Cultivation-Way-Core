using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.SimpleZip;
using HarmonyLib;
using Newtonsoft.Json;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_Save
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AutoSaveManager), "autoSave")]
        public static bool autoSave()
        {
            return !W_Content_WorldLaws.is_no_auto_save();
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "saveWorldToDirectory")]
        public static bool saveWorldToDirectory_Prefix(string pFolder, bool pCompress, bool pCheckFolder, ref SavedMap __result)
        {
            pFolder = Reflection.CallStaticMethod(typeof(SaveManager), "folderPath", pFolder) as string;
            if (!pCheckFolder || !Directory.Exists(pFolder)) Directory.CreateDirectory(pFolder);
            Reflection.CallStaticMethod(typeof(SaveManager), "saveImagePreview", pFolder);
            __result = CW_SaveManager.save_to(pFolder, false);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadData")]
        public static bool loadData_Prefix(SavedMap pData)
        {
            CW_SaveManager.load_origin_data(pData);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadWorld", typeof(string), typeof(bool))]
        public static bool loadWorld_Prefix(SaveManager __instance, string pPath, bool pLoadWorkshop = false)
        {
            SmoothLoader.prepare();
            if (pLoadWorkshop)
            {
                CW_SaveManager.load_origin_data(JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(File.ReadAllBytes(Reflection.CallStaticMethod(typeof(SaveManager), "folderPath", SaveManager.currentWorkshopMapData.main_path) + "map.wbox"))));
                return false;
            }
            string main_path = Reflection.CallStaticMethod(typeof(SaveManager), "folderPath", pPath) as string;
            if (!Directory.Exists(main_path))
            {
                Debug.Log("Directory does not exist : " + main_path);
                return false;
            }
            // 存在修仙的数据
            string cw_save_path = main_path + "cw_map.wbox";
            if (File.Exists(cw_save_path))
            {
                Reflection.CallStaticMethod(typeof(SaveManager), "fileInfo", cw_save_path);
                CW_SaveManager.load_cw_data(JsonConvert.DeserializeObject<CW_SavedGameData>(Zip.Decompress(File.ReadAllBytes(cw_save_path))));
                return false;
            }
            cw_save_path = main_path + "cw_map.wbax";
            if (File.Exists(cw_save_path))
            {
                Reflection.CallStaticMethod(typeof(SaveManager),"fileInfo", cw_save_path);
                CW_SaveManager.load_cw_data(JsonConvert.DeserializeObject<CW_SavedGameData>(File.ReadAllText(cw_save_path)));
                return false;
            }
            // 仅有原版数据
            cw_save_path = main_path + "map.wbox";
            if (File.Exists(cw_save_path))
            {
                Reflection.CallStaticMethod(typeof(SaveManager),"fileInfo", cw_save_path);
                CW_SaveManager.load_origin_data(JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(File.ReadAllBytes(cw_save_path))));
                return false;
            }
            cw_save_path = main_path + "map.wbax";
            if (File.Exists(cw_save_path))
            {
                Reflection.CallStaticMethod(typeof(SaveManager),"fileInfo", cw_save_path);
                CW_SaveManager.load_origin_data(JsonConvert.DeserializeObject<SavedMap>(File.ReadAllText(cw_save_path)));
                return false;
            }
            cw_save_path = main_path + "map.json";
            if (File.Exists(cw_save_path))
            {
                Reflection.CallStaticMethod(typeof(SaveManager),"fileInfo", cw_save_path);
                CW_SaveManager.load_origin_data(JsonConvert.DeserializeObject<SavedMap>(Zip.Decompress(File.ReadAllText(cw_save_path))));
                return false;
            }
            Debug.LogError(string.Format("No found save files under '{0}'", main_path));
            return false;
        }
    }
}
