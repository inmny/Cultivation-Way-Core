using System;
using System.Collections.Generic;
using System.Linq;

namespace Cultivation_Way.Implementation;

internal static class Buildings
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
        add_ming();
        add_wu();
        fix_bugs();
    }

    private static void add_wu()
    {
        clone_human_buildings(Content_Constants.wu_race);
    }

    private static void add_ming()
    {
        BuildOrder bonfire_ming_order = AssetManager.race_build_orders
            .get(Content_Constants.ming_race).list
            .Find(order => order.id == SB.order_bonfire);
        bonfire_ming_order.id = "bonfire_ming";

        foreach (BuildOrder order in AssetManager.race_build_orders.get(Content_Constants.ming_race).list)
        {
            if (order.requirements_orders.Exists(order_id => order_id == SB.order_bonfire))
            {
                order.requirements_orders.Remove(SB.order_bonfire);
                order.requirements_orders.Add("bonfire_ming");
            }
        }

        clone_human_buildings(Content_Constants.ming_race);

        BuildingAsset bonfire = AssetManager.buildings.clone("bonfire_ming", "bonfire");
        bonfire.race = Content_Constants.ming_race;
        AssetManager.buildings.loadSprites(bonfire);

        AssetManager.buildings.get("tent_ming").fundament = new BuildingFundament(1, 1, 1, 0);
        AssetManager.buildings.get("house_ming_0").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_ming_1").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_ming_2").fundament = new BuildingFundament(3, 3, 4, 0);
        AssetManager.buildings.get("house_ming_3").fundament = new BuildingFundament(4, 4, 6, 0);
        AssetManager.buildings.get("house_ming_4").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("house_ming_5").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("hall_ming_0").fundament = new BuildingFundament(4, 4, 7, 0);
        AssetManager.buildings.get("hall_ming_1").fundament = new BuildingFundament(5, 5, 9, 0);
        AssetManager.buildings.get("hall_ming_2").fundament = new BuildingFundament(8, 8, 14, 0);

        AssetManager.buildings.get("temple_ming").fundament = new BuildingFundament(3, 3, 5, 0);
        AssetManager.buildings.get("barracks_ming").fundament = new BuildingFundament(3, 3, 7, 0);
        AssetManager.buildings.get("windmill_ming_0").fundament = new BuildingFundament(2, 1, 2, 0);
        AssetManager.buildings.get("windmill_ming_1").fundament = new BuildingFundament(2, 2, 2, 0);
        AssetManager.buildings.get("watch_tower_ming").fundament = new BuildingFundament(2, 2, 3, 0);
    }

    private static void fix_bugs()
    {
        // 修复树阻止建筑升级
        foreach (BuildingAsset building in AssetManager.buildings.list)
        {
            if (building.type == "trees") building.priority = -1;
            if (building.kingdom == "nature") building.upgradeLevel -= 1;
            if (building.upgradeLevel >= 0) building.priority = Math.Max(building.priority, building.upgradeLevel);
        }
    }

    private static void add_yao()
    {
        clone_human_buildings(Content_Constants.yao_race);
        AssetManager.buildings.get("windmill_yao_0").fundament = new BuildingFundament(2, 2, 1, 0);
        AssetManager.buildings.get("windmill_yao_1").fundament = new BuildingFundament(3, 3, 1, 0);
    }

    private static void add_eastern_human()
    {
        var race_order = AssetManager.race_build_orders.get(Content_Constants.eastern_human_race);

        clone_human_buildings(Content_Constants.eastern_human_race);

        BuildingAsset bonfire = AssetManager.buildings.clone("bonfire_eastern_human", "bonfire");
        bonfire.smoke = false;
        bonfire.race = Content_Constants.eastern_human_race;
        
        BuildingAsset smelt_mill = AssetManager.buildings.clone(CW_SB.eh_smelt_mill, SB.bonfire);
        smelt_mill.race = Content_Constants.eastern_human_race;
        smelt_mill.draw_light_size = 2;
        smelt_mill.type = CW_SB.smelt_mill;
        smelt_mill.fundament = new BuildingFundament(3, 3, 5, 0);
        smelt_mill.smokeOffset = new(3, 5);
        smelt_mill.max_houses = 0;
        smelt_mill.build_place_single = true;
        var smelt_mill_order = race_order.addBuilding(CW_SB.order_smelt_mill);
        smelt_mill_order.requirements_orders = new();
        smelt_mill_order.requirements_orders.Add(SB.order_hall_2);
        
        AssetManager.buildings.add(smelt_mill);
        
        AssetManager.buildings.loadSprites(smelt_mill);
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