using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Content;
using Cultivation_Way.Library;
namespace Cultivation_Way.Utils
{
    public static class CW_Utils_ActorTools
    {
        internal static CW_Actor spawn_actor(string stats_id, WorldTile tile, ActorStatus origin_status, CW_ActorData cw_data, float height)
        {
            CW_ActorStats actor_stats = CW_Library_Manager.instance.units.get(stats_id);
            CW_Actor actor = UnityEngine.Object.Instantiate(W_Content_Helper.get_actor_prefab("actors/" + actor_stats.origin_stats.prefab)).gameObject.GetComponent<CW_Actor>();
            actor.transform.name = actor_stats.id;
            actor.setWorld();
            actor.cw_stats = actor_stats;
            actor.loadStats(actor_stats.origin_stats);
            actor.new_creature = false;
            actor.cw_cur_stats = new CW_BaseStats(CW_Actor.get_curstats(actor));
            actor.cw_data = cw_data;
            actor.fast_data = origin_status;
            actor.cw_status = cw_data.status;
            actor.setData(origin_status);
            CW_Actor.set_professionAsset(actor, AssetManager.professions.get(origin_status.profession));
            actor.transform.position = tile.posV3;
            CW_Actor.func_spawnOn(actor, tile, height);
            CW_Actor.func_create(actor);

            Cultivation_Way.Content.Harmony.W_Harmony_Actor.__actor_updateStats(actor);

            if (actor.stats.kingdom != "") CW_Actor.func_setKingdom(actor, MapBox.instance.kingdoms.dict_hidden[actor.stats.kingdom]);
            else
            {
                CW_Actor.func_setKingdom(actor, MapBox.instance.kingdoms.dict_hidden["nomads_" + actor.stats.race]);
            }
            actor.transform.parent = actor.stats.hideOnMinimap?W_Content_Helper.transformUnits: W_Content_Helper.transformCreatures;

            MapBox.instance.units.Add(actor);
            return actor;
        }
    }
}
