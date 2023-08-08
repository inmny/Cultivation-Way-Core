using System;
using System.Collections.Generic;

namespace Cultivation_Way.Content;

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
    }

    private static void add_ming()
    {
        Race ming = AssetManager.raceLibrary.clone(Content_Constants.ming_race, SK.human);
        AssetManager.raceLibrary.t = ming;
        ming.path_icon = "ui/Icons/iconMing";
        ming.build_order_id = ming.id;
        ming.nameLocale = Content_Constants.ming_name_locale;
        ming.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.ming_race;
        ming.skin_citizen_male = new List<string> { "unit_male_1" };
        ming.skin_citizen_female = new List<string> { "unit_female_1" };
        ming.skin_warrior = new List<string> { "unit_warrior_1" };
        if (Environment.UserName == "94508")
        {
            ming.name_template_kingdom = "easternhuman_kingdom";
            ming.name_template_city = "easternhuman_city";
            ming.name_template_clan = "easternhuman_clan";
            ming.name_template_culture = "easternhuman_culture";
        }

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
        ming.building_order_keys.Remove(SB.order_bonfire);
        ming.building_order_keys.Add("bonfire_ming", "bonfire_ming");
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
        if (Environment.UserName == "94508")
        {
            yao.name_template_kingdom = "easternhuman_kingdom";
            yao.name_template_city = "easternhuman_city";
            yao.name_template_clan = "easternhuman_clan";
            yao.name_template_culture = "easternhuman_culture";
        }

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
        eastern_human.skin_citizen_male = new List<string> { "unit_male_1" };
        eastern_human.skin_citizen_female = new List<string> { "unit_female_1" };
        eastern_human.skin_warrior = new List<string> { "unit_warrior_1" };
        if (Environment.UserName == "94508")
        {
            eastern_human.name_template_kingdom = "easternhuman_kingdom";
            eastern_human.name_template_city = "easternhuman_city";
            eastern_human.name_template_clan = "easternhuman_clan";
            eastern_human.name_template_culture = "easternhuman_culture";
        }

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
        eastern_human.building_order_keys.Remove(SB.order_bonfire);
        eastern_human.building_order_keys.Add("bonfire_eastern_human", "bonfire_eastern_human");
    }
}