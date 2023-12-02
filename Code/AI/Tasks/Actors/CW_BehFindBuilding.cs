using ai.behaviours;

namespace Cultivation_Way.AI.Tasks.Actors;

public class CW_BehFindBuilding : BehCity
{
    // Token: 0x04002178 RID: 8568
    private string type;

    // Token: 0x04002179 RID: 8569
    private readonly string[] types;

    public CW_BehFindBuilding(string pType)
    {
        type = pType;
        if (pType.Contains(","))
        {
            types = pType.Split(',');
        }
    }

    // Token: 0x06001D6A RID: 7530 RVA: 0x000E6C44 File Offset: 0x000E4E44
    public override BehResult execute(Actor pActor)
    {
        if (types != null)
        {
            type = types.GetRandom();
        }

        pActor.beh_building_target = pActor.city.getBuildingType(type, true, true);
        return BehResult.Continue;
    }
}