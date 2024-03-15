using System.Collections.Generic;
using System.Reflection;

namespace Cultivation_Way.Abstract;

public abstract class ExtendedLibrary<TAsset, TLibrary>
    where TAsset : Asset where TLibrary : ExtendedLibrary<TAsset, TLibrary>
{
    private readonly Dictionary<string, FieldInfo> _fields      = new();
    public           List<TAsset>                  added_assets = new();
    protected        AssetLibrary<TAsset>          cached_library;
    protected        TAsset                        t;

    protected ExtendedLibrary()
    {
        I = this as TLibrary;

        var fields = GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (FieldInfo field in fields)
            if (typeof(TAsset).IsAssignableFrom(field.FieldType))
                _fields.Add(field.Name, field);
    }

    public static TLibrary I { get; private set; }

    private void _init()
    {
        cached_library ??= (AssetLibrary<TAsset>)AssetManager.instance.list.Find(l => l is AssetLibrary<TAsset>);
    }

    protected virtual TAsset Get(string pId)
    {
        _init();
        return cached_library.get(pId);
    }

    protected virtual TAsset Add(TAsset pObj)
    {
        _init();
        t = pObj;
        added_assets.Add(pObj);

        _set_field(pObj);

        return cached_library.add(pObj);
    }

    protected virtual TAsset Clone(string pNew, string pFrom)
    {
        _init();

        TAsset pObj = cached_library.clone(pNew, pFrom);

        _set_field(pObj);
        return pObj;
    }

    protected virtual void Replace(TAsset pNew)
    {
        _init();
        if (cached_library.dict.TryGetValue(pNew.id, out TAsset old)) cached_library.list.Remove(old);

        cached_library.list.Add(pNew);
        cached_library.dict[pNew.id] = pNew;

        _set_field(pNew);
    }

    private void _set_field(TAsset pObj)
    {
        if (_fields.TryGetValue(pObj.id, out FieldInfo field)) field.SetValue(null, pObj);
    }
}