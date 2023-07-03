namespace Cultivation_Way.Content;

internal static class Kingdoms
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
    }

    private static void add_yao()
    {
    }

    private static void add_eastern_human()
    {
        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.eastern_human_race,
            civ = true
        });
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.eastern_human_race);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.eastern_human_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);

        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.nomad_kingdom_prefix + Content_Constants.eastern_human_race,
            nomads = true,
            mobs = true
        });
        AssetManager.kingdoms.t.default_kingdom_color = new ColorAsset("#BACADD", "#BACADD", "#BACADD");
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.eastern_human_race);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.eastern_human_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);
    }
}