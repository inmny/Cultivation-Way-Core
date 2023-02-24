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
        /// <summary>
        /// 即将弃用换做cw_stats
        /// </summary>
        public CW_BaseStats addition_stats;
        public CW_BaseStats cw_stats;
        public WorldAction action_when_get;
        public CW_Asset_Trait()
        {

        }
        internal CW_Asset_Trait(ActorTrait origin_trait)
        {
            this.id = origin_trait.id;
            this.origin_asset = origin_trait;
            this.cw_stats = new CW_BaseStats(origin_asset.baseStats);
            this.addition_stats = cw_stats;
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
        internal void register()
        {
            throw new NotImplementedException();
        }
        public CW_Asset_Trait add(ActorTrait origin_trait, CW_BaseStats cw_stats, WorldAction action_when_get)
        {
            return add(new CW_Asset_Trait()
            {
                id = origin_trait.id,
                origin_asset = origin_trait,
                cw_stats = cw_stats,
                addition_stats = cw_stats,
                action_when_get = action_when_get
            });
        }
        public override CW_Asset_Trait get(string pID)
        {
            CW_Asset_Trait ret;
            if (dict.TryGetValue(pID, out ret)) return ret;
            UnityEngine.Debug.Log($"Try to get '{pID}'");
            ret = new CW_Asset_Trait(AssetManager.traits.get(pID));
            
            if (ret == null) return null;
            this.__add(ret);
            return ret;
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
