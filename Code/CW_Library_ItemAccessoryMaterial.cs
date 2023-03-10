using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Library_ItemAccessoryMaterial : CW_Asset_Library<CW_Asset_Item>
    {
        public override void init()
        {
            base.init();
            foreach (ItemAsset asset in AssetManager.items_material_accessory.list)
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
            return base.add(new CW_Asset_Item(origin_asset));
        }
    }
}
