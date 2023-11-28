using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;

internal static class CW_ActorJobLibrary
{
    public static void init()
    {
        ActorJob job = new();
        job.id = CW_AIS.job_artificer;
        job.addTask("wait");
        job.addTask("city_idle_walking");
        job.addTask("wait");
        job.addTask("try_to_return_home");
        job.addTask("wait");
        job.addTask(CW_AIS.task_cw_make_items);
        AssetManager.job_actor.add(job);
    }
}