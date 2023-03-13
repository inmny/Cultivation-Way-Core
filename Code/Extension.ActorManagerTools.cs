using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
namespace Cultivation_Way.Extension
{
    internal static class ActorManagerTools
    {
        private static Action<ActorManager, string, Actor, WorldTile, float> __finalizeActor = (Action<ActorManager, string, Actor, WorldTile, float>)ReflectionHelper.get_method<ActorManager>("finalizeActor");
        public static void finalizeActor(this ActorManager actor_manager, string asset_id, Actor actor, WorldTile tile, float z_height)
        {
            __finalizeActor(actor_manager, asset_id, actor, tile, z_height);
        }
    }
}
