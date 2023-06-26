namespace Cultivation_Way.Content;

internal class Content_Constants
{
    public const string cultisys_prefix = Constants.Core.mod_prefix + "cultisys_";
    public const string energy_prefix = Constants.Core.mod_prefix + "energy_";
    public const string energy_wakan_id = energy_prefix + "wakan";

    public const string immortal_id = cultisys_prefix + "immortal";
    public const int immortal_max_level = 20;
    public const float immortal_max_wakan_regen = 0.6f;
    public const float immortal_base_cultivelo = 12;


    public const string bushido_id = cultisys_prefix + "bushido";
    public const string energy_bushido_id = energy_prefix + "inforce";
    public const int bushido_max_level = 20;


    public const string soul_id = cultisys_prefix + "soul";
    public const string energy_soul_id = energy_prefix + "soul";
    public const int soul_max_level = 20;
}