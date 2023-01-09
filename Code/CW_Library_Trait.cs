using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Trait : Asset
    {
        public ActorTrait origin_asset;
        public CW_BaseStats addition_stats;
        public WorldAction action_when_get;
        public CW_Asset_Trait()
        {

        }
        internal CW_Asset_Trait(ActorTrait origin_trait)
        {
            this.id = origin_trait.id;
            this.origin_asset = origin_trait;
        }
    }
    public class CW_Library_Trait : AssetLibrary<CW_Asset_Trait>
    {
        public override void init()
        {
            base.init();
            foreach(ActorTrait trait in AssetManager.traits.list)
            {
                __add(new CW_Asset_Trait(trait));
            }
        }
        public CW_Asset_Trait add(ActorTrait origin_trait, CW_BaseStats addition_stats, WorldAction action_when_get)
        {
            return add(new CW_Asset_Trait()
            {
                id = origin_trait.id,
                origin_asset = origin_trait,
                addition_stats = addition_stats,
                action_when_get = action_when_get
            });
        }
        public override CW_Asset_Trait add(CW_Asset_Trait trait_asset)
        {
            if(!AssetManager.traits.list.Contains(trait_asset.origin_asset)) AssetManager.traits.add(trait_asset.origin_asset);
            return base.add(trait_asset);
        }
        internal CW_Asset_Trait __add(CW_Asset_Trait trait_asset)
        {
            this.dict.Add(trait_asset.id, trait_asset);
            this.list.Add(trait_asset);
            return trait_asset;
        }
    }
}
