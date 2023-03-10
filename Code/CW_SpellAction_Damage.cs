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
            float damage = cost * Others.CW_Constants.default_spell_damage_co;
            Others.CW_Enums.CW_AttackType attack_type = Others.CW_Enums.CW_AttackType.Other;
            if ((spell_asset.tags & (1ul << (int)CW_Spell_Tag.IMMORTAL)) > 0)
            {
                //damage *= Others.CW_Constants.default_spell_damage_co;
                attack_type = Others.CW_Enums.CW_AttackType.Spell;
            }
            else if((spell_asset.tags & (1ul << (int)CW_Spell_Tag.BUSHIDO)) > 0)
            {
                damage *= spell_asset.free_val;
            }
            switch (spell_asset.target_type)
            {
                case CW_Spell_Target_Type.TILE:
                    List<Actor> targets_on_tile = pTargetTile.units;
                    foreach (CW_Actor target in targets_on_tile)
                    {
                        if(Utils.CW_SpellHelper.is_enemy(pUser, target))
                            Utils.CW_SpellHelper.cause_damage_to_target(pUser, target, damage, attack_type, true);
                    }
                    Building target_building = pTargetTile.building;
                    if (Utils.CW_SpellHelper.is_enemy(pUser, target_building))
                        Utils.CW_SpellHelper.cause_damage_to_target(pUser, target_building, damage, attack_type, true);
                    break;

                case CW_Spell_Target_Type.CHUNK:
                    Utils.CW_SpellHelper.__find_kingdom_enemies_in_chunk(pTargetTile.chunk, pUser==null?null:pUser.kingdom);
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
