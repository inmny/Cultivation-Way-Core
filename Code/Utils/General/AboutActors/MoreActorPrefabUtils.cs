using Cultivation_Way.Core;
using Cultivation_Way.Others;
using UnityEngine;

namespace Cultivation_Way.Utils.General.AboutActors;

public static class MoreActorPrefabUtils
{
    public static void PatchNewActorPrefab(string pPath, GameObject pPrefab)
    {
        if (string.IsNullOrEmpty(pPath) || pPrefab == null)
        {
            CW_Core.LogWarning($"Failed to patch {pPath} because it doesn't have any content.");
            return;
        }

        if (pPrefab.GetComponent<CW_Actor>() == null)
        {
            CW_Core.LogWarning($"Failed to patch {pPath} because it doesn't have CW_Actor component.");
            return;
        }

        FastVisit.patch_actor_prefab(pPath, pPrefab);
    }

    public static GameObject GetActorPrefab(string pPath)
    {
        return FastVisit.get_actor_prefab(pPath);
    }
}