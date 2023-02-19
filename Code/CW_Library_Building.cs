using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Building : Asset
    {
        public BuildingAsset origin_stats;
        public WorldAction destroy_action;
        public CW_BaseStats cw_base_stats;

    }
    public class CW_Library_Building : CW_Asset_Library<CW_Asset_Building>
    {
        internal List<string> added_buildings = new List<string>();
        public override void init()
        {
            base.init();
            foreach(BuildingAsset asset in AssetManager.buildings.list)
            {
                add(asset);
            }
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        public override CW_Asset_Building clone(string pNew, string pFrom)
        {
            CW_Asset_Building new_stats = JsonUtility.FromJson<CW_Asset_Building>(JsonUtility.ToJson(this.dict[pFrom]));
            new_stats.id = pNew;
            new_stats.cw_base_stats = dict[pFrom].cw_base_stats.deepcopy();
            new_stats.origin_stats = JsonUtility.FromJson<BuildingAsset>(JsonUtility.ToJson(AssetManager.buildings.dict[pFrom]));
            new_stats.origin_stats.id = pNew;
            new_stats.origin_stats.baseStats = new_stats.cw_base_stats.base_stats;
            this.add(new_stats);
            return new_stats;
        }
        public override CW_Asset_Building add(CW_Asset_Building pAsset)
        {
            added_buildings.Add(pAsset.id);
            AssetManager.buildings.add(pAsset.origin_stats);
            return base.add(pAsset);
        }
        public CW_Asset_Building add(BuildingAsset origin_asset, WorldAction destroy_action = null, CW_BaseStats addition_stats = null)
        {
            CW_Asset_Building building = new CW_Asset_Building() { id = origin_asset.id, cw_base_stats = addition_stats == null ? new CW_BaseStats(origin_asset.baseStats) : addition_stats, destroy_action = destroy_action, origin_stats = origin_asset };
            this.dict.Add(building.id, building);
            this.list.Add(building);
            if (!AssetManager.buildings.list.Contains(origin_asset)) AssetManager.buildings.add(origin_asset);
            return building;
        }
        public override CW_Asset_Building get(string pID)
        {
            CW_Asset_Building ret;
            if (this.dict.TryGetValue(pID, out ret)) return ret;
            BuildingAsset origin_stats = AssetManager.buildings.get(pID);
            if (origin_stats == null) return base.get(pID);
            return add(origin_stats);
        }
    }
}
