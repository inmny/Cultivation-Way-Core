using System.Runtime.CompilerServices;
using Cultivation_Way.Utils;

namespace Cultivation_Way.Extension;

public static class BaseSystemDataTools
{
    public static void Clear(this BaseSystemData pData)
    {
        pData.custom_data_bool?.dict?.Clear();
        pData.custom_data_float?.dict?.Clear();
        pData.custom_data_int?.dict?.Clear();
        pData.custom_data_string?.dict?.Clear();
        pData.custom_data_flags?.Clear();
    }

    /// <summary>
    ///     以key为key, 将pObject JSON序列化后写入data
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteObj<T>(this BaseSystemData pData, string pKey, T pObject, bool pPrivateMembersIncluded = false)
    {
        if(pObject == null) pData.removeString(pKey);
        pData.set(pKey, GeneralHelper.to_json(pObject, pPrivateMembersIncluded));
    }

    /// <summary>
    ///     以key为key, 从data中读取JSON, 并反序列化为T, 若不存在则会返回default(T)
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ReadObj<T>(this BaseSystemData pData, string pKey, bool pPrivateMembersIncluded = false)
    {
        pData.get(pKey, out string obj_str);

        if (string.IsNullOrEmpty(obj_str)) return default;

        return GeneralHelper.from_json<T>(obj_str, pPrivateMembersIncluded);
    }
}