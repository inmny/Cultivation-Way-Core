using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
namespace Cultivation_Way.Core
{
    public class CW_Actor : Actor
    {
        public Library.CW_ActorAsset cw_asset;
        public List<string> cur_spells;
        internal Dictionary<string, CW_StatusEffectData> statuses;

        private readonly static List<string> __status_effects_to_remove = new();
        private readonly static List<CW_StatusEffectData> __status_effects_to_update = new();

        internal void cw_newCreature()
        {
            this.cur_spells = new List<string>();
            this.data.set(DataS.shield, 0f);
            CW_Element element = CW_Element.get_element_for_set_data(cw_asset.prefer_element, cw_asset.prefer_element_scale);
            this.data.set_element(element);
        }
    }
}
