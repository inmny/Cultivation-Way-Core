using System;
using System.Collections.Generic;
using System.Linq;
using Cultivation_Way.Abstract;
using UnityEngine;

namespace Cultivation_Way.Implementation;

internal sealed class Buildings : ExtendedLibrary<BuildingAsset, Buildings>
{
    internal Buildings()
    {
        add_eastern_human();
        add_yao();
        add_ming();
        add_wu();
        fix_bugs();
    }

    private void add_wu()
    {
        clone_human_buildings(Content_Constants.wu_race);
    }

    private void add_ming()
    {
        clone_human_buildings(Content_Constants.ming_race);

        BuildingAsset bonfire = AssetManager.buildings.clone("bonfire_ming", "bonfire");
        bonfire.race = Content_Constants.ming_race;
        AssetManager.buildings.loadSprites(bonfire);

        AssetManager.buildings.get("tent_ming").fundament = new BuildingFundament(1,        1, 1,  0);
        AssetManager.buildings.get("house_ming_0").fundament = new BuildingFundament(3,     3, 4,  0);
        AssetManager.buildings.get("house_ming_1").fundament = new BuildingFundament(3,     3, 4,  0);
        AssetManager.buildings.get("house_ming_2").fundament = new BuildingFundament(3,     3, 4,  0);
        AssetManager.buildings.get("house_ming_3").fundament = new BuildingFundament(4,     4, 6,  0);
        AssetManager.buildings.get("house_ming_4").fundament = new BuildingFundament(5,     5, 9,  0);
        AssetManager.buildings.get("house_ming_5").fundament = new BuildingFundament(5,     5, 9,  0);
        AssetManager.buildings.get("hall_ming_0").fundament = new BuildingFundament(4,      4, 7,  0);
        AssetManager.buildings.get("hall_ming_1").fundament = new BuildingFundament(5,      5, 9,  0);
        AssetManager.buildings.get("hall_ming_2").fundament = new BuildingFundament(8,      8, 14, 0);
        AssetManager.buildings.get("temple_ming").fundament = new BuildingFundament(3,      3, 5,  0);
        AssetManager.buildings.get("barracks_ming").fundament = new BuildingFundament(3,    3, 7,  0);
        AssetManager.buildings.get("windmill_ming_0").fundament = new BuildingFundament(2,  1, 2,  0);
        AssetManager.buildings.get("windmill_ming_1").fundament = new BuildingFundament(2,  2, 2,  0);
        AssetManager.buildings.get("watch_tower_ming").fundament = new BuildingFundament(2, 2, 3,  0);
    }

    private void fix_bugs()
    {
        // 修复树阻止建筑升级
        foreach (BuildingAsset building in AssetManager.buildings.list)
        {
            if (building.type         == "trees") building.priority = -1;
            if (building.kingdom      == "nature") building.upgradeLevel -= 1;
            if (building.upgradeLevel >= 0) building.priority = Math.Max(building.priority, building.upgradeLevel);
        }
    }

    private void add_yao()
    {
        clone_human_buildings(Content_Constants.yao_race);
        AssetManager.buildings.get("windmill_yao_0").fundament = new BuildingFundament(2, 2, 1, 0);
        AssetManager.buildings.get("windmill_yao_1").fundament = new BuildingFundament(3, 3, 1, 0);
    }

    private void add_eastern_human()
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
        smelt_mill.smokeOffset = new Vector2Int(3, 5);
        smelt_mill.max_houses = 0;
        smelt_mill.build_place_single = true;
        smelt_mill.tech = "smelt_mill";
        var smelt_mill_order = race_order.addBuilding(CW_SB.order_smelt_mill, 1, 1);
        smelt_mill_order.requirements_orders = new List<string>();
        //smelt_mill_order.requirements_orders.Add(SB.order_hall_2);


        AssetManager.buildings.loadSprites(smelt_mill);
        AssetManager.buildings.loadSprites(bonfire);
    }

    private void clone_human_buildings(string race)
    {
        List<BuildingAsset> human_buildings = AssetManager.buildings.list
                                                          .Where(building => building.race == "human").ToList();

        foreach (BuildingAsset building in human_buildings)
        {
            var test_path = building.sprite_path;
            if (string.IsNullOrEmpty(test_path))
                test_path = "buildings/" + building.id.Replace(SK.human, race);
            if (UnityEngine.Resources.LoadAll<Sprite>(test_path) is not { Length: > 0 }) continue;

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