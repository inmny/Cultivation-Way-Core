using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Kingdom : Asset
    {
        public KingdomAsset origin_asset;
        public CW_Asset_Kingdom() { }
        public CW_Asset_Kingdom(KingdomAsset origin_asset)
        {
            this.id = origin_asset.id;
            this.origin_asset = origin_asset;
        }
    }
    public class CW_Library_Kingdom : CW_Asset_Library<CW_Asset_Kingdom>
    {
        public override void init()
        {
            base.init();
            foreach(KingdomAsset kingdom in AssetManager.kingdoms.list)
            {
                __add(new CW_Asset_Kingdom(kingdom));
            }
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        public override CW_Asset_Kingdom add(CW_Asset_Kingdom pAsset)
        {
            if (!AssetManager.kingdoms.list.Contains(pAsset.origin_asset)) AssetManager.kingdoms.add(pAsset.origin_asset);
            return base.add(pAsset);
        }
        internal CW_Asset_Kingdom __add(CW_Asset_Kingdom pAsset)
        {
            return base.add(pAsset);
        }
    }
}
