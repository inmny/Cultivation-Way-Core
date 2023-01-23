using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal static class W_Content_KingdomAndRace
    {
        internal static void add_kingdoms_and_races()
        {
            add_kingdoms();
            add_races();
        }

        private static void add_races()
        {
            add_eastern_human_race();
        }

        private static void add_eastern_human_race()
        {
            CW_Asset_Race race = new CW_Asset_Race();
            race.id = "eastern_human";
            race.origin_asset = new Race();
            Race origin = race.origin_asset;
            origin.banner_id = "human";                         // TODO:旗帜
            origin.buildingPlacements = BuildingPlacements.Random;      // 随机放置建筑
            origin.build_order_id = "eastern_human";                    // 建筑顺序
            origin.civilization = true;                                 // 城镇化，能够随机产生特质、按性别产生名字、作为第一个居民能够设置国家的种族
            origin.civ_baseArmy = 10;                                   // 城市的基础军队规模
            origin.civ_baseCities = 9;                                  // 国家可容纳的基础最大城市数量
            origin.civ_base_zone_range = 7;                             // 城市的基础区域范围
            origin.color = Color.black;                               // TODO: 0.14.3 游戏未使用
            origin.colorBorder = Color.black;                         // TODO: 0.14.3 游戏未使用
            origin.colorBorderOut = Color.black;                      // TODO: 0.14.3 游戏未使用
            origin.color_abandoned = Color.black;                     // TODO: 0.14.3 游戏未使用
            origin.culture_colors = List.Of(new string[] {
            "#FF695B", "#596CFF", "#AD3AFF", "#FF4FC4", "#FF423F", "#A6FF47", "#2BFFA3", "#28F7FF", "#66CEFF", "#1D5B44",
            "#594555", "#2B313D"
            });                                                         // 文化颜色
            origin.culture_decors = new List<string> { "cultures/culture_decor_0", "cultures/culture_decor_1", "cultures/culture_decor_2", "cultures/culture_decor_3", "cultures/culture_decor_4", "cultures/culture_decor_5", "cultures/culture_decor_6", "cultures/culture_decor_7", "cultures/culture_decor_8" };           // 文化窗口装饰
            origin.culture_elements = new List<string> { "cultures/culture_element_0", "cultures/culture_element_1", "cultures/culture_element_2", "cultures/culture_element_3", "cultures/culture_element_4", "cultures/culture_element_5", "cultures/culture_element_6", "cultures/culture_element_7" };                          // 文化窗口元素
            origin.culture_forbidden_tech = new List<string>();         // 文化禁用科技
            origin.culture_knowledge_gain_base = 2;                     // 文化知识获取基础量
            origin.culture_knowledge_gain_per_intelligence = 2.5f;      // 每点领导人智力增加的文化知识获取量
            origin.culture_knowledge_gain_rate = 0.1f;                  // 总的领导人增加的文化知识量的系数
            origin.culture_rate_tech_limit = 7;                         // 文化稀有科技数量限制, "rare"
            origin.hateRaces = "Yao";                                   // 敌对种族
            origin.nameLocale = "Eastern Humans";                       // 统计数据中使用的id
            origin.name_template_city = "human_city";           // TODO:城市命名模板
            origin.name_template_culture = "human_culture";     // TODO:文化命名模板
            origin.name_template_kingdom = "human_kingdom";     // TODO:城市命名模板
            origin.nature = false;                                      // TODO: 待研究
            origin.nomad_kingdom_id = "nomads_eastern_human";           // 种族的默认隐藏国家
            origin.path_icon = "ui/Icons/iconEastern_Humans";           // 种族图标
            setPreferredStatPool(origin, "diplomacy#2,warfare#5,stewardship#5,intelligence#5");     // 属性偏好
            setPreferredFoodPool(origin, "berries#5,bread#5,fish#5,meat#2,sushi#2,jam#1,cider#1,ale#2,burger#1,pie#1,tea#2");                                                       // 食物偏好
            addPreferredWeapon(origin, "stick", 5);                     // 武器偏好
            addPreferredWeapon(origin, "sword", 5);                     // 武器偏好
            addPreferredWeapon(origin, "axe", 2);                       // 武器偏好
            addPreferredWeapon(origin, "spear", 2);                     // 武器偏好
            addPreferredWeapon(origin, "bow", 5);                       // 武器偏好
            addPreferredWeapon(origin, "hammer", 1);                    // 武器偏好
            origin.production = new string[] { "bread", "pie", "jam", "sushi", "cider" };                                                // 生产的食物
            origin.roads_forbideen = false;                             // 允许建路
            origin.skin_citizen_female = List.Of("unit_female_1");      // 女性皮肤
            origin.skin_citizen_male = List.Of("unit_male_1");          // 男性皮肤
            origin.skin_civ_default_female = "unit_female_1";           // 无文化时女性皮肤
            origin.skin_civ_default_male = "unit_male_1";               // 无文化时男性皮肤
            origin.skin_warrior = List.Of("unit_warrior_1");            // 士兵皮肤
            origin.stats.culture_spread_speed.value = 8;                // 文化传播速度
            origin.stats.culture_spread_convert_chance.value = 0.3f;    // 文化转化速度
            origin.stats.bonus_max_unit_level.value = 6;                // 人物最大等级
            origin.id = race.id;
            CW_Library_Manager.instance.races.add(race);
        }

        private static void add_kingdoms()
        {
            add_eastern_human_kingdom();
        }

        private static void add_eastern_human_kingdom()
        {
            CW_Asset_Kingdom kingdom = new CW_Asset_Kingdom();
            kingdom.id = "eastern_human";
            kingdom.origin_asset = new KingdomAsset();
            KingdomAsset origin = kingdom.origin_asset;
            origin.id = kingdom.id;
            origin.civ = true;
            origin.addTag("civ");
            origin.addTag("eastern_human");
            origin.addFriendlyTag("eastern_human");
            origin.addFriendlyTag("neutral");
            origin.addFriendlyTag("good");
            origin.addEnemyTag("yao");
            origin.addEnemyTag("bandits");
            CW_Library_Manager.instance.kingdoms.add(kingdom);
            MapBox.instance.kingdoms.CallMethod("newHiddenKingdom", origin);

            kingdom = new CW_Asset_Kingdom();
            kingdom.id = "nomads_eastern_human";
            kingdom.origin_asset = new KingdomAsset();
            origin = kingdom.origin_asset;
            origin.id = kingdom.id;
            origin.nomads = true;
            origin.mobs = true;
            origin.default_kingdom_color = new KingdomColor("#5aa4ae", "#5aa4ae", "#5aa4ae");//0x5aa4ae
            origin.addTag("civ");
            origin.addTag("eastern_human");
            origin.addFriendlyTag("eastern_human");
            origin.addFriendlyTag("neutral");
            origin.addFriendlyTag("good");
            origin.addEnemyTag("yao");
            origin.addEnemyTag("bandits");
            CW_Library_Manager.instance.kingdoms.add(kingdom);
            MapBox.instance.kingdoms.CallMethod("newHiddenKingdom", origin);
        }


        /// <summary>
        /// TODO: 0.14.3 游戏中拷贝
        /// </summary>
        /// <param name="race"></param>
        /// <param name="pString"></param>
        private static void setPreferredStatPool(Race race, string pString)
        {
            pString = pString.Replace(" ", string.Empty);
            string[] array = pString.Split(new char[] { ',' });
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(new char[] { '#' });
                int num = int.Parse(array2[1]);
                string text = array2[0];
                for (int j = 0; j < num; j++)
                {
                    race.preferred_attribute.Add(text);
                }
            }
        }
        /// <summary>
        /// TODO: 0.14.3 游戏中拷贝
        /// </summary>
        /// <param name="race"></param>
        /// <param name="pString"></param>
        private static void setPreferredFoodPool(Race race, string pString)
        {
            pString = pString.Replace(" ", string.Empty);
            string[] array = pString.Split(new char[] { ',' });
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(new char[] { '#' });
                int num = int.Parse(array2[1]);
                string text = array2[0];
                for (int j = 0; j < num; j++)
                {
                    race.preferred_food.Add(text);
                }
            }
        }
        /// <summary>
        /// TODO: 0.14.3 游戏中拷贝
        /// </summary>
        /// <param name="race"></param>
        /// <param name="pID"></param>
        /// <param name="pAmount"></param>
        private static void addPreferredWeapon(Race race, string pID, int pAmount)
        {
            for (int i = 0; i < pAmount; i++)
            {
                race.preferred_weapons.Add(pID);
            }
        }
    }
}
