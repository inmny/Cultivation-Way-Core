using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using Cultivation_Way.Animation;
using UnityEngine;

namespace Cultivation_Way.Actions
{
    public class CW_SpellAction_Anim
    {
        public static void no_anim(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            return;
        }
        public static void default_anim(CW_Asset_Spell spell_asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            switch (spell_asset.anim_type)
            {
                case CW_Spell_Animation_Type.ON_USER:
                    {
                        if (pUser == null) return;
                        CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pUser.currentPosition,pUser.currentPosition, pUser, pUser, 1f);
                        if (anim == null) return;
                        anim.cost_for_spell = cost;
                        break;
                    }
                case CW_Spell_Animation_Type.ON_TARGET:
                    {
                        CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pTarget == null ? pTargetTile.pos : pTarget.currentPosition, pTarget == null ? pTargetTile.pos : pTarget.currentPosition, pUser, pTarget, 1f);
                        if (anim == null) return;
                        anim.cost_for_spell = cost;
                        break;
                    }
                    
                case CW_Spell_Animation_Type.USER_TO_TARGET:
                    {
                        if (pUser != null && (pTarget != null || pTargetTile!=null))
                        {
                            CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, pUser.currentPosition, pTarget == null ? pTargetTile.pos : pTarget.currentPosition, pUser, pTarget, 1f);
                            if (anim == null) return;
                            anim.cost_for_spell = cost;
                        }
                        break;
                    }
                case CW_Spell_Animation_Type.DOWNWARD:
                    {
                        if (pTarget != null || pTargetTile != null)
                        {
                            Vector2 dst_vec = pTarget == null ? pTargetTile.pos : pTarget.currentPosition;
                            CW_SpriteAnimation anim = CW_EffectManager.instance.spawn_anim(spell_asset.anim_id, new Vector2(dst_vec.x, dst_vec.y + spell_asset.free_val), new Vector2(dst_vec.x, dst_vec.y - 0.5f), pUser, pTarget, 1f);
                            if (anim == null) return;
                            anim.cost_for_spell = cost;
                        }
                        break;
                    }
                   
            }
            
        }
    }
}
