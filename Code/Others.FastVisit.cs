using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way.Others
{
    internal static class FastVisit
    {
        private static string[] actor_prefab_paths = new string[] { "actors/p_unit", "actors/p_dragon", "actors/p_tornado", "actors/p_ufo", "actors/p_boat", "actors/p_godFinger", "actors/p_zombie_dragon", "actors/p_crabzilla" };
        private readonly static Dictionary<string, GameObject> actor_prefabs = new();
        internal static Font font_STLiti = Font.CreateDynamicFontFromOSFont("STLiti", 18);
        internal static Font font_STKaiti = Font.CreateDynamicFontFromOSFont("STKaiti", 18);
        public static void init()
        {
            get_actor_prefabs();
        }
        public static GameObject get_actor_prefab(string path)
        {
            return actor_prefabs.ContainsKey(path) ? actor_prefabs[path] : null;
        }
        private static Sprite square_frame;
        public static Sprite get_square_frame()
        {
            if (square_frame != null) return square_frame;
            Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/square_frame");
            square_frame = Sprite.Create(_orig.texture, _orig.rect, new Vector2(24, 24f), _orig.pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(7, 7, 7, 7));
            square_frame.name = "square_frame";
            return square_frame;
        }
        private static Sprite square_frame_only;
        public static Sprite get_square_frame_only()
        {
            if (square_frame_only != null) return square_frame_only;
            Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/square_frame_only");
            square_frame_only = Sprite.Create(_orig.texture, _orig.rect, new Vector2(24, 24f), _orig.pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(7, 7, 7, 7));
            square_frame_only.name = "square_frame_only";
            return square_frame_only;
        }
        private static Sprite window_bar;
        public static Sprite get_window_bar()
        {
            if (window_bar != null) return window_bar;
            Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowBar");
            window_bar = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(5, 6, 5, 6));
            window_bar.name = "window_bar";
            return window_bar;
        }
        private static Sprite window_bar_90;
        public static Sprite get_window_bar_90()
        {
            if (window_bar_90 != null) return window_bar_90;
            Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowBar_90");
            window_bar_90 = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0, SpriteMeshType.Tight, new Vector4(6, 5, 6, 5));
            window_bar_90.name = "window_bar_90";
            return window_bar_90;
        }
        private static void get_actor_prefabs()
        {
            for (int i = 0; i < actor_prefab_paths.Length; i++)
            {
                GameObject old_prefab = Resources.Load<GameObject>(actor_prefab_paths[i]);

                if (old_prefab == null) { Logger.Warn($"Empty prefab {actor_prefab_paths[i]}"); continue; }

                GameObject new_prefab = UnityEngine.Object.Instantiate<GameObject>(old_prefab, CW_Core.instance.transform);
                new_prefab.SetActive(false);
                UnityEngine.Object.Destroy(new_prefab.GetComponent<Actor>());
                new_prefab.AddComponent<Core.CW_Actor>();
                actor_prefabs[actor_prefab_paths[i]] = new_prefab;
            }
        }
    }
}
