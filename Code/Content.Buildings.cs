using System.Collections.Generic;
using System.Linq;

namespace Cultivation_Way.Content;

internal static class Buildings
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
    }

    private static void add_yao()
    {
        clone_human_buildings(Content_Constants.yao_race);
        AssetManager.buildings.get("windmill_yao_0").fundament = new BuildingFundament(2, 2, 1, 0);
        AssetManager.buildings.get("windmill_yao_1").fundament = new BuildingFundament(3, 3, 1, 0);
    }

    private static void add_eastern_human()
    {
        BuildOrder bonfire_eastern_human_order = AssetManager.race_build_orders
            .get(Content_Constants.eastern_human_race).list
            .Find(order => order.id == SB.order_bonfire);
        bonfire_eastern_human_order.id = "bonfire_eastern_human";

        foreach (BuildOrder order in AssetManager.race_build_orders.get(Content_Constants.eastern_human_race).list)
        {
            if (order.requirements_orders.Exists(order_id => order_id == SB.order_bonfire))
            {
                order.requirements_orders.Remove(SB.order_bonfire);
                order.requirements_orders.Add("bonfire_eastern_human");
            }
        }

        clone_human_buildings(Content_Constants.eastern_human_race);

        BuildingAsset bonfire = AssetManager.buildings.clone("bonfire_eastern_human", "bonfire");
        bonfire.smoke = false;
        bonfire.race = Content_Constants.eastern_human_race;
        AssetManager.buildings.loadSprites(bonfire);

        AssetManager.buildings.get("tent_eastern_human").fundament = new BuildingFundament(1, 1, 1, 0);
        AssetManager.buildings.get("house_eastern_human_0").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_eastern_human_1").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_eastern_human_2").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_eastern_human_3").fundament = new BuildingFundament(4, 4, 6, 0);
        AssetManager.buildings.get("house_eastern_human_4").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("house_eastern_human_5").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("hall_eastern_human_0").fundament = new BuildingFundament(4, 4, 7, 0);
        AssetManager.buildings.get("hall_eastern_human_1").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("hall_eastern_human_2").fundament = new BuildingFundament(8, 8, 14, 0);

        AssetManager.buildings.get("temple_eastern_human").fundament = new BuildingFundament(3, 3, 5, 0);
        AssetManager.buildings.get("barracks_eastern_human").fundament = new BuildingFundament(3, 3, 7, 0);
        AssetManager.buildings.get("windmill_eastern_human_0").fundament = new BuildingFundament(2, 1, 2, 0);
        AssetManager.buildings.get("windmill_eastern_human_1").fundament = new BuildingFundament(2, 2, 2, 0);
        AssetManager.buildings.get("watch_tower_eastern_human").fundament = new BuildingFundament(2, 2, 3, 0);
    }

    private static void clone_human_buildings(string race)
    {
        List<BuildingAsset> human_buildings = AssetManager.buildings.list
            .Where(building => building.race == "human").ToList();

        foreach (BuildingAsset building in human_buildings)
        {
            BuildingAsset new_building =
                AssetManager.buildings.clone(building.id.Replace(SK.human, race),
                    building.id);

            new_building.race = race;

            if (building.canBeUpgraded)
                new_building.upgradeTo = new_building.upgradeTo.Replace(SK.human, race);

            if (!string.IsNullOrEmpty(new_building.upgradedFrom))
                new_building.upgradedFrom =
                    new_building.upgradedFrom.Replace(SK.human, race);

            AssetManager.buildings.loadSprites(new_building);
        }
    }
}