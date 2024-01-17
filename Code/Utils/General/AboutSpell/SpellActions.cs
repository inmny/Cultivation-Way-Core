using System.Data;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Cultivation_Way.Others;

namespace Cultivation_Way.Utils.General.AboutSpell;

/// <summary>
///     提供一般法术的spell_action
/// </summary>
public static class SpellActions
{
    /// <summary>
    ///     生成一个给予状态的spell_action
    /// </summary>
    /// <param name="status_id">给予的状态id</param>
    /// <returns></returns>
    /// <exception cref="RowNotInTableException">When target is Building</exception>
    public static SpellAction generate_give_status_spell_action(string status_id)
    {
        return (asset, user, target, tile, cost) =>
        {
            if (target == null) return;
            switch (target.objectType)
            {
                case MapObjectType.Actor:
                    ((CW_Actor)target).AddStatus(status_id, user);
                    break;
                case MapObjectType.Building:
                    throw new RowNotInTableException();
            }
        };
    }

    /// <summary>
    ///     默认的伤害计算
    /// </summary>
    /// <param name="spell_asset"></param>
    /// <param name="user"></param>
    /// <param name="target"></param>
    /// <param name="target_tile"></param>
    /// <param name="cost"></param>
    public static void default_damage_on(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target,
        WorldTile target_tile, float cost)
    {
    }
}