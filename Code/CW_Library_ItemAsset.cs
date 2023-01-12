using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Item : Asset
    {
        public ItemAsset origin_asset;
        public CW_BaseStats cw_base_stats;
        public CW_Asset_Item() { }
        public CW_Asset_Item(ItemAsset origin_asset)
        {
            this.id = origin_asset.id;
            this.origin_asset = origin_asset;
            this.cw_base_stats = new CW_BaseStats(origin_asset.baseStats);
        }
    }
}
