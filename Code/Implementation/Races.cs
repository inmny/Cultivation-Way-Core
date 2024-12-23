using System.Collections.Generic;

namespace Cultivation_Way.Implementation;

internal static class Races
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
        add_ming();
        add_wu();
        AssetManager.raceLibrary.post_init();
    }

    private static void add_wu()
    {
        Race wu = AssetManager.raceLibrary.clone(Content_Constants.wu_race, SK.human);
        AssetManager.raceLibrary.t = wu;
        wu.path_icon = "ui/Icons/iconWus";
        wu.nameLocale = Content_Constants.wu_name_locale;
        wu.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.wu_race;
        wu.skin_citizen_male = new List<string> { "unit_male_1" };
        wu.skin_citizen_female = new List<string> { "unit_female_1" };
        wu.skin_warrior = new List<string> { "unit_warrior_1" };
#if 一米_中文名
        wu.name_template_city = "wu_city";
        wu.name_template_clan = "wu_clan";
        wu.name_template_culture = "wu_culture";
        wu.name_template_kingdom = "wu_kingdom";
#endif

        AssetManager.raceLibrary.setPreferredStatPool("diplomacy#5,warfare#5,stewardship#5,intelligence#5");
        AssetManager.raceLibrary.setPreferredFoodPool(
            "berries#5,bread#5,fish#5,meat#2,sushi#2,jam#1,cider#1,ale#2,burger#1,pie#1,tea#2");
        AssetManager.raceLibrary.addPreferredWeapon("stick", 5);
        AssetManager.raceLibrary.addPreferredWeapon("sword", 5);
        AssetManager.raceLibrary.addPreferredWeapon("axe", 2);
        AssetManager.raceLibrary.addPreferredWeapon("spear", 2);
        AssetManager.raceLibrary.addPreferredWeapon("bow", 5);
        AssetManager.raceLibrary.cloneBuildingKeys(SK.human, wu.id);
    }

    private static void add_ming()
    {
        Race ming = AssetManager.raceLibrary.clone(Content_Constants.ming_race, SK.human);
        AssetManager.raceLibrary.t = ming;
        ming.path_icon = "ui/Icons/iconMings";
        ming.build_order_id = ming.id;
        ming.nameLocale = Content_Constants.ming_name_locale;
        ming.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.ming_race;
        ming.skin_citizen_male = new List<string> { "unit_male_1" };
        ming.skin_citizen_female = new List<string> { "unit_female_1" };
        ming.skin_warrior = new List<string> { "unit_warrior_1" };
#if 一米_中文名
        ming.name_template_city = "ming_city";
        ming.name_template_clan = "ming_clan";
        ming.name_template_culture = "ming_culture";
        ming.name_template_kingdom = "ming_kingdom";
#endif

        AssetManager.raceLibrary.setPreferredStatPool("diplomacy#5,warfare#5,stewardship#5,intelligence#5");
        AssetManager.raceLibrary.setPreferredFoodPool(
            "berries#5,bread#5,fish#5,meat#2,sushi#2,jam#1,cider#1,ale#2,burger#1,pie#1,tea#2");
        AssetManager.raceLibrary.addPreferredWeapon("stick", 5);
        AssetManager.raceLibrary.addPreferredWeapon("sword", 5);
        AssetManager.raceLibrary.addPreferredWeapon("axe", 2);
        AssetManager.raceLibrary.addPreferredWeapon("spear", 2);
        AssetManager.raceLibrary.addPreferredWeapon("bow", 5);
        AssetManager.raceLibrary.cloneBuildingKeys(SK.human, ming.id);
        AssetManager.race_build_orders.clone(ming.build_order_id, "kingdom_base");
        ming.building_order_keys[SB.order_bonfire] = "bonfire_ming";
    }

    private static void add_yao()
    {
        Race yao = AssetManager.raceLibrary.clone(Content_Constants.yao_race, SK.human);
        AssetManager.raceLibrary.t = yao;
        yao.path_icon = "ui/Icons/iconYaos";
        yao.nameLocale = Content_Constants.yao_name_locale;
        yao.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.yao_race;
        yao.skin_citizen_male = new List<string> { "unit_male_1" };
        yao.skin_citizen_female = new List<string> { "unit_female_1" };
        yao.skin_warrior = new List<string> { "unit_warrior_1" };
#if 一米_中文名
        yao.name_template_city = "yao_city";
        yao.name_template_clan = "yao_clan";
        yao.name_template_culture = "yao_culture";
        yao.name_template_kingdom = "yao_kingdom";
#endif

        AssetManager.raceLibrary.setPreferredStatPool("diplomacy#5,warfare#5,stewardship#5,intelligence#5");
        AssetManager.raceLibrary.setPreferredFoodPool(
            "berries#5,bread#5,fish#5,meat#2,sushi#2,jam#1,cider#1,ale#2,burger#1,pie#1,tea#2");
        AssetManager.raceLibrary.addPreferredWeapon("stick", 5);
        AssetManager.raceLibrary.addPreferredWeapon("sword", 5);
        AssetManager.raceLibrary.addPreferredWeapon("axe", 2);
        AssetManager.raceLibrary.addPreferredWeapon("spear", 2);
        AssetManager.raceLibrary.addPreferredWeapon("bow", 5);
        AssetManager.raceLibrary.cloneBuildingKeys(SK.human, yao.id);
    }

    private static void add_eastern_human()
    {
        Race eastern_human = AssetManager.raceLibrary.clone(Content_Constants.eastern_human_race, SK.human);
        AssetManager.raceLibrary.t = eastern_human;
        eastern_human.path_icon = "ui/Icons/iconEastern_Humans";
        eastern_human.nameLocale = Content_Constants.eastern_human_name_locale;
        eastern_human.build_order_id = eastern_human.id;
        eastern_human.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.eastern_human_race;
        eastern_human.skin_citizen_male = new List<string>
        {
            "unit_male_1", "unit_male_chaoxian", "unit_male_chunqiu", "unit_male_fusang", "unit_male_ming",
            "unit_male_qing", "unit_male_song",
            "unit_male_tang", "unit_male_yuan"
        };
        eastern_human.skin_citizen_female = new List<string>
        {
            "unit_female_1", "unit_female_chaoxian", "unit_female_chunqiu", "unit_female_fusang", "unit_female_ming",
            "unit_female_qing", "unit_female_song",
            "unit_female_tang", "unit_female_yuan"
        };
        eastern_human.skin_warrior = new List<string>
        {
            "unit_warrior_1", "unit_warrior_chaoxian", "unit_warrior_chunqiu", "unit_warrior_fusang",
            "unit_warrior_ming", "unit_warrior_qing", "unit_warrior_song",
            "unit_warrior_tang", "unit_warrior_yuan"
        };
#if 一米_中文名
        eastern_human.name_template_city = "eastern_human_city";
        eastern_human.name_template_clan = "eastern_human_clan";
        eastern_human.name_template_culture = "eastern_human_culture";
        eastern_human.name_template_kingdom = "eastern_human_kingdom";
#endif

        AssetManager.raceLibrary.setPreferredStatPool("diplomacy#5,warfare#5,stewardship#5,intelligence#5");
        AssetManager.raceLibrary.setPreferredFoodPool(
            "berries#5,bread#5,fish#5,meat#2,sushi#2,jam#1,cider#1,ale#2,burger#1,pie#1,tea#2");
        AssetManager.raceLibrary.addPreferredWeapon("stick", 5);
        AssetManager.raceLibrary.addPreferredWeapon("sword", 5);
        AssetManager.raceLibrary.addPreferredWeapon("axe", 2);
        AssetManager.raceLibrary.addPreferredWeapon("spear", 2);
        AssetManager.raceLibrary.addPreferredWeapon("bow", 5);
        AssetManager.raceLibrary.cloneBuildingKeys(SK.human, eastern_human.id);
        AssetManager.race_build_orders.clone(eastern_human.build_order_id, "kingdom_base");
        eastern_human.building_order_keys[SB.order_bonfire] = "bonfire_eastern_human";
        eastern_human.building_order_keys[CW_SB.order_smelt_mill] = CW_SB.eh_smelt_mill;
    }
}