﻿using System.Collections.Generic;
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
    private static readonly Dictionary<string, Material> color_materials = new();
    private static Sprite square_frame;
    private static Sprite square_frame_only;
    private static Sprite window_bar;
    private static Sprite window_bar_90;
    private static Sprite window_big_close;
    private static Sprite window_inner_sliced;
    private static Sprite window_library;
    private static Sprite red_button;
    private static Sprite button_1;
    private static Sprite info_bg;

    private static readonly HashSet<string> actor_prefab_blacklist = new();

    public static void init()
    {
        get_actor_prefabs();
        add_color_materials();
    }

    private static void add_color_materials()
    {
        Material mat = new(LibraryMaterials.instance.matDamaged.shader);
        mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matDamaged);
        color_materials.Add("red", mat);

        mat = new Material(LibraryMaterials.instance.matHighLighted.shader);
        mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matHighLighted);
        color_materials.Add("white", mat);

        mat = new Material(LibraryMaterials.instance.matHighLighted.shader);
        mat.CopyPropertiesFromMaterial(LibraryMaterials.instance.matHighLighted);
        mat.color = new Color(0.2617f, 0.2617f, 0.2617f, 1f);
        color_materials.Add("gray", mat);
    }

    public static Material get_color_material(string color_id)
    {
        color_materials.TryGetValue(color_id, out Material ret);

        if (ret == null)
        {
            ret = new Material(LibraryMaterials.instance.matHighLighted.shader);
            ret.CopyPropertiesFromMaterial(LibraryMaterials.instance.matHighLighted);
            ret.color = Colors.GetColor(color_id);
            color_materials[color_id] = ret;
        }

        return ret;
    }

    public static GameObject get_actor_prefab(string path)
    {
        return actor_prefabs.TryGetValue(path, out GameObject prefab) ? prefab : try_generate_actor_prefab(path);
    }

    public static Sprite get_info_bg()
    {
        if (info_bg != null) return info_bg;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/city_info_bg");
        info_bg = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(5, 5, 5, 5));
        info_bg.name = "info_bg";
        return info_bg;
    }

    public static Sprite get_button_1()
    {
        if (button_1 != null) return button_1;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/button1");
        button_1 = Sprite.Create(_orig.texture, _orig.rect, new Vector2(16, 16), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(6, 6, 6, 6));
        button_1.name = "button_1";
        return button_1;
    }

    public static Sprite get_red_button()
    {
        if (red_button != null) return red_button;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/red_button");
        red_button = Sprite.Create(_orig.texture, _orig.rect, new Vector2(40.5f, 10), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(3, 3, 3, 3));
        red_button.name = "red_button";
        return red_button;
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

    public static Sprite get_window_inner_sliced()
    {
        if (window_inner_sliced != null) return window_inner_sliced;
        Sprite _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowInnerSliced");
        window_inner_sliced = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(5, 6, 5, 6));
        window_inner_sliced.name = "windowInnerSliced";
        return window_inner_sliced;
    }

    public static Sprite get_window_library()
    {
        if (window_library != null) return window_library;
        var _orig = SpriteTextureLoader.getSprite("ui/cw_window/windowLibrary");
        window_library = Sprite.Create(_orig.texture, _orig.rect, new Vector2(0, 0), _orig.pixelsPerUnit, 0,
            SpriteMeshType.Tight, new Vector4(18, 56, 18, 28));
        window_library.name = "window_library";
        return window_library;
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

            GameObject new_prefab = Object.Instantiate(old_prefab, CW_Core.actor_prefab_library);
            new_prefab.SetActive(false);
            Object.Destroy(new_prefab.GetComponent<Actor>());
            new_prefab.AddComponent<CW_Actor>();
            actor_prefabs[actor_prefab_paths[i]] = new_prefab;
        }
    }

    private static GameObject try_generate_actor_prefab(string pPath)
    {
        if (actor_prefab_blacklist.Contains(pPath)) return null;
        GameObject old_prefab = Resources.Load<GameObject>(pPath);

        if (old_prefab == null)
        {
            CW_Core.LogWarning($"Empty prefab {pPath}");
            actor_prefab_blacklist.Add(pPath);
            return null;
        }

        GameObject new_prefab = Object.Instantiate(old_prefab, CW_Core.actor_prefab_library);
        new_prefab.SetActive(false);
        Object.Destroy(new_prefab.GetComponent<Actor>());
        new_prefab.AddComponent<CW_Actor>();
        actor_prefabs[pPath] = new_prefab;
        return new_prefab;
    }

    internal static void patch_actor_prefab(string path, GameObject prefab)
    {
        prefab.SetActive(false);
        prefab.transform.SetParent(CW_Core.actor_prefab_library);
        actor_prefabs[path] = prefab;
    }
}