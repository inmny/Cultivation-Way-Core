using ai.behaviours;
using Cultivation_Way.Implementation;
using Cultivation_Way.Library;

namespace Cultivation_Way.AI.Tasks.Cities;

public class CW_CityBehCheckArtificer : BehaviourActionCity
{
    private CityBehCheckCitizenTasks checkCitizen;

    public CW_CityBehCheckArtificer(CityBehCheckCitizenTasks pCheckCitizen)
    {
        checkCitizen = pCheckCitizen;
    }

    public override BehResult execute(City pCity)
    {
        //if (checkCitizen._citizens_left == 0) return BehResult.Continue;

        if (!pCity.hasBuildingType(CW_SB.smelt_mill)) return BehResult.Continue;

        CitizenJobs jobs = pCity.jobs;
        if (jobs.countCurrentJobs(CW_CitizenJobLibrary.artificer) >= 1) return BehResult.Continue;

        //checkCitizen._citizens_left--;
        jobs.addToJob(CW_CitizenJobLibrary.artificer, 1);

        return BehResult.Continue;
    }
}