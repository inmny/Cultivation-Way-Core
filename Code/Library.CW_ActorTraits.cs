namespace Cultivation_Way.Library;

internal static class CW_ActorTraits
{
    public static ActorTrait positive_creature;
    public static ActorTrait negative_creature;

    public static void init()
    {
        ActorTrait trait;

        trait = new ActorTrait();
        trait.id = Constants.Core.mod_prefix + "positive_creature";
        trait.only_active_on_era_flag = true;
        trait.era_active_moon = false;
        trait.era_active_night = false;
        trait.base_stats[S.mod_speed] = -0.2f;
        trait.base_stats[S.mod_attack_speed] = -0.2f;
        trait.base_stats[S.mod_crit] = -0.2f;
        trait.path_icon = "ui/icons/iconYang";
        positive_creature = trait;
        add(trait);


        trait = new ActorTrait();
        trait.id = Constants.Core.mod_prefix + "negative_creature";
        trait.only_active_on_era_flag = true;
        trait.era_active_moon = false;
        trait.era_active_night = false;
        trait.base_stats[S.mod_health] = -0.3f;
        trait.base_stats[S.mod_damage] = -0.3f;
        trait.base_stats[S.mod_speed] = -0.15f;
        trait.base_stats[S.mod_attack_speed] = -0.1f;
        trait.base_stats[S.mod_crit] = -0.2f;
        trait.base_stats[S.mod_armor] = -0.4f;
        trait.path_icon = "ui/icons/iconYin";
        negative_creature = trait;
        add(trait);
    }

    private static void add(ActorTrait trait)
    {
        AssetManager.traits.add(trait);
    }
}