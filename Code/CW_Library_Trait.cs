using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Trait : Asset
    {
        public ActorTrait origin_trait;
        public CW_BaseStats addition_stats;
        public WorldAction action_when_get;
    }
    public class CW_Library_Trait : AssetLibrary<CW_Asset_Trait>
    {
        public override void init()
        {
            base.init();
            foreach(ActorTrait trait in AssetManager.traits.list)
            {
                add(new CW_Asset_Trait()
                {
                    id = trait.id,
                    origin_trait = trait,
                    addition_stats = new CW_BaseStats(),
                    action_when_get = null
                });
            }
        }
    }
}
