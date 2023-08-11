using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Newtonsoft.Json;
using UnityEngine;

namespace Cultivation_Way.Core;

/// <summary>
///     拓展后的Actor, 用于添加新的功能
///     <para>在没有模组冲突的情况下, 运行过程中所有Actor均能强制转换成CW_Actor</para>
///     <para>由Actor转CW_Actor见<see cref="Cultivation_Way.HarmonySpace.H_Actor" /></para>
/// </summary>
public class CW_Actor : Actor
{
    /// <summary>
    ///     当前可用的法术
    /// </summary>
    public List<string> cur_spells = new();

    /// <summary>
    ///     data中的法术的拷贝, 用于快速访问
    /// </summary>
    internal HashSet<string> __data_spells = new();

    /// <summary>
    ///     ActorAsset拓展部分, 在生物创建时已经初始化
    /// </summary>
    public CW_ActorAsset cw_asset;

    /// <summary>
    ///     状态数据
    /// </summary>
    internal Dictionary<string, CW_StatusEffectData> statuses;

    public void start_color_effect(string color_id, float rewrtie_timer = -1)
    {
        if (!asset.effectDamage)
        {
            return;
        }

        if (!is_visible)
        {
            return;
        }

        batch.c_color_effect.Add(this);
        colorEffect = rewrtie_timer < 0 ? 0.3f : rewrtie_timer;
        Material material = FastVisit.get_color_material(color_id);
        if (material == null)
        {
            Logger.Warn($"No found color material: {color_id}");
            return;
        }

        setSpriteSharedMaterial(material);
    }

    /// <summary>
    ///     死后保留需要保留的数据
    /// </summary>
    internal void leave_data()
    {
        BloodNodeAsset blood = data.get_main_blood();
        if (blood != null && blood.id == data.id && blood.ancestor_data == data)
        {
            blood.ancestor_data = JsonConvert.DeserializeObject<ActorData>(JsonConvert.SerializeObject(data));
        }

        data.clear_blood_nodes();
        data.clear_cultibook();
    }

    /// <summary>
    ///     申请释放法术
    /// </summary>
    /// <param name="spell_id">法术id</param>
    /// <param name="target">法术目标, 可为null</param>
    /// <param name="target_tile">法术目标区域, 可为null</param>
    /// <returns>法术是否合法</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool cast_spell(string spell_id, BaseSimObject target, WorldTile target_tile)
    {
        return cast_spell(Manager.spells.get(spell_id), target, target_tile);
    }

    /// <summary>
    ///     申请释放法术
    /// </summary>
    /// <param name="spell">法术asset</param>
    /// <param name="target">法术目标, 可为null</param>
    /// <param name="target_tile">法术目标区域, 可为null</param>
    /// <returns>法术是否合法</returns>
    public bool cast_spell(CW_SpellAsset spell, BaseSimObject target, WorldTile target_tile)
    {
        if ((spell.target_type == SpellTargetType.TILE && target_tile == null) ||
            (spell.target_type == SpellTargetType.ACTOR &&
             (target == null || target.objectType == MapObjectType.Building)) ||
            (spell.target_type == SpellTargetType.BUILDING &&
             (target == null || target.objectType == MapObjectType.Actor)))
            return false;
        if (data.hasFlag(DataS.is_bound)) return false;

        bool is_enemy = kingdom == null || kingdom.isEnemy(target.kingdom);

        if ((spell.target_camp == SpellTargetCamp.ALIAS && is_enemy) ||
            (spell.target_camp == SpellTargetCamp.ENEMY && !is_enemy)) return false;

        float cost = spell.spell_cost_action(spell, this);
        if (cost < 0) return false;

        CW_Core.mod_state.spell_manager.enqueue_spell(spell, this, target, target_tile, cost);
        return true;
    }

