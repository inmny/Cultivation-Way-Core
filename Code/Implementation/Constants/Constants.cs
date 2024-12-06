namespace Cultivation_Way.Implementation;

public class Content_Constants
{
    public const string cultisys_prefix = Constants.Core.mod_prefix + "cultisys_";
    public const string energy_prefix   = Constants.Core.mod_prefix + "energy_";
    public const string energy_wakan_id = energy_prefix             + "wakan";

    public const string immortal_id              = cultisys_prefix + "immortal";
    public const int    immortal_max_level       = 20;
    public const float  immortal_max_wakan_regen = 0.6f;
    public const float  immortal_base_cultivelo  = 1.2f;


    public const string bushido_id               = cultisys_prefix + "bushido";
    public const string energy_bushido_id        = energy_prefix   + "inforce";
    public const int    bushido_max_level        = 20;
    public const float  bushido_max_health_regen = 0.6f;
    public const float  bushido_base_cultivelo   = 5f;
    public const string data_bushido_cultivelo   = "cw_bushido_cultivelo";


    public const string soul_id        = cultisys_prefix + "soul";
    public const string energy_soul_id = energy_prefix   + "soul";
    public const int    soul_max_level = 20;

    public const string blood_id             = cultisys_prefix + "blood";
    public const string energy_blood_id      = energy_prefix   + "blood";
    public const int    blood_max_level      = 20;
    public const float  blood_cultivelo_co_t = 0.01f;
    public const float  blood_cultivelo_co_k = 10;

    public const float default_spell_cost = 0.03f;
    public const float call_ancestor_min_purity = 0.75f;

    public const string nomad_kingdom_prefix = "nomads_";
    public const string eastern_human_id     = "unit_eastern_human";
    public const string eastern_human_race   = "eastern_human";
    public const string eastern_human_name_locale = "Eastern Humans";
    public const string yao_postfix      = "_yao";
    public const string yao_race         = "yao";
    public const string yao_name_locale  = "Yaos";
    public const string ming_id          = "unit_ming";
    public const string ming_race        = "ming";
    public const string ming_name_locale = "Mings";
    public const string wu_id            = "unit_wu";
    public const string wu_race          = "wu";
    public const string wu_name_locale   = "Wus";
}