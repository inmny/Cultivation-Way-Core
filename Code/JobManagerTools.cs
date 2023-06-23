using Cultivation_Way.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    /// <summary>
    /// 按原版框架拓展原版JobManager
    /// </summary>
    internal static class JobManagerTools
    {
        private static int _last_month = 0;
        public static void add_actor_update_month_job(BatchActors batch)
        {
            batch.addJob(batch.c_main, new JobUpdater(batch.update_month_job), JobType.Parallel, "cw_update_month", 0);
        }
        private static void update_month_job(this BatchActors batch)
        {
            if (World.world.mapStats.getCurrentMonth() != _last_month) _last_month = World.world.mapStats.getCurrentMonth();
            else return;

            if (!batch.check(batch._cur_container))
            {
                return;
            }
            List<Actor> list = batch._list;
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                ((CW_Actor)list[i]).update_month();
            }
        }
    }
}