    /// <summary>
    ///     添加状态并返回状态数据, 如果已经存在则返回存在的状态数据
    ///     <para>仅作用于模组内状态效果</para>
    /// </summary>
    /// <param name="status_id">添加的状态Asset的id</param>
    /// <param name="from">状态来源</param>
    /// <param name="rewrite_effect_time">重写状态持续时间</param>
    /// <param name="as_id">加入状态表的key</param>
    /// <returns></returns>
    public CW_StatusEffectData add_status(string status_id, BaseSimObject from = null, float rewrite_effect_time = -1,
        string as_id = null)
    {
        CW_StatusEffect status_asset = Manager.statuses.get(status_id);
        if (status_asset == null) return null;
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (string op_status_id in status_asset.opposite_statuses)
        {
            if (has_status(op_status_id)) return null;
        }

        as_id ??= status_id;
        statuses ??= new Dictionary<string, CW_StatusEffectData>();

        if (statuses.TryGetValue(as_id, out CW_StatusEffectData same_id_status)) return same_id_status;
        CW_StatusEffectData status = new(status_asset, from)
        {
            id = as_id
        };
        if (rewrite_effect_time > 0) status.left_time = rewrite_effect_time;

        if (!string.IsNullOrEmpty(status_asset.anim_id))
        {
            status.anim = EffectManager.instance.spawn_anim(status_asset.anim_id,
                from == null ? Vector2.zero : from.currentPosition, currentPosition, from, this);
            if (status.anim is { isOn: true })
            {
                status.anim.change_scale(stats[S.scale]);
            }
        }

        statuses.Add(as_id, status);
        status_asset.action_on_get?.Invoke(status, from, this);

        activeStatus_dict ??= new Dictionary<string, StatusEffectData>();
        setStatsDirty();
        return status;
    }

    /// <summary>
    ///     按id搜寻状态, 仅作用于模组内状态效果
    /// </summary>
    /// <param name="id">状态id</param>
    /// <returns>状态效果数据</returns>
    public CW_StatusEffectData get_status(string id)
    {
        if (statuses == null || statuses.Count > 0) return null;
        return statuses.TryGetValue(id, out CW_StatusEffectData status) ? status : null;
    }

    /// <summary>
    ///     拓展后hasAnyStatusEffect, 直接调用
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool has_any_status_effect()
    {
        return activeStatus_dict is { Count: > 0 } || statuses is { Count: > 0 };
    }

    /// <summary>
    ///     拓展后的hasStatus, 直接调用
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool has_status(string id)
    {
        return (activeStatus_dict is { Count: > 0 } && activeStatus_dict.ContainsKey(id)) ||
               (statuses is { Count: > 0 } && statuses.ContainsKey(id));
    }

    /// <summary>
    ///     对于模组内状态, 则跳转至模组内状态添加
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override void addStatusEffect(string pID, float pOverrideTimer = -1)
    {
        if (!Manager.statuses.contains(pID))
        {
            base.addStatusEffect(pID, pOverrideTimer);
            return;
        }

        add_status(pID, null, pOverrideTimer);
        if (!has_any_status_effect()) return;
        activeStatus_dict ??= new Dictionary<string, StatusEffectData>();
        batch.c_status_effects.Add(this);
    }

    /// <summary>
    ///     一同更新模组状态效果
    /// </summary>
    /// <param name="pElapsed"></param>
    public override void updateStatusEffects(float pElapsed)
    {
        if (statuses == null || statuses.Count == 0)
        {
            if (activeStatus_dict == null || activeStatus_dict.Count == 0) return;

            base.updateStatusEffects(pElapsed);
            return;
        }

        List<CW_StatusEffectData> list = Factories.status_list_factory.get_next();
        list.AddRange(statuses.Values);
        foreach (CW_StatusEffectData status in list.Where(status => !status.finished))
        {
            if (status.status_asset.action_on_update != null && status._update_action_timer <= 0)
            {
                status.status_asset.action_on_update(status, status.source, this);
                status._update_action_timer = status.status_asset.action_interval;
            }

            status.update_timer(pElapsed);
            //activeStatus_dict?[status.status_asset.id].setTimer(status.left_time);
        }

        foreach (CW_StatusEffectData status in list.Where(status => status.finished))
        {
            status.status_asset.action_on_end?.Invoke(status, status.source, this);
            statuses.Remove(status.id);
        }

        list.Clear();
        if (activeStatus_dict == null || activeStatus_dict.Count == 0) return;

        base.updateStatusEffects(pElapsed);
    }

