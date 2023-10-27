namespace Cultivation_Way.Implementation;

internal static class Manager
{
    public static void init()
    {
        Actors.init();
        ActorTraits.init();
        Achievements.init();
        Bloods.init();
        Cultibooks.init();
        Cultisyses.init();
        Elements.init();
        Energies.init();
        Kingdoms.init();
        StatusEffects.init();
        Spells.init();
        Races.init();
        // 由于BuildingOrderAsset初始化在Races.init之中，所以必须在Races.init之后
        Buildings.init();
        WorldLaws.init();
        HarmonySpace.Manager.init();
    }
}