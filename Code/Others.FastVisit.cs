using System.Collections.Generic;
using Cultivation_Way.Core;
using UnityEngine;

namespace Cultivation_Way.Others;

internal static class FastVisit
{
    private static readonly string[] actor_prefab_paths =
    {
        "actors/p_unit", "actors/p_dragon", "actors/p_tornado", "actors/p_ufo", "actors/p_boat", "actors/p_godFinger",
        "actors/p_zombie_dragon", "actors/p_crabzilla"
    };

    private static readonly Dictionary<string, GameObject> actor_prefabs = new();
    private static Sprite square_frame;
    private static Sprite square_frame_only;
    private static Sprite window_bar;
    private static Sprite window_bar_90;
    private static Sprite window_big_close;

    public static void init()
    {
        get_actor_prefabs();
    }

    public static GameObject get_actor_prefab(string path)
    {
        return actor_prefabs.ContainsKey(path) ? actor_prefabs[path] : null;
    }

    public static Sprite get_square_frame()
    {
        if (square_frame != null) return square_frame;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/square_frame");
        square_frame = Sprite.Create(_orig.texture, _orig.rect, new Vector2(24, 24f), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(7, 7, 7, 7));
        square_frame.name = "square_frame";
        return square_frame;
    }

    public static Sprite get_square_frame_only()
    {
        if (square_frame_only != null) return square_frame_only;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/square_frame_only");
        square_frame_only = Sprite.Create(_orig.texture, _orig.rect, new Vector2(24, 24f), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(7, 7, 7, 7));
        square_frame_only.name = "square_frame_only";
        return square_frame_only;
    }

    public static Sprite get_window_bar()
    {
        if (window_bar != null) return window_bar;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowBar");
        window_bar = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(5, 6, 5, 6));
        window_bar.name = "window_bar";
        return window_bar;
    }

    public static Sprite get_window_bar_90()
    {
        if (window_bar_90 != null) return window_bar_90;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowBar_90");
        window_bar_90 = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(6, 5, 6, 5));
        window_bar_90.name = "window_bar_90";
        return window_bar_90;
    }

    public static Sprite get_window_big_close()
    {
        if (window_big_close != null) return window_big_close;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowBigClose");
        window_big_close = Sprite.Create(_orig.texture, _orig.rect, new Vector2(20, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(0, 4, 0, 6));
        window_big_close.name = "windowBigClose";
        return window_big_close;
    }

    private static void get_actor_prefabs()
    {
        for (int i = 0; i < actor_prefab_paths.Length; i++)
        {
            GameObject old_prefab = Resources.Load<GameObject>(actor_prefab_paths[i]);

            if (old_prefab == null)
            {
                Logger.Warn($"Empty prefab {actor_prefab_paths[i]}");
                continue;
            }

            GameObject new_prefab = Object.Instantiate(old_prefab, CW_Core.instance.transform);
            new_prefab.SetActive(false);
            Object.Destroy(new_prefab.GetComponent<Actor>());
            new_prefab.AddComponent<CW_Actor>();
            actor_prefabs[actor_prefab_paths[i]] = new_prefab;
        }
    }
}