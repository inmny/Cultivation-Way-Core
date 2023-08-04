using System;
using System.Collections.Generic;

namespace Cultivation_Way.Content;

internal static class Races
{
    public static void init()
    {
        add_eastern_human();
        add_yao();
        AssetManager.raceLibrary.post_init();
    }

    private static void add_yao()
    {
        Race yao = AssetManager.raceLibrary.clone(Content_Constants.yao_race, SK.human);
        AssetManager.raceLibrary.t = yao;
        yao.path_icon = "ui/Icons/iconYaos";
        yao.nameLocale = "Yaos";
        yao.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.yao_race;
        yao.skin_citizen_male = new List<string> { "unit_male_1" };
        yao.skin_citizen_female = new List<string> { "unit_female_1" };
        yao.skin_warrior = new List<string> { "unit_warrior_1" };
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
        eastern_human.nameLocale = "Eastern Humans";
        eastern_human.build_order_id = eastern_human.id;
        eastern_human.nomad_kingdom_id = Content_Constants.nomad_kingdom_prefix + Content_Constants.eastern_human_race;
        eastern_human.skin_citizen_male = new List<string> { "unit_male_1" };
        eastern_human.skin_citizen_female = new List<string> { "unit_female_1" };
        eastern_human.skin_warrior = new List<string> { "unit_warrior_1" };
        if (Environment.UserName == "94508")
        {
            eastern_human.name_template_kingdom = "easternhuman_kingdom";
            eastern_human.name_template_city = "easternhuman_city";
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