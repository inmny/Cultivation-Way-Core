using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal static class W_Content_GodPower
    {
        internal static void add_god_powers()
        {
            add_spawn_EasternHuman();
            add_spawn_Yao();
        }

        private static void add_spawn_Yao()
        {
            GodPower power = AssetManager.powers.clone("spawnYao", "_spawnActor");
            power.name = "Yaos";
            power.spawnSound = "spawnOrc";
            power.actorStatsId = "unit_Yao,";// TODO:其他妖族
            power.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
        }

        private static void add_spawn_EasternHuman()
        {
            GodPower power = AssetManager.powers.clone("spawnEastern_Human", "_spawnActor");
            power.name = "Eastern Humans";
            power.spawnSound = "spawnHuman";
            power.actorStatsId = "unit_eastern_human";
            power.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
        }
    }
}
