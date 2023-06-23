using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using System.Collections.Generic;
namespace Cultivation_Way.Core
{
    /// <summary>
    /// 拓展后的Actor, 用于添加新的功能
    /// <para>在没有模组冲突的情况下, 运行过程中所有Actor均能强制转换成CW_Actor</para>
    /// <para>由Actor转CW_Actor见<see cref="Cultivation_Way.HarmonySpace.H_Actor"/></para>
    /// </summary>
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
            this.data.set("cw_cultisys_immortal", 0);
        }
    }
}
