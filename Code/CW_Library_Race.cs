using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Race : Asset
    {
        public Race origin_asset;
        public CW_Asset_Race() { }
        internal CW_Asset_Race(Race origin_asset)
        {
            this.id = origin_asset.id;
            this.origin_asset = origin_asset;
        }
    }
    public class CW_Library_Race : CW_Asset_Library<CW_Asset_Race>
    {
        internal List<string> added_races = new List<string>();
        public override void init()
        {
            base.init();
            foreach(Race race in AssetManager.raceLibrary.list)
            {
                __add(new CW_Asset_Race(race));
            }
        }
        internal override void register()
        {
            throw new NotImplementedException();
        }
        public override CW_Asset_Race add(CW_Asset_Race pAsset)
        {
            added_races.Add(pAsset.id);
            AssetManager.raceLibrary.add(pAsset.origin_asset);
            return base.add(pAsset);
        }
        internal CW_Asset_Race __add(CW_Asset_Race asset)
        {
            return base.add(asset);
        }
    }
}
