using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way.Others
{
    internal static class FastVisit
    {
        private static string[] actor_prefab_paths = new string[] { "actors/p_unit", "actors/p_dragon", "actors/p_tornado", "actors/p_ufo", "actors/p_boat", "actors/p_boulder", "actors/p_godFinger", "actors/p_zombie_dragon", "actors/p_crabzilla", "actors/p_santa" };
        private static Dictionary<string, GameObject> actor_prefabs = new Dictionary<string, GameObject>();
        internal static Font font_STLiti = Font.CreateDynamicFontFromOSFont("STLiti", 18);
        internal static Font font_STKaiti = Font.CreateDynamicFontFromOSFont("STKaiti", 18);
        internal static Transform transformUnits;
        internal static Transform transformCreatures;
        public static void init()
        {
            get_actor_prefabs();
        }
        public static GameObject get_actor_prefab(string path)
        {
            return actor_prefabs.ContainsKey(path) ? actor_prefabs[path] : null;
        }


        private static void get_actor_prefabs()
        {
            for (int i = 0; i < actor_prefab_paths.Length; i++)
            {
                GameObject newPrefab = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(actor_prefab_paths[i]), Core.CW_Core.instance.transform);
                newPrefab.SetActive(false);
                UnityEngine.Object.Destroy(newPrefab.GetComponent<Actor>());
                newPrefab.AddComponent<Core.CW_Actor>();
                actor_prefabs[actor_prefab_paths[i]] = newPrefab;
            }
        }
    }
}
