using UnityEngine;

namespace Cultivation_Way.General.AboutUI;

public static class MapLayerUtils
{
    public static T CreateNewLayer<T>(string pId) where T : MapLayer
    {
        GameObject layer_obj = new(pId, typeof(SpriteRenderer), typeof(T));
        layer_obj.transform.SetParent(GameObject.Find("[layer]World Map Edges").transform.parent);

        T layer = layer_obj.GetComponent<T>();

        World.world.mapLayers.Add(layer);

        return layer;
    }
}