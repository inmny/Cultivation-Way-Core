using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cultivation_Way.Library;
using ReflectionUtility;
using Cultivation_Way.Utils;
namespace Cultivation_Way.Tester
{
    internal static class CW_Anim_Tester
    {
        private static Func<StackEffects, Vector3, Vector3, string, float, Projectile> s_p = (Func<StackEffects, Vector3, Vector3, string, float, Projectile>)CW_ReflectionHelper.get_method<StackEffects>("startProjectile");
        private static bool force_spell(string spell_id, string src_id, string dst_id, int dst_x, int dst_y, float cost)
        {
            CW_Asset_Spell spell_asset = CW_Library_Manager.instance.spells.get(spell_id);
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            WorldTile tile = (dst_x==0 && dst_y==0)?dst_actor.currentTile:MapBox.instance.GetTile(dst_x, dst_y);
            if (spell_asset.anim_type != Library.CW_Spell_Animation_Type.CUSTOM)
            {
                if(spell_asset.damage_action!=null)spell_asset.damage_action(spell_asset, src_actor, dst_actor, tile, 10);
                if (spell_asset.anim_action != null) spell_asset.anim_action(spell_asset, src_actor, dst_actor, tile, 10);
                //WorldBoxConsole.Console.print("Spell should cast");
                ((CW_Actor)src_actor).fast_data.favorite = true;
            }
            else
            {
                spell_asset.spell_action(spell_asset, src_actor, dst_actor, tile, cost);
            }
            return true;
        }
        private static void spawn_gold_blade()
        {
            spawn_anim("gold_blade_anim", "u_0", "u_1", 0, 0, 0, 0, 1f,0);
        }
        private static void spawn_gold_escape()
        {
            spawn_anim("gold_escape_anim", "u_0", "u_1", 0, 0, 0, 0, 0.1f,0);
        }
        private static void spawn_gold_swords(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                spawn_anim("single_gold_sword_anim", "u_0", "u_1", 0, 0, 0, 0, 1f,0);
            }
        }
        private static void spawn_arrows(int amount, string src_id, string dst_id)
        {
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            if(src_actor == null || dst_actor == null)
            {
                Debug.LogError(string.Format("src:{0},dst:{1}", src_actor, dst_actor));
            }
            try
            {
                for (int i = 0; i < amount; i++)
                {
                    s_p(MapBox.instance.stackEffects, src_actor.currentPosition, dst_actor.currentPosition, "arrow", 0);
                }
            }
            catch(Exception e)
            {
                Debug.LogError(e.StackTrace);
            }
        }
        private static void spawn_anim_on_specific_units(string anim_id)
        {
            spawn_anim(anim_id, "u_0", "u_1", 0, 0, 0, 0, 1f, 0);
        }
        private static bool spawn_anim(string anim_id, string src_id, string dst_id, int src_x, int src_y, int dst_x, int dst_y, float scale, float cost)
        {
            WorldBoxConsole.Console.print("Test anim:" + anim_id + ", scale:" + scale);
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            Vector2 src_vec = (src_actor == null ? new Vector2(src_x, src_y) : src_actor.currentPosition);
            Vector2 dst_vec = (dst_actor == null ? new Vector2(dst_x, dst_y) : dst_actor.currentPosition);

            if (src_x != 0 && src_y != 0) src_vec = new Vector2((float)src_x, (float)src_y);
            if (dst_x != 0 && dst_y != 0) dst_vec = new Vector2(dst_x, dst_y);
            WorldBoxConsole.Console.print(string.Format("src_vec:{0}, dst_vec:{1}", src_vec, dst_vec));


            Animation.CW_SpriteAnimation anim = ModState.instance.effect_manager.spawn_anim(anim_id, src_vec, dst_vec, src_actor, dst_actor, scale);
            if (anim == null) return false;
            anim.cost_for_spell = cost;
            return true;
        }
    }
}
