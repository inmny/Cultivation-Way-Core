using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Building
    {
        internal static void add_buildings()
        {
            add_eastern_human_building();
            add_yao_building();
            fix_origin_bugs();
        }

        private static void add_yao_building()
        {
            AssetManager.race_build_orders.clone("yao", "human").replace("human", "yao");

            List<CW_Asset_Building> human_buildings = new List<CW_Asset_Building>();
            foreach (CW_Asset_Building building in CW_Library_Manager.instance.buildings.list)
            {
                if (building.origin_stats.race == "eastern_human") human_buildings.Add(building);
            }

            foreach (CW_Asset_Building building in human_buildings)
            {
                CW_Asset_Building new_building = CW_Library_Manager.instance.buildings.clone(building.id.Replace("eastern_human", "yao"), building.id);

                new_building.origin_stats.id = new_building.id;
                new_building.origin_stats.race = "yao";

                if (building.origin_stats.canBeUpgraded) new_building.origin_stats.upgradeTo = new_building.origin_stats.upgradeTo.Replace("eastern_human", "yao");

                if (!string.IsNullOrEmpty(new_building.origin_stats.upgradedFrom)) new_building.origin_stats.upgradedFrom = new_building.origin_stats.upgradedFrom.Replace("eastern_human", "yao");

                AssetManager.buildings.loadSprites(new_building.origin_stats);
            }
            AssetManager.buildings.get("windmill_yao").fundament = new BuildingFundament(2, 2, 1, 0);
            AssetManager.buildings.get("1windmill_yao").fundament = new BuildingFundament(3, 3, 1, 0);
        }


        private static void add_eastern_human_building()
        {
            AssetManager.race_build_orders.clone("eastern_human", "human").replace("human", "eastern_human");
            AssetManager.race_build_orders.get("eastern_human").replace("bonfire", "bonfire_eastern_human");

            List<CW_Asset_Building> human_buildings = new List<CW_Asset_Building>();
            foreach (CW_Asset_Building building in CW_Library_Manager.instance.buildings.list)
            {
                if (building.origin_stats.race == "human") human_buildings.Add(building);
            }

            foreach (CW_Asset_Building building in human_buildings)
            {
                CW_Asset_Building new_building = CW_Library_Manager.instance.buildings.clone(building.id.Replace("human", "eastern_human"), building.id);

                new_building.origin_stats.id = new_building.id;
                new_building.origin_stats.race = "eastern_human";

                if (building.origin_stats.canBeUpgraded) new_building.origin_stats.upgradeTo = new_building.origin_stats.upgradeTo.Replace("human", "eastern_human");

                if (!string.IsNullOrEmpty(new_building.origin_stats.upgradedFrom)) new_building.origin_stats.upgradedFrom = new_building.origin_stats.upgradedFrom.Replace("human", "eastern_human");

                AssetManager.buildings.loadSprites(new_building.origin_stats);
            }
            CW_Asset_Building bonfire = CW_Library_Manager.instance.buildings.clone("bonfire_eastern_human", "bonfire");
            bonfire.origin_stats.smoke = false;
            bonfire.origin_stats.race = "eastern_human";
            AssetManager.buildings.loadSprites(bonfire.origin_stats);

            AssetManager.buildings.get("tent_eastern_human").fundament = new BuildingFundament(1, 1, 1, 0);
            AssetManager.buildings.get("house_eastern_human").fundament = new BuildingFundament(3, 3, 4, 0);
            AssetManager.buildings.get("1house_eastern_human").fundament = new BuildingFundament(3, 3, 4, 0);
            AssetManager.buildings.get("2house_eastern_human").fundament = new BuildingFundament(3, 3, 4, 0);
            AssetManager.buildings.get("3house_eastern_human").fundament = new BuildingFundament(4, 4, 6, 0);
            AssetManager.buildings.get("4house_eastern_human").fundament = new BuildingFundament(5, 5, 9, 0);
            AssetManager.buildings.get("5house_eastern_human").fundament = new BuildingFundament(5, 5, 9, 0);
            AssetManager.buildings.get("hall_eastern_human").fundament = new BuildingFundament(4, 4, 7, 0);
            AssetManager.buildings.get("1hall_eastern_human").fundament = new BuildingFundament(5, 5, 9, 0);
            AssetManager.buildings.get("2hall_eastern_human").fundament = new BuildingFundament(8, 8, 14, 0);

            AssetManager.buildings.get("temple_eastern_human").fundament = new BuildingFundament(3, 3, 5, 0);
            AssetManager.buildings.get("barracks_eastern_human").fundament = new BuildingFundament(3, 3, 7, 0);
            AssetManager.buildings.get("windmill_eastern_human").fundament = new BuildingFundament(2, 1, 2, 0);
            AssetManager.buildings.get("1windmill_eastern_human").fundament = new BuildingFundament(2, 2, 2, 0);
            AssetManager.buildings.get("watch_tower_eastern_human").fundament = new BuildingFundament(2, 2, 3, 0);
        }
        private static void fix_origin_bugs()
        {
            // 修复树阻止建筑升级
            foreach (CW_Asset_Building building in CW_Library_Manager.instance.buildings.list)
            {
                if (building.origin_stats.type == "trees") building.origin_stats.priority = -1;
                if (building.origin_stats.kingdom == "nature") building.origin_stats.upgradeLevel -= 1;
                if (building.origin_stats.upgradeLevel >= 0) building.origin_stats.priority = Math.Max(building.origin_stats.priority, building.origin_stats.upgradeLevel);
            }
        }
    }
}
