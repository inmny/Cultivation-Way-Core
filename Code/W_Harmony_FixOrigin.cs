using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ReflectionUtility;
using Cultivation_Way.Utils;

namespace Cultivation_Way.Content.Harmony
{
    internal class W_Harmony_FixOrigin
    {
        private static Action<Boat, life.AnimationDataBoat> set_animationDataBoat = Utils.CW_ReflectionHelper.create_setter<Boat, life.AnimationDataBoat>("animationDataBoat");
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Boat), "updateTexture")]
        public static bool boat_load_fixed_texture(Boat __instance)
        {
            CW_Actor actor = __instance.GetComponent<CW_Actor>();
            if (string.IsNullOrEmpty(actor.stats.texture_path)) return true;

            life.AnimationDataBoat anim_boat = ActorAnimationLoader.loadAnimationBoat(actor.stats.texture_path);
            set_animationDataBoat(__instance, anim_boat);

            ActorAnimation anim = ((Dictionary<int, ActorAnimation>)Reflection.GetField(typeof(life.AnimationDataBoat), anim_boat, "dict"))[0];

            SpriteAnimation actor_sprite_anim = CW_Actor.get_spriteAnimation(actor);
            actor_sprite_anim.CallMethod("setFrames", Reflection.GetField(typeof(ActorAnimation), anim, "frames"));
            actor_sprite_anim.timeBetweenFrames = (float)Reflection.GetField(typeof(ActorAnimation), anim, "timeBetweenFrames");
            actor_sprite_anim.resetAnim(0);
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "loadTexture")]
        public static bool cancel_load_boat_texture_by_default_actor_loadTexture(ActorBase __instance)
        {
            return !__instance.stats.isBoat;
        }
    }
}
