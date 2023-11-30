using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way.Utils;

public static class MapIconHelper
{
    private class CommonIconRecord
    {
        public Sprite icon;
        public float left_time;
        public Vector3 pos;
    }
    private static readonly List<CommonIconRecord> common_icons = new();
    private static readonly Stack<CommonIconRecord> common_icon_pool = new();
    public static void AddCommonIcon(Sprite pIcon, Vector3 pPos)
    {
        CommonIconRecord new_one = null;
        if (common_icon_pool.Count > 0)
        {
            new_one = common_icon_pool.Pop();
        }
        else
        {
            new_one = new CommonIconRecord();
        }
        new_one.icon = pIcon;
        new_one.left_time = Constants.Others.common_map_icon_duration;
        new_one.pos = pPos;
        common_icons.Add(new_one);
    }
    internal static void _draw_common_icon(MapIconAsset pAsset)
    {
        for (int i = 0; i < common_icons.Count; i++)
        {
            common_icons[i].left_time -= Time.deltaTime;
            
            if(common_icons[i].left_time <= 0)
            {
                common_icon_pool.Push(common_icons[i]);
                common_icons.RemoveAt(i);
                i--;
                continue;
            }
            float percent = common_icons[i].left_time / Constants.Others.common_map_icon_duration;
            float height = iTween.easeOutCubic(0f, 1f, percent);
            
            var pos = common_icons[i].pos;
            pos.y += 1 + 2 * height;

            float scale = Math.Max(0.5f, height);

            var map_mark = MapIconLibrary.drawMark(pAsset, pos, pModScale: scale);
            map_mark.setSprite(common_icons[i].icon);
            map_mark.transform.eulerAngles = new(0, 0, height * 360f);

            Color color = new Color(1, 1, 1, percent);
            map_mark.setColor(ref color);
        }
    }
}