    /// <summary>
    ///     恢复属性(生命/其他)
    /// </summary>
    /// <param name="regen_id">恢复的属性id</param>
    /// <param name="value">恢复量, 生命恢复四舍五入</param>
    public void regen(string regen_id, float value)
    {
        if (regen_id == S.health)
        {
            data.health += (int)(value + 0.5f);
            if (data.health > stats[S.health])
            {
                data.health = (int)stats[S.health];
            }

            return;
        }

        data.get(regen_id, out float cur_value, -1);
        if (cur_value < 0)
        {
            return;
        }

        cur_value += value;
        if (cur_value > stats[regen_id])
        {
            cur_value = stats[regen_id];
        }

        data.set(regen_id, cur_value);
    }

    /// <summary>
    ///     每月更新，用于生命恢复等
    /// </summary>
    internal void update_month()
    {
        // 生命恢复
        data.health += (int)stats[CW_S.health_regen];
        if (data.health >= stats[S.health])
        {
            data.health = (int)stats[S.health];
        }

        // 元神恢复
        data.get(DataS.soul, out float soul);
        soul += stats[CW_S.soul_regen];
        if (soul > stats[CW_S.soul])
        {
            soul = stats[CW_S.soul];
        }

        data.set(DataS.soul, soul);

        // 修炼体系的月度更新
        foreach (CultisysAsset cultisys in Manager.cultisys.list.Where(cultisys =>
                     cultisys.monthly_update_action != null))
        {
            data.get(cultisys.id, out int level, -1);
            if (level < 0) continue;
            cultisys.monthly_update_action(this, cultisys, level);
        }
    }

