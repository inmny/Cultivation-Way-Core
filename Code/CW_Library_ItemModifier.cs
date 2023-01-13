using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Library_ItemModifier : CW_Asset_Library<CW_Asset_Item>
    {
        public Dictionary<string, List<CW_Asset_Item>> pools;
        public override void init()
        {
            base.init();
            pools = new Dictionary<string, List<CW_Asset_Item>>();
            pools["weapon"] = new List<CW_Asset_Item>();
            pools["armor"] = new List<CW_Asset_Item>();
            pools["accessory"] = new List<CW_Asset_Item>();
            foreach (ItemAsset asset in AssetManager.items_modifiers.list)
            {
                __add(asset);
            }
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        internal CW_Asset_Item __add(ItemAsset origin_asset)
        {
            CW_Asset_Item modifier = new CW_Asset_Item(origin_asset);
            // TODO: 小心大值rarity导致高内存占用
            int i;
            foreach(string key in pools.Keys)
            {
                if (modifier.origin_asset.pool.Contains(key))
                {
                    for (i = 0; i < modifier.origin_asset.rarity; i++) pools[key].Add(modifier);
                }
            }
            return base.add(new CW_Asset_Item(origin_asset));
        }
    }
}
