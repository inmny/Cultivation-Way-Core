using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Cultivation_Way.Library;
namespace Cultivation_Way.Tester
{
    internal static class CW_Anim_Tester
    {
        private static bool force_spell(string spell_id, string src_id, string dst_id, int dst_x, int dst_y, float scale)
        {
            CW_Asset_Spell spell_asset = CW_Library_Manager.instance.spells.get(spell_id);
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            WorldTile tile = MapBox.instance.GetTile(dst_x, dst_y);
            if (spell_asset.anim_type != Library.CW_Spell_Animation_Type.CUSTOM)
            {
                spell_asset.damage_action(spell_asset, src_actor, dst_actor, tile, 10);
                spell_asset.anim_action(spell_asset, src_actor, dst_actor, tile, 10);
                //WorldBoxConsole.Console.print("Spell should cast");
                ((CW_Actor)src_actor).fast_data.favorite = true;
            }
            else
            {
                spell_asset.spell_action(spell_asset, src_actor, dst_actor, tile, 10);
            }
            return true;
        }
        private static void spawn_gold_blade()
        {
            spawn_anim("gold_blade_anim", "u_0", "u_1", 0, 0, 0, 0, 1f);
        }
        private static void spawn_gold_escape()
        {
            spawn_anim("gold_escape_anim", "u_0", "u_1", 0, 0, 0, 0, 0.1f);
        }
        private static void spawn_anim_on_specific_units(string anim_id)
        {
            spawn_anim(anim_id, "u_0", "u_1", 0, 0, 0, 0, 1f);
        }
        private static bool spawn_anim(string anim_id, string src_id, string dst_id, int src_x, int src_y, int dst_x, int dst_y, float scale)
        {
            WorldBoxConsole.Console.print("Test anim:" + anim_id + ", scale:" + scale);
            Actor src_actor = MapBox.instance.getActorByID(src_id);
            Actor dst_actor = MapBox.instance.getActorByID(dst_id);
            Vector2 src_vec = (src_actor == null ? new Vector2(src_x, src_y) : src_actor.currentPosition);
            Vector2 dst_vec = (dst_actor == null ? new Vector2(dst_x, dst_y) : dst_actor.currentPosition);

            if (src_x != 0 && src_y != 0) src_vec = new Vector2((float)src_x, (float)src_y);
            if (dst_x != 0 && dst_y != 0) dst_vec = new Vector2(dst_x, dst_y);
            WorldBoxConsole.Console.print(string.Format("src_vec:{0}, dst_vec:{1}", src_vec, dst_vec));


            ModState.instance.effect_manager.spawn_anim(anim_id, src_vec, dst_vec, src_actor, dst_actor, scale);
            return true;
        }
    }
}