    /// <summary>
    ///     重写getHit, 并应用属性
    /// </summary>
    public override void getHit(float pDamage, bool pFlash = true, AttackType pAttackType = AttackType.Other,
        BaseSimObject pAttacker = null, bool pSkipIfShake = true, bool pMetallicWeapon = false)
    {
        attackedBy = null;
        CW_AttackType attack_type = (CW_AttackType)pAttackType;

        if ((pSkipIfShake && shake_active) || data.health <= 0 || !isAlive() || has_status("invincible")) return;

        #region 攻击音效

        if (attack_type == CW_AttackType.Weapon)
        {
            if (pMetallicWeapon && haveMetallicWeapon())
            {
                MusicBox.playSound("event:/SFX/HIT/HitSwordSword", currentTile, false, true);
            }
            else if (!string.IsNullOrEmpty(asset.sound_hit))
            {
                MusicBox.playSound(asset.sound_hit, currentTile, false, true);
            }
        }

        #endregion

        if (pAttacker != null && pAttacker.isActor() && pAttacker.isAlive() &&
            ((CW_Actor)pAttacker).cw_asset.addition_soul_damage && attack_type != CW_AttackType.Soul)
        {
            getHit(pAttacker.stats[CW_S.soul_regen], pFlash, (AttackType)CW_AttackType.Soul, pAttacker, pSkipIfShake);
        }

        #region 伤害计算

        float num = 1f;
        switch (attack_type)
        {
            case CW_AttackType.Other:
            case CW_AttackType.Weapon:
                num = 1f - stats[S.armor] / (stats[S.armor] + 100);
                break;
            case CW_AttackType.Acid:
            case CW_AttackType.Eaten:
            case CW_AttackType.Fire:
            case CW_AttackType.Hunger:
            case CW_AttackType.Infection:
            case CW_AttackType.Plague:
            case CW_AttackType.Poison:
            case CW_AttackType.Tumor:
            case CW_AttackType.AshFever:
            case CW_AttackType.Spell:
                num = 1f - stats[CW_S.spell_armor] / (stats[CW_S.spell_armor] + 100);
                break;
            case CW_AttackType.Soul:
                if (pAttacker != null && pAttacker.isActor() && pAttacker.isAlive())
                {
                    num = 1f - stats[CW_S.soul_regen] / Mathf.Min(0.01f, pAttacker.stats[CW_S.soul_regen]);
                }
                else
                {
                    num = 0;
                }

                break;
        }

        pDamage *= num;

        #endregion

        #region 攻击应激

        if (pAttacker != this) attackedBy = pAttacker;
        if (!has_attack_target && attackedBy != null && !shouldIgnoreTarget(attackedBy) && canAttackTarget(attackedBy))
        {
            setAttackTarget(attackedBy);
        }

        foreach (string text in data.s_traits_ids)
        {
            AssetManager.traits.get(text).action_get_hit?.Invoke(this, pAttacker, currentTile);
        }

        if (activeStatus_dict != null)
        {
            foreach (StatusEffectData status_effect_data in activeStatus_dict.Values)
            {
                status_effect_data.asset.action_get_hit?.Invoke(this, pAttacker, currentTile);
            }
        }

        if (statuses != null)
        {
            List<CW_StatusEffectData> statuses_list = Factories.status_list_factory.get_next();
            statuses_list.AddRange(statuses.Values);
            foreach (CW_StatusEffectData status_effect_data in statuses_list)
            {
                status_effect_data.status_asset.action_on_get_hit?.Invoke(status_effect_data, pAttacker, this);
            }

            statuses_list.Clear();
        }

        if (__data_spells.Count > 0)
        {
            CW_SpellAsset spell = Manager.spells.get(__data_spells.GetRandom());
            if ((pAttacker != null && spell.can_trigger(SpellTriggerTag.NAMED_DEFEND)) ||
                (pAttacker == null && spell.can_trigger(SpellTriggerTag.UNNAMED_DEFEND)))
            {
                cast_spell(spell, pAttacker, pAttacker == null ? null : pAttacker.currentTile);
            }
        }

        asset.action_get_hit?.Invoke(this, pAttacker, currentTile);

        #endregion

        if (pDamage < 1)
        {
            return;
        }

        data.get(DataS.soul, out float curr_soul);
        if (attack_type == CW_AttackType.Soul)
        {
            curr_soul -= pDamage;
            data.set(DataS.soul, curr_soul);
        }
        else
        {
            data.health -= (int)pDamage;
        }

        #region 攻击额外效果

        timer_action = 0.002f;
        if (pFlash) startColorEffect(ActorColorEffect.Red);
        if (data.health <= 0 || curr_soul <= 0)
        {
            if (!(pAttacker != null && pAttacker != this && pAttacker.isActor() && pAttacker.isAlive()))
            {
                killHimself(false, pAttackType);
                return;
            }

            BattleKeeperManager.unitKilled(this);
            try
            {
                pAttacker.a.newKillAction(this, kingdom);
            }
            catch (NullReferenceException)
            {
                if (pAttacker.a.data == null) Logger.Error("pAttacker.a.data==null");
                if (pAttacker.kingdom == null) Logger.Error("pAttacker.kingdom==null");
                Logger.Error("Kill action error");
            }

            if (pAttacker.city == null)
            {
                killHimself(false, pAttackType);
                return;
            }

            if (asset.animal)
            {
                pAttacker.city.data.storage.change("meat");
            }

            if (asset.animal || (asset.unit && pAttacker.a.hasTrait("savage")))
            {
                if (Toolbox.randomChance(0.5f))
                {
                    pAttacker.city.data.storage.change(SR.bones);
                }
                else if (Toolbox.randomChance(0.5f))
                {
                    pAttacker.city.data.storage.change(SR.leather);
                }
                else if (Toolbox.randomChance(0.5f))
                {
                    pAttacker.city.data.storage.change(SR.meat);
                }
            }

            killHimself(false, pAttackType);
            return;
        }

        if (attack_type == CW_AttackType.Weapon && pDamage >= stats[S.health] * 0.01f && !asset.immune_to_injuries &&
            !hasStatus("shield"))
        {
            if (Toolbox.randomChance(0.02f))
            {
                addTrait("crippled");
            }

            if (Toolbox.randomChance(0.02f))
            {
                addTrait("eyepatch");
            }
        }

        try
        {
            startShake();
        }
        catch (NullReferenceException)
        {
            if (batch == null)
                Logger.Error($"batch==null, isDestroyed: {object_destroyed}, Alive: {isAlive()}, Health: " +
                             $"{(data == null ? -1 : data.health)}");
        }

        #endregion
    }

    /// <summary>
    ///     创建/改良血脉
    /// </summary>
    public void create_blood()
    {
        if (data.get_main_blood_id() == data.id) return;

        BloodNodeAsset blood_asset = new()
        {
            id = data.id,
            ancestor_data = data
        };
        Manager.bloods.add(blood_asset);
        data.set_blood_nodes(new Dictionary<string, float>
        {
            { blood_asset.id, 1f }
        });

        setStatsDirty();
    }

