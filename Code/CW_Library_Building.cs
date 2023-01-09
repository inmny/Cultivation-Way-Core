using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Building : Asset
    {
        public BuildingAsset origin_stats;
        public WorldAction destroy_action;
        public CW_BaseStats addition_stats;

    }
    public class CW_Library_Building : AssetLibrary<CW_Asset_Building>
    {
        public override void init()
        {
            base.init();
            foreach(BuildingAsset asset in AssetManager.buildings.list)
            {
                add(asset);
            }
        }

        public CW_Asset_Building add(BuildingAsset origin_asset, WorldAction destroy_action = null, CW_BaseStats addition_stats = null)
        {
            CW_Asset_Building building = new CW_Asset_Building() { id = origin_asset.id, addition_stats = addition_stats == null ? new CW_BaseStats() : addition_stats, destroy_action = destroy_action, origin_stats = origin_asset };
            this.dict.Add(building.id, building);
            this.list.Add(building);
            if (!AssetManager.buildings.list.Contains(origin_asset)) AssetManager.buildings.add(origin_asset);
            return building;
        }
    }
}
