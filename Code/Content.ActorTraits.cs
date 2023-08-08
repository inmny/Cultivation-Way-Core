using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Content;

internal static class ActorTraits
{
    public static void init()
    {
        ActorTrait trait;

        trait = new ActorTrait
        {
            id = Constants.Core.mod_prefix + "curse_immune",
            opposite = "cursed",
            group_id = TraitGroup.body,
            path_icon = "ui/icons/iconCurseImmune"
        };
        add(trait);

        trait = new ActorTrait
        {
            id = Constants.Core.mod_prefix + "all_elements",
            group_id = TraitGroup.body,
            path_icon = "ui/icons/iconElement",
            achievement_id = Constants.Core.mod_prefix + "achievementComplete",
            unlocked_with_achievement = true,
            action_special_effect = (actor, tile) =>
            {
                if (!actor.isAlive()) return false;
                CW_Actor self = (CW_Actor)actor;
                CW_Element element = self.data.get_element();
                if (element.comp_type() == Constants.Core.mod_prefix + "uniform") return false;
                self.data.set_element(new CW_Element(new[] { 20, 20, 20, 20, 20 }));

                uint allow_cultisys_types = 0b111;
                foreach (CultisysAsset cultisys in self.cw_asset.allowed_cultisys)
                {
                    self.data.get(cultisys.id, out int level, -1);
                    if (level >= 0)
                    {
                        allow_cultisys_types &= ~(uint)cultisys.type;
                        continue;
                    }

                    if ((allow_cultisys_types & (uint)cultisys.type) == 0 || !cultisys.allow(self, cultisys))
                        continue;
                    self.data.set(cultisys.id, 0);
                    allow_cultisys_types &= ~(uint)cultisys.type;
                }

                return true;
            }
        };
        add(trait);
    }

    private static void add(ActorTrait trait)
    {
        AssetManager.traits.add(trait);
    }
}