    /// <summary>
    ///     创建/改良功法
    /// </summary>
    public void create_cultibook()
    {
        Cultibook old_cultibook = data.get_cultibook();
        Cultibook new_cultibook = new();
        if (old_cultibook != null)
        {
            new_cultibook.copy_from(old_cultibook);

            new_cultibook.max_spell_nr = old_cultibook.max_spell_nr +
                                         (Toolbox.randomChance(Constants.Others.new_spell_slot_for_cultibook) ? 1 : 0);

            new_cultibook.editor_name = getName();

            new_cultibook.level++;
        }
        else
        {
            new_cultibook.author_name = getName();
            int spell_slot = 1;
            int max_spell_slot = 4;
            while (max_spell_slot-- > 0 && Toolbox.randomChance(Constants.Others.new_spell_slot_for_cultibook))
                spell_slot++;

            new_cultibook.max_spell_nr = spell_slot;
        }

        // 功法内法术更新
        if (__data_spells.Count > 0)
        {
            HashSet<string> can_be_add_spells = new();
            can_be_add_spells.UnionWith(__data_spells);
            can_be_add_spells.ExceptWith(new_cultibook.spells);

            if (can_be_add_spells.Count > 0)
            {
                string spell_id = can_be_add_spells.GetRandom();

                if (Manager.spells.get(spell_id).learn_check(this) >= 0)
                {
                    new_cultibook.spells.Add(spell_id);
                    if (new_cultibook.spells.Count > new_cultibook.max_spell_nr)
                    {
                        new_cultibook.spells.RemoveAt(0);
                    }
                }
            }
        }


        new_cultibook.name = $"{new_cultibook.author_name}著,{new_cultibook.editor_name}改的功法";
        new_cultibook.id = $"{new_cultibook.level}_{data.id}";
        new_cultibook.bonus_stats.merge_stats(data.get_element().comp_bonus_stats(), new_cultibook.level * 0.3f);

        Manager.cultibooks.add(new_cultibook);
        data.set_cultibook(new_cultibook);

        setStatsDirty();
    }

    /// <summary>
    ///     学会法术
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void learn_spell(CW_SpellAsset spell)
    {
        if (__data_spells.Contains(spell.id)) return;
        __data_spells.Add(spell.id);
        data.add_spell(spell.id);
    }

    /// <summary>
    ///     检查目标修炼体系是否能够升级, 如果能够则会进行一次升级
    /// </summary>
    /// <param name="cultisys_id">目标修炼体系，null为检查所有修炼体系</param>
    /// <exception cref="System.Exception">严格模式下该生物不存在该修炼体系</exception>
    public void check_level_up(string cultisys_id = null)
    {
        if (cultisys_id != null)
        {
            data.get(cultisys_id, out int level, -1);
            if (level < 0)
            {
                if (Constants.Others.strict_mode) throw new Exception("CW_Actor.check_level_up: cultisys level < 0");
                return;
            }

            CultisysAsset cultisys = Manager.cultisys.get(cultisys_id);
            if (level >= cultisys.max_level - 1) return;

            if (cultisys.can_levelup(this, cultisys)) __level_up_and_get_bonus(cultisys, level);

            return;
        }

        int[] cultisys_levels = data.get_cultisys_level();
        for (int i = 0; i < Manager.cultisys.size; i++)
        {
            if (cultisys_levels[i] < 0) continue;
            CultisysAsset cultisys = Manager.cultisys.list[i];

            if (cultisys_levels[i] >= cultisys.max_level - 1) continue;

            if (cultisys.can_levelup(this, cultisys)) __level_up_and_get_bonus(cultisys, cultisys_levels[i]);
        }
    }

    /// <summary>
    ///     升级并获取增益
    /// </summary>
    /// <param name="cultisys">升级的修炼体系</param>
    /// <param name="curr_level">升级前的等级</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __level_up_and_get_bonus(CultisysAsset cultisys, int curr_level)
    {
        curr_level++;
        data.set(cultisys.id, curr_level);

        if (!__learn_spell_from_cultibook()) __learn_spell_generally();
        if (__can_create_blood_on_levelup(cultisys, curr_level)) create_blood();
        if (__can_create_cultibook_on_levelup(cultisys, curr_level)) create_cultibook();
        if (cultisys.external_levelup_bonus != null) _ = cultisys.external_levelup_bonus(this, cultisys, curr_level);

        setStatsDirty();
    }

    /// <summary>
    ///     从法术库中学习法术
    /// </summary>
    private void __learn_spell_generally()
    {
        int[] cultisys_levels = data.get_cultisys_level();
        uint cultisys_types = 0;
        for (int i = 0; i < Manager.cultisys.size; i++)
        {
            if (cultisys_levels[i] < 0) continue;
            cultisys_types |= (uint)Manager.cultisys.list[i].type;
        }

        List<CW_SpellAsset> spells = Manager.spells.get_spells_by_cultisys(cultisys_types);
        if (spells.Count == 0) return;
        // 按照学习概率排序
        Dictionary<CW_SpellAsset, float> spell_float = new();
        foreach (CW_SpellAsset spell in spells)
        {
            if (__data_spells.Contains(spell.id)) continue;
            float chance = spell.learn_check(this, cultisys_types);
            if (chance < 0) continue;
            spell_float.Add(spell, chance);
        }

        IOrderedEnumerable<KeyValuePair<CW_SpellAsset, float>>
            ordered_spell_chances = spell_float.OrderBy(x => x.Value);
        // 尝试学习
        foreach (KeyValuePair<CW_SpellAsset, float> spell_chance in
                 ordered_spell_chances.Where(spell_chance =>
                     Toolbox.randomChance(spell_chance.Value)))
        {
            learn_spell(spell_chance.Key);
            return;
        }
    }

    /// <summary>
    ///     升级时从功法中学习法术, 返回是否成功学习
    /// </summary>
    private bool __learn_spell_from_cultibook()
    {
        Cultibook cultibook = data.get_cultibook();
        if (cultibook == null) return false;

        if (cultibook.spells.Count == 0) return false;

        int[] cultisys_levels = data.get_cultisys_level();
        uint cultisys_types = 0;
        for (int i = 0; i < Manager.cultisys.size; i++)
        {
            if (cultisys_levels[i] < 0) continue;
            cultisys_types |= (uint)Manager.cultisys.list[i].type;
        }

        foreach (string t in cultibook.spells)
        {
            if (__data_spells.Contains(t)) continue;
            CW_SpellAsset spell = Manager.spells.get(t);

            float learn_chance = spell.learn_check(this, cultisys_types);

            if (learn_chance < 0) continue;
            if (Toolbox.randomChance(learn_chance))
            {
                learn_spell(spell);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     能否在本次升级时创建/改良血脉
    /// </summary>
    /// <param name="cultisys">升级的修炼体系</param>
    /// <param name="curr_level">升级后的等级</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool __can_create_blood_on_levelup(CultisysAsset cultisys, int curr_level)
    {
        return curr_level == cultisys.max_level;
    }

    /// <summary>
    ///     能否在本次升级时创建/改良功法
    /// </summary>
    /// <param name="cultisys">升级的修炼体系</param>
    /// <param name="curr_level">升级后的等级</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool __can_create_cultibook_on_levelup(CultisysAsset cultisys, int curr_level)
    {
        return curr_level % Constants.Others.create_cultibook_every_level == 4;
    }

    internal void cw_newCreature()
    {
        data.set_element(CW_Element.get_element_for_set_data(cw_asset.prefer_element, cw_asset.prefer_element_scale));

        if (Constants.Others.new_creature_create_blood) create_blood();
    }

    internal void cw_finalize()
    {
        data.health = int.MaxValue;
        data.set(DataS.soul, float.MaxValue);
        // 暂且不支持直接的血脉修炼体系
        uint allow_cultisys_types = 0b111;
        // 强制添加的修炼体系
        foreach (CultisysAsset cultisys in cw_asset.force_cultisys)
        {
            if ((allow_cultisys_types & (uint)cultisys.type) == 0) continue;
            data.set(cultisys.id, 0);
            allow_cultisys_types &= ~(uint)cultisys.type;
        }

        foreach (CultisysAsset cultisys in cw_asset.allowed_cultisys)
        {
            if ((allow_cultisys_types & (uint)cultisys.type) == 0 || !cultisys.allow(this, cultisys))
                continue;
            data.set(cultisys.id, 0);
            allow_cultisys_types &= ~(uint)cultisys.type;
        }

        foreach (string spell_id in cw_asset.born_spells)
        {
            learn_spell(Manager.spells.get(spell_id));
        }

        setStatsDirty();
    }
}