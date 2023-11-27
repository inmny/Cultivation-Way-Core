namespace Cultivation_Way.Implementation;
/// <summary>
///     建筑字符串
/// </summary>
public static class CW_SB
{
    public const string smelt_mill = "smelt_mill";
    public static readonly string eh_smelt_mill = C(smelt_mill, Content_Constants.eastern_human_race);
    public static readonly string order_smelt_mill = C("order", smelt_mill);

    private static string C(params string[] pStrings)
    {
        return string.Join("_", pStrings);
    }
}