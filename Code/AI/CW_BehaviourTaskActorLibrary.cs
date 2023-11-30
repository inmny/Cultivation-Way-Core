using ai.behaviours;
using Cultivation_Way.AI.Tasks.Actors;
using Cultivation_Way.Constants;
using Cultivation_Way.Implementation;

namespace Cultivation_Way.AI;

internal static class CW_BehaviourTaskActorLibrary
{
    public static void init()
    {
        BehaviourTaskActor cw_make_items = new BehaviourTaskActor()
        {
            id = CW_AIS.task_cw_make_items
        };
        cw_make_items.addBeh(new CW_BehFindBuilding(CW_SB.smelt_mill));
        cw_make_items.addBeh(new BehFindRandomFrontBuildingTile());
        cw_make_items.addBeh(new BehGoToTileTarget());
        cw_make_items.addBeh(new CW_BehPrepareMakeItem());
        cw_make_items.addBeh(new CW_BehMakeItem(3, 10));
        cw_make_items.addBeh(new CW_BehFinishMakeItem());
        AssetManager.tasks_actor.add(cw_make_items);
    }
}