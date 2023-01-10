using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Library_ItemArmorMaterial : ItemAssetLibrary<CW_Asset_Item>
    {
        public override void init()
        {
            base.init();
            foreach (ItemAsset asset in AssetManager.items_material_armor.list)
            {
                __add(asset);
            }
        }
        public override ItemAsset add(ItemAsset pAsset)
        {
            if (!AssetManager.items_material_armor.list.Contains(pAsset)) AssetManager.items_material_armor.add(pAsset);
            return base.add(new CW_Asset_Item(pAsset));
        }
        public CW_Asset_Item add(ItemAsset asset, CW_BaseStats addition_stats)
        {
            if (!AssetManager.items_material_armor.list.Contains(asset)) AssetManager.items_material_armor.add(asset);
            return __add(asset, addition_stats);
        }
        internal CW_Asset_Item __add(ItemAsset asset, CW_BaseStats addition_stats = null)
        {
            return (CW_Asset_Item)base.add(new CW_Asset_Item(asset, addition_stats == null ? new CW_BaseStats() : addition_stats));
        }
    }
}
