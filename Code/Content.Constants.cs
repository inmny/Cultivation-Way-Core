namespace Cultivation_Way.Content;

internal class Content_Constants
{
    public const string cultisys_prefix = Constants.Core.mod_prefix + "cultisys_";
    public const string energy_prefix = Constants.Core.mod_prefix + "energy_";
    public const string energy_wakan_id = energy_prefix + "wakan";

    public const string immortal_id = cultisys_prefix + "immortal";
    public const int immortal_max_level = 20;
    public const float immortal_max_wakan_regen = 0.6f;
    public const float immortal_base_cultivelo = 1.2f;


    public const string bushido_id = cultisys_prefix + "bushido";
    public const string energy_bushido_id = energy_prefix + "inforce";
    public const int bushido_max_level = 20;
    public const float bushido_max_health_regen = 0.6f;
    public const float bushido_base_cultivelo = 5f;
    public const string data_bushido_cultivelo = "cw_bushido_cultivelo";


    public const string soul_id = cultisys_prefix + "soul";
    public const string energy_soul_id = energy_prefix + "soul";
    public const int soul_max_level = 20;

    public const float default_spell_cost = 30f;
    public const float call_ancestor_min_purity = 0.75f;

    public const string nomad_kingdom_prefix = "nomads_";
    public const string eastern_human_id = "unit_eastern_human";
    public const string eastern_human_race = "eastern_human";
    public const string eastern_human_name_locale = "Eastern Humans";
    public const string yao_postfix = "_yao";
    public const string yao_race = "yao";
    public const string yao_name_locale = "Yaos";
}