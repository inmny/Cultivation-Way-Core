using System.Runtime.CompilerServices;
using Cultivation_Way.Core;

namespace Cultivation_Way.Extension;

public static class ActorTools
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CW_Actor CW(this Actor pActor)
    {
        return pActor as CW_Actor;
    }
}