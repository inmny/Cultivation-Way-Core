﻿namespace Cultivation_Way.Content;

internal static class Manager
{
    public static void init()
    {
        Cultisys.init();
        Element.init();
        Energy.init();
        StatusEffect.init();
        Spell.init();

        HarmonySpace.Manager.init();
    }
}