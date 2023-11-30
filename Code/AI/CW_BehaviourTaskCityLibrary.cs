using ai.behaviours;
using Cultivation_Way.AI.Tasks.Cities;

namespace Cultivation_Way.AI;

internal static class CW_BehaviourTaskCityLibrary
{
    public static void init()
    {
        BehaviourTaskCity do_checks = AssetManager.tasks_city.get("do_checks");
        do_checks.addBeh(new CW_CityBehCheckArtificer((CityBehCheckCitizenTasks)do_checks.list[do_checks.list.Count - 2]));
        do_checks.addBeh(new CityBehRandomWait(0.1f, 1));
    }
}