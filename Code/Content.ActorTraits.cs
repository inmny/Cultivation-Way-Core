namespace Cultivation_Way.Content;

internal static class ActorTraits
{
    public static void init()
    {
        ActorTrait trait;

        trait = new ActorTrait
        {
            id = Constants.Core.mod_prefix + "curse_immune",
            opposite = "cursed",
            group_id = TraitGroup.body,
            path_icon = "ui/icons/iconCurseImmune"
        };
        add(trait);
    }

    private static void add(ActorTrait trait)
    {
        AssetManager.traits.add(trait);
    }
}