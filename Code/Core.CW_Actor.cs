using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
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
        /// <summary>
        /// ActorAsset拓展部分, 在生物创建时已经初始化
        /// </summary>
        public Library.CW_ActorAsset cw_asset;
        /// <summary>
        /// 当前可用的法术
        /// </summary>
        public List<string> cur_spells = new();
        /// <summary>
        /// data中的法术的拷贝, 用于快速访问
        /// </summary>
        private HashSet<string> __data_spells = new();
        internal Dictionary<string, CW_StatusEffectData> statuses;

        private readonly static List<string> __status_effects_to_remove = new();
        private readonly static List<CW_StatusEffectData> __status_effects_to_update = new();
        /// <summary>
        /// 创建/改良血脉
        /// </summary>
        public void create_blood()
        {
            Dictionary<string, float> curr_bloods = data.get_blood_nodes();
            BloodNodeAsset main_blood = data.get_main_blood();
        }
        /// <summary>
        /// 创建/改良功法
        /// </summary>
        public void create_cultibook()
        {
            Cultibook old_cultibook = data.get_cultibook();
            Cultibook new_cultibook = new();
            if (old_cultibook != null)
            {
                new_cultibook.copy_from(old_cultibook, false);

                new_cultibook.spells = new string[old_cultibook.spells.Length + (Toolbox.randomChance(Constants.Others.new_spell_slot_for_cultibook) ? 1 : 0)];
                old_cultibook.spells.CopyTo(new_cultibook.spells, 0);

                new_cultibook.editor_name = getName();

                new_cultibook.level++;
            }
            else
            {
                new_cultibook.author_name = getName();
                int spell_slot = 1;
                int max_spell_slot = 4;
                while(max_spell_slot-->0 && Toolbox.randomChance(Constants.Others.new_spell_slot_for_cultibook)) spell_slot++;

                new_cultibook.spells = new string[spell_slot];
            }
            new_cultibook.name = $"{new_cultibook.author_name}著,{new_cultibook.editor_name}改的功法";
            new_cultibook.id = $"{new_cultibook.level}_{data.id}";
            new_cultibook.bonus_stats.mergeStats(data.get_element().comp_bonus_stats(), new_cultibook.level * 0.3f);

            Library.Manager.cultibooks.add(new_cultibook);
            data.set_cultibook(new_cultibook);
        }
        /// <summary>
        /// 学会法术
        /// </summary>
        public void learn_spell(CW_SpellAsset spell)
        {
            if (__data_spells.Contains(spell.id)) return;
            __data_spells.Add(spell.id);
            data.add_spell(spell.id);
        }
        /// <summary>
        /// 检查目标修炼体系是否能够升级, 如果能够则会进行一次升级
        /// </summary>
        /// <param name="cultisys_id">目标修炼体系，null为检查所有修炼体系</param>
        /// <exception cref="System.Exception">严格模式下该生物不存在该修炼体系</exception>
        public void check_level_up(string cultisys_id = null)
        {
            if(cultisys_id != null)
            {
                data.get(cultisys_id, out int level, -1);
                if(level < 0)
                {
                    if(Constants.Others.strict_mode) throw new System.Exception("CW_Actor.check_level_up: cultisys level < 0");
                    else return;
                }
                CultisysAsset cultisys = Library.Manager.cultisys.get(cultisys_id);
                if(level >= cultisys.max_level) return;

                if (cultisys.can_levelup(this, cultisys)) __level_up_and_get_bonus(cultisys, level);

                return;
            }
            else
            {
                int[] cultisys_levels = data.get_cultisys_level();
                for(int i = 0; i < Library.Manager.cultisys.size; i++)
                {
                    if (cultisys_levels[i] < 0) continue;
                    CultisysAsset cultisys = Library.Manager.cultisys.list[i];

                    if (cultisys_levels[i] >= cultisys.max_level) continue;

                    if (cultisys.can_levelup(this, cultisys)) __level_up_and_get_bonus(cultisys, cultisys_levels[i]);
                }
            }
        }
        /// <summary>
        /// 升级并获取增益
        /// </summary>
        /// <param name="cultisys">升级的修炼体系</param>
        /// <param name="curr_level">升级前的等级</param>
        private void __level_up_and_get_bonus(CultisysAsset cultisys, int curr_level)
        {
            curr_level++;
            data.set(cultisys.id, curr_level);

            if (__can_create_blood_on_levelup(cultisys, curr_level)) create_blood();
            if (__can_create_cultibook_on_levelup(cultisys, curr_level)) create_cultibook();
            if (cultisys.external_levelup_bonus != null) _ = cultisys.external_levelup_bonus(this, cultisys, curr_level);
        }
        /// <summary>
        /// 能否在本次升级时创建/改良血脉
        /// </summary>
        /// <param name="cultisys">升级的修炼体系</param>
        /// <param name="curr_level">升级后的等级</param>
        /// <returns></returns>
        private bool __can_create_blood_on_levelup(CultisysAsset cultisys, int curr_level)
        {
            return false;
        }
        /// <summary>
        /// 能否在本次升级时创建/改良功法
        /// </summary>
        /// <param name="cultisys">升级的修炼体系</param>
        /// <param name="curr_level">升级后的等级</param>
        /// <returns></returns>
        private bool __can_create_cultibook_on_levelup(CultisysAsset cultisys, int curr_level)
        {
            return curr_level % 5 == 1;
        }

        internal void cw_newCreature()
        {
            this.data.set_element(CW_Element.get_element_for_set_data(cw_asset.prefer_element, cw_asset.prefer_element_scale));

            if (Constants.Others.new_creature_create_blood) create_blood();

            // 暂且不支持直接的血脉修炼体系
            uint allow_cultisys_types = 0b111;
            // 强制添加的修炼体系
            foreach(CultisysAsset cultisys in cw_asset.force_cultisys)
            {
                if((allow_cultisys_types & (uint)cultisys.type) == 0) continue;

                data.set(cultisys.id, 0);
                allow_cultisys_types &= ~(uint)cultisys.type;
            }
            foreach(CultisysAsset cultisys in cw_asset.allowed_cultisys)
            {
                if ((allow_cultisys_types & (uint)cultisys.type) == 0) continue;

                if (!cultisys.allow(this, cultisys)) continue;

                data.set(cultisys.id, 0);
                allow_cultisys_types &= ~(uint)cultisys.type;
            }
            this.setStatsDirty();
        }
    }
}
