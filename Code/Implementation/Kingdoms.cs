namespace Cultivation_Way.Implementation;

internal static class Kingdoms
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
        add_ming();
        add_wu();
    }

    private static void add_wu()
    {
        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.wu_race,
            civ = true
        });
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.wu_race);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.wu_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);

        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.nomad_kingdom_prefix + Content_Constants.wu_race,
            nomads = true,
            mobs = true
        });
        AssetManager.kingdoms.t.default_kingdom_color = new ColorAsset("#BACADD", "#BACADD", "#BACADD");
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.wu_race);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.wu_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);
    }

    private static void add_ming()
    {
        AssetManager.kingdoms.get(SK.undead).addFriendlyTag(Content_Constants.ming_race);

        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.ming_race,
            civ = true
        });
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.ming_race);
        AssetManager.kingdoms.t.addTag(SK.undead);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.ming_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addFriendlyTag(SK.undead);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.eastern_human_race);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);

        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.nomad_kingdom_prefix + Content_Constants.ming_race,
            nomads = true,
            mobs = true
        });
        AssetManager.kingdoms.t.default_kingdom_color = new ColorAsset("#BACADD", "#BACADD", "#BACADD");
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.ming_race);
        AssetManager.kingdoms.t.addTag(SK.undead);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.ming_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addFriendlyTag(SK.undead);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.eastern_human_race);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);
    }

    private static void add_yao()
    {
        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.yao_race,
            civ = true
        });
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.yao_race);
        AssetManager.kingdoms.t.addTag(SK.nature_creature);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.yao_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addFriendlyTag(SK.nature_creature);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.eastern_human_race);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);

        AssetManager.kingdoms.add(new KingdomAsset
        {
            id = Content_Constants.nomad_kingdom_prefix + Content_Constants.yao_race,
            nomads = true,
            mobs = true
        });
        AssetManager.kingdoms.t.default_kingdom_color = new ColorAsset("#BACADD", "#BACADD", "#BACADD");
        AssetManager.kingdoms.t.addTag(SK.civ);
        AssetManager.kingdoms.t.addTag(Content_Constants.yao_race);
        AssetManager.kingdoms.t.addTag(SK.nature_creature);
        AssetManager.kingdoms.t.addFriendlyTag(Content_Constants.yao_race);
        AssetManager.kingdoms.t.addFriendlyTag(SK.neutral);
        AssetManager.kingdoms.t.addFriendlyTag(SK.good);
        AssetManager.kingdoms.t.addFriendlyTag(SK.nature_creature);
        AssetManager.kingdoms.t.addEnemyTag(SK.bandits);
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.eastern_human_race);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);
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
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.yao_race);
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
        AssetManager.kingdoms.t.addEnemyTag(Content_Constants.yao_race);
        World.world.kingdoms.newHiddenKingdom(AssetManager.kingdoms.t);
    }
}