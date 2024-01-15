using UnityEngine;

namespace Cultivation_Way.Extension;

public static class ObjectPoolGenericMonoExtension
{
    public static void InactiveObj<T>(this ObjectPoolGenericMono<T> pPool, T obj) where T : MonoBehaviour
    {
        if (pPool._elements_inactive.Contains(obj) || !pPool._elements_total.Contains(obj))
        {
            CW_Core.LogWarning(
                $"[ObjectGenericMonoPoolExtension] ReturnToPool: {obj.name} is already in inactive pool({pPool._elements_inactive.Contains(obj)}) or not in pool({!pPool._elements_total.Contains(obj)})");
            return;
        }

        obj.gameObject.SetActive(false);
        pPool._elements_inactive.Push(obj);
    }
}