using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Helper
    {
        private static string[] actor_prefab_paths = new string[] { "actors/p_unit", "actors/p_dragon", "actors/p_tornado", "actors/p_ufo", "actors/p_boat", "actors/p_boulder", "actors/p_godFinger", "actors/p_zombie_dragon", "actors/p_crabzilla", "actors/p_santa" };
        private static Dictionary<string, GameObject> actor_prefabs = new Dictionary<string, GameObject>();
        private static Dictionary<string, Material> color_materials = new Dictionary<string, Material>();
        internal static Font font_STLiti = Font.CreateDynamicFontFromOSFont("STLiti", 18);
        internal static Font font_STKaiti = Font.CreateDynamicFontFromOSFont("STKaiti", 18);
        private static GameObject building_prefab;
        internal static Transform transformUnits;
        internal static Transform transformCreatures;
        internal static Transform transformBuildings;
        internal static Transform transformTrees;
        internal static GameStatsData game_stats_data;
        internal static IslandsCalculator islands_calculator;
        internal static ZoneCalculator zone_calculator;
        internal static SpriteRenderer zone_cal_sprRnd;
        internal static List<SpriteGroupSystem<GroupSpriteObject>> list_systems;
        internal static List<string> yaos;
        internal static List<BaseSimObject> temp_map_objects;
        internal static void init()
        {
            get_actor_prefabs();
            get_building_prefabs();
            add_color_materials();
            transformUnits = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformUnits") as Transform;
            transformCreatures = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformCreatures") as Transform;
            transformBuildings = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformBuildings") as Transform;
            transformTrees = transformBuildings.Find("Trees");
            game_stats_data = ReflectionUtility.Reflection.GetField(typeof(GameStats), MapBox.instance.gameStats, "data") as GameStatsData;
            islands_calculator = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "islandsCalculator") as IslandsCalculator;
            list_systems = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "list_systems") as List<SpriteGroupSystem<GroupSpriteObject>>;
            zone_calculator = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "zoneCalculator") as ZoneCalculator;
            zone_cal_sprRnd = ReflectionUtility.Reflection.GetField(typeof(ZoneCalculator), zone_calculator, "sprRnd") as SpriteRenderer;
            yaos = new List<string>();
            temp_map_objects = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
        }

        private static void add_color_materials()
        {
            Material mat = new Material(LibraryMaterials.instance.matDamaged.shader);
            mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matDamaged);
            color_materials.Add("red", mat);

            mat = new Material(LibraryMaterials.instance.matHighLighted.shader);
            mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matHighLighted);
            color_materials.Add("white", mat);

            mat = new Material(LibraryMaterials.instance.matHighLighted.shader);
            mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matHighLighted);
            mat.color = new Color(0.2617f, 0.2617f, 0.2617f, 1f);
            color_materials.Add("grey", mat);
        }

        public static Material get_color_material(string id)
        {
            Material ret;
            if (color_materials.TryGetValue(id, out ret)) return ret;
            Debug.LogError(string.Format("No found Color Material '{0}'", id));
            return null;
        }
        internal static CW_ActorData get_load_cw_data(ActorData origin_data)
        {
            CW_ActorData ret = null;
            switch (ModState.instance.load_object_reason)
            {
                case Load_Object_Reason.SPAWN:
                    ret = Harmony.W_Harmony_City.tmp_data;
                    Harmony.W_Harmony_City.tmp_data = null;
                    break;
                case Load_Object_Reason.LOAD_SAVES:
                    ret = CW_SaveManager.tmp_loaded_actor_data;
                    CW_SaveManager.tmp_loaded_actor_data = null;
                    break;
                default:
                    break;
            }

            return ret;
        }
        internal static CW_BuildingData get_load_cw_data(BuildingData origin_data)
        {
            CW_BuildingData ret = null;
            switch (ModState.instance.load_object_reason)
            {
                case Load_Object_Reason.SPAWN:
                    throw new NotImplementedException();
                    break;
                case Load_Object_Reason.LOAD_SAVES:
                    ret = CW_SaveManager.tmp_loaded_building_data;
                    CW_SaveManager.tmp_loaded_building_data = null;
                    break;
                default:
                    break;
            }

            return ret;
        }
        internal static GameObject get_actor_prefab(string path)
        {
            return actor_prefabs.ContainsKey(path) ? actor_prefabs[path] : null;
        }
        internal static GameObject get_building_prefab()
        {
            return building_prefab;
        }
        private static void get_building_prefabs()
        {
            building_prefab = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("buildings/p_building"), Main.instance.transform);
            building_prefab.SetActive(false);
            UnityEngine.Object.Destroy(building_prefab.GetComponent<Building>());
            building_prefab.AddComponent<CW_Building>();
        }
        private static void get_actor_prefabs()
        {
            for (int i = 0; i < actor_prefab_paths.Length; i++)
            {
                GameObject newPrefab = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(actor_prefab_paths[i]), Main.instance.transform);
                newPrefab.SetActive(false);
                UnityEngine.Object.Destroy(newPrefab.GetComponent<Actor>());
                newPrefab.AddComponent<CW_Actor>();
                actor_prefabs[actor_prefab_paths[i]] = newPrefab;
            }
        }
    }
}
