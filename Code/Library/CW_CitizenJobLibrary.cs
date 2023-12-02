using Cultivation_Way.Constants;

namespace Cultivation_Way.Library;

internal static class CW_CitizenJobLibrary
{
    public static CitizenJobAsset artificer;

    public static void init()
    {
        artificer = add(new CitizenJobAsset
        {
            id = CW_AIS.job_artificer,
            common_job = true,
            debug_option = DebugOption.CitizenJobBlacksmith,
            path_icon = "ui/Icons/citizen_jobs/iconCitizenJobBlacksmith"
        });
    }

    public static CitizenJobAsset add(CitizenJobAsset pAsset)
    {
        AssetManager.citizen_job_library.add(pAsset);
        if (pAsset.common_job)
        {
            if (pAsset.priority_no_food > 0)
            {
                AssetManager.citizen_job_library.list_priority_high_food.Add(pAsset);
            }

            if (pAsset.priority > 0)
            {
                AssetManager.citizen_job_library.list_priority_high.Add(pAsset);
            }
            else
            {
                AssetManager.citizen_job_library.list_priority_normal.Add(pAsset);
            }

            pAsset.unit_job_default = pAsset.id;
        }

        return pAsset;
    }
}