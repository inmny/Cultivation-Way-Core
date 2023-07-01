namespace Cultivation_Way.Content;

internal static class Manager
{
    public static void init()
    {
        Cultisyses.init();
        Elements.init();
        Energies.init();
        StatusEffects.init();
        Spells.init();

        HarmonySpace.Manager.init();
    }
}