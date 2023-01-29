using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Actions
{
    public class CW_SpellAction_Damage
    {
        public static void no_damage(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            return;
        }
        public static void defualt_damage(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            float damage = cost * Others.CW_Constants.default_spell_damage_co * spell_asset.free_val;
            Others.CW_Enums.CW_AttackType attack_type = Others.CW_Enums.CW_AttackType.Other;
            if ((spell_asset.tags & (1ul << (int)CW_Spell_Tag.IMMORTAL)) > 0)
            {
                damage *= cost;
                attack_type = Others.CW_Enums.CW_AttackType.Spell;
            }
            switch (spell_asset.target_type)
            {
                case CW_Spell_Target_Type.TILE:
                    List<Actor> targets_on_tile = pTargetTile.units;
                    foreach (CW_Actor target in targets_on_tile)
                    {
                        if(Utils.CW_SpellHelper.is_enemy(pUser, target))
                            target.get_hit(damage, true, attack_type, pUser, true);
                    }
                    Building target_building = pTargetTile.building;
                    if (target_building != null && Utils.CW_SpellHelper.is_enemy(pUser, target_building))
                        CW_Building.func_getHit(target_building, damage, true, (AttackType)attack_type, pUser, true);
                    break;
                case CW_Spell_Target_Type.CHUNK:
                    Utils.CW_SpellHelper.__find_kingdom_enemies_in_chunk(pTargetTile.chunk, pUser.kingdom);
                    foreach(List<BaseSimObject> list in Utils.CW_SpellHelper.temp_list_objects_enemies)
                    {
                        for(int i = 0; i < list.Count; i++)
                        {
                            Utils.CW_SpellHelper.cause_damage_to_target(pUser, list[i], damage, attack_type);
                        }
                    }

                    break;
                default:
                    Utils.CW_SpellHelper.cause_damage_to_target(pUser, pTarget, damage, attack_type);
                    break;
            }
        }
    }
}
