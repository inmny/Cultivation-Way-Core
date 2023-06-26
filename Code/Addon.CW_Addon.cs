using ModDeclaration;
using UnityEngine;

namespace Cultivation_Way.Addon;

public class CW_Addon : MonoBehaviour
{
    internal bool initialized = false;
    private bool loaded;
    public Info mod_info;

    internal void Awake()
    {
        if (loaded) return;
        loaded = true;
    }
}