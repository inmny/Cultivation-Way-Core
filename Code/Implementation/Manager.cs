namespace Cultivation_Way.Implementation;

internal static class Manager
{
    public static void init()
    {
        _ = new CW_SR();
        _ = new Actors();
        _ = new ActorTraits();
        _ = new Achievements();
        Bloods.init();
        Cultibooks.init();
        _ = new Cultisyses();
        _ = new Elements();
        Energies.init();
        Items.init();
        Kingdoms.init();
        StatusEffects.init();
        _ = new Spells();
        Terraforms.init();
        ImprovedSpells.init();
        SpecialSpells.init();
        Races.init();
        Resources.init();
        // 由于BuildingOrderAsset初始化在Races.init之中，所以必须在Races.init之后
        _ = new Buildings();
        WorldLaws.init();
        HarmonySpace.Manager.init();
    }
}