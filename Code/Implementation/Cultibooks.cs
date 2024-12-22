using Cultivation_Way.Constants;
using Cultivation_Way.Library;
using Cultivation_Way.Utils.General.AboutCultibook;

namespace Cultivation_Way.Implementation;

internal static class Cultibooks
{
    public static void init()
    {
        Cultibook cultibook;
        cultibook = StaticCultibook.create_and_add_cultibook(
            "墟间诀", "跌身入墟间，万里避锋芒。",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            8 // 没啥用的等级, 就可能在天榜里会用到
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.5f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "临阳功", "灵灵真火，浩浩天阳！",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            6 // 没啥用的等级, 就可能在天榜里会用到
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.6f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "百花风云录", "赏尽天下残百花，具是人间千秋压。\n若无风云卷兜鍪，百花自有百刀杀。",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            6 // 没啥用的等级, 就可能在天榜里会用到
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.2f;
        cultibook.bonus_stats[S.damage] = 500;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "青元剑诀（残篇）", "在修仙界平平无奇的一部功法\n由于修练到高层会有真元流失的副作用\n难有人主修此功法 ",
            "青元子", "青元子",
            5 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.15f;
        cultibook.bonus_stats[S.mod_damage] = 1.25f;
        cultibook.bonus_stats[S.damage] = 120f;
        cultibook.bonus_stats[S.mod_health] = 0.4f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.4f;
        cultibook.bonus_stats[CW_S.health_regen] = 12f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.4f;
        cultibook.bonus_stats[S.mod_armor] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.1f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "合欢术", "合欢宗主修的功法\n若不加节制会导致寿元流失",
            "合欢老魔", "合欢老魔",
            6 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 1f;
        cultibook.bonus_stats[S.mod_damage] = 0.8f;
        cultibook.bonus_stats[S.mod_health] = 0.6f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.6f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.8f;
        cultibook.bonus_stats[S.mod_armor] = 1.2f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 1.2f;
        cultibook.bonus_stats[S.max_age] = -20f;
        cultibook.bonus_stats[S.max_children] = 3000f;
        cultibook.bonus_stats[S.fertility] = 12f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "厚土诀", " ",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            3 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.2f;
        cultibook.bonus_stats[S.mod_damage] = 0.1f;
        cultibook.bonus_stats[S.mod_health] = 0.25f;
        cultibook.bonus_stats[S.health] = 20f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.1f;
        cultibook.bonus_stats[CW_S.health_regen] = 2;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.1f;
        cultibook.bonus_stats[S.mod_armor] = 0.6f;
        cultibook.bonus_stats[S.armor] = 20f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.6f;
        cultibook.bonus_stats[CW_S.spell_armor] = 40f;
        cultibook.bonus_stats[S.knockback_reduction] = 0.4f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "小水元功", " ",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            3 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.5f;
        cultibook.bonus_stats[S.mod_damage] = 0.1f;
        cultibook.bonus_stats[S.mod_health] = 0.35f;
        cultibook.bonus_stats[S.health] = 40f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.6f;
        cultibook.bonus_stats[CW_S.health_regen] = 8;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.6f;
        cultibook.bonus_stats[CW_S.wakan_regen] = 16f;
        cultibook.bonus_stats[S.mod_armor] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.1f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "离火诀", " ",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            3 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.25f;
        cultibook.bonus_stats[S.mod_damage] = 0.25f;
        cultibook.bonus_stats[S.damage] = 20f;
        cultibook.bonus_stats[S.mod_health] = 0.2f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.35f;
        cultibook.bonus_stats[CW_S.health_regen] = 3;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.2f;
        cultibook.bonus_stats[S.mod_armor] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.1f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "长春功", " ",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            3 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.35f;
        cultibook.bonus_stats[S.mod_damage] = 0.1f;
        cultibook.bonus_stats[S.mod_health] = 0.5f;
        cultibook.bonus_stats[S.health] = 120f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.5f;
        cultibook.bonus_stats[CW_S.health_regen] = 8;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.4f;
        cultibook.bonus_stats[S.mod_armor] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.25f;
        cultibook.bonus_stats[S.max_age] = 10;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "金光咒", " ",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            3 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.25f;
        cultibook.bonus_stats[S.mod_damage] = 0.35f;
        cultibook.bonus_stats[S.damage] = 30f;
        cultibook.bonus_stats[S.mod_health] = 0.2f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 0.8f;
        cultibook.bonus_stats[S.mod_armor] = 0.1f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.1f;


        cultibook = StaticCultibook.create_and_add_cultibook(
            "临阳功", "灵灵真火，浩浩天阳！",
            "某位没透露身份的大佬", "另一位位没透露身份的大佬",
            1 // 没啥用的等级, 就可能在天榜里会用到
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.6f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "太乙引气诀", "最强练气功法，练了练气期无敌",
            "白 * 然", "白 * 然",
            35 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 1f;
        cultibook.bonus_stats[S.mod_damage] = 1f;
        cultibook.bonus_stats[S.damage] = 7200f;
        cultibook.bonus_stats[S.health] = 12000000f;
        cultibook.bonus_stats[CW_S.health_regen] = 7200f;
        cultibook.bonus_stats[CW_S.wakan] = 12960000000000000f;
        cultibook.bonus_stats[S.armor] = 1200f;
        cultibook.bonus_stats[CW_S.spell_armor] = 6000f;
        cultibook.bonus_stats[S.max_age] = 3000;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "道德经",
            "有物混成先天地生。\n寂兮寥兮独立不改，周行而不殆，\n可以为天下母。\n吾不知其名，强字之曰道。\n强为之名曰大。\n大曰逝，逝曰远，远曰反。\n故道大、天大、地大、人亦大。\n域中有大，而人居其一焉。\n人法地，地法天，天法道，道法自然。 ",
            "道德天尊", "道德天尊",
            35 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 10800f;
        cultibook.bonus_stats[S.mod_damage] = 3600f;
        cultibook.bonus_stats[S.mod_health] = 3600f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 3600f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 3600f;
        cultibook.bonus_stats[S.mod_armor] = 3600f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 3600f;
        cultibook.bonus_stats[S.max_age] = 30000;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "御火真诀", " ",
            "天火真人", "天火真人",
            14 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 3.5f;
        cultibook.bonus_stats[S.mod_damage] = 1.85f;
        cultibook.bonus_stats[S.damage] = 1850f;
        cultibook.bonus_stats[S.mod_health] = 1.2f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 1.2f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 1.2f;
        cultibook.bonus_stats[S.mod_armor] = 0.8f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 1f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "三转重元功", "具有凝练真元辅助突破结丹的奇效\n由于极难修炼，难有人真正修炼其到结丹期 ",
            "青元子", "青元子",
            5 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 0.8f;
        cultibook.bonus_stats[S.mod_damage] = 0.6f;
        cultibook.bonus_stats[S.mod_health] = 0.4f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 0.4f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 12f;
        cultibook.bonus_stats[CW_S.mod_wakan] = -0.8f;
        cultibook.bonus_stats[CW_S.wakan] = 120f;
        cultibook.bonus_stats[S.mod_armor] = 0.4f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 0.4f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "青元剑诀", "完整的青元剑诀是人界顶级的剑道功法\n其修炼者具有独步天下的绝顶战力 ",
            "青元子", "青元子",
            22 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 14f;
        cultibook.bonus_stats[S.mod_damage] = 108f;
        cultibook.bonus_stats[S.damage] = 7200f;
        cultibook.bonus_stats[S.mod_health] = 8f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 6f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 16f;
        cultibook.bonus_stats[S.mod_armor] = 4f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 4f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "房中术", "人多力量大！",
            "合欢老魔", "合欢老魔",
            23 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 18f;
        cultibook.bonus_stats[S.mod_damage] = 5f;
        cultibook.bonus_stats[S.mod_health] = 12f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 8f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 8f;
        cultibook.bonus_stats[S.mod_armor] = 3f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 3f;
        cultibook.bonus_stats[S.max_age] = -80f;
        cultibook.bonus_stats[S.max_children] = 3000000f;
        cultibook.bonus_stats[S.fertility] = 30000f;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "羽化仙经", "",
            "羽升子", "羽升子",
            25 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 20f;
        cultibook.bonus_stats[S.mod_damage] = 3.5f;
        cultibook.bonus_stats[S.mod_health] = 15f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 6.5f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 6f;
        cultibook.bonus_stats[CW_S.mod_wakan] = 1f;
        cultibook.bonus_stats[S.mod_armor] = 3.5f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 3.5f;
        cultibook.bonus_stats[S.max_age] = 10;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "灵宝经", "玉晨之精气，九庆之紫烟\n玉辉焕耀，金映流真\n结化含秀，苞凝元神\n寄胎母氏，育形为人 ",
            "灵宝天尊", "灵宝天尊",
            35 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 7200f;
        cultibook.bonus_stats[S.mod_damage] = 10800f;
        cultibook.bonus_stats[S.mod_health] = 1800f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 1800f;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 10800f;
        cultibook.bonus_stats[CW_S.wakan_regen] = 64000000;
        cultibook.bonus_stats[S.mod_armor] = 7200f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 7200f;
        cultibook.bonus_stats[S.max_age] = 10000;

        cultibook = StaticCultibook.create_and_add_cultibook(
            "太古经", "太古者\n无名无象，不变不迁\n虚空同体，历劫长存\n先天地而不为老，后六极而不为下\n寂兮寥兮\n独立而不攺\n经阴阳而不殆\n不生不死，无往无来\n卓然安静矣 ",
            "原始天尊", "原始天尊",
            35 // 没啥用的等级, 就可能在天榜里会用到(1-9黄 10-18玄 19-27地 >28天 )
        );
        cultibook.bonus_stats[CW_S.mod_cultivelo] = 3600f;
        cultibook.bonus_stats[S.mod_damage] = 3600f;
        cultibook.bonus_stats[S.mod_health] = 10800f;
        cultibook.bonus_stats[CW_S.mod_health_regen] = 10800f;
        cultibook.bonus_stats[CW_S.health_regen] = 72000000;
        cultibook.bonus_stats[CW_S.mod_wakan_regen] = 3600f;
        cultibook.bonus_stats[S.mod_armor] = 360f;
        cultibook.bonus_stats[CW_S.mod_spell_armor] = 3600f;
        cultibook.bonus_stats[S.max_age] = 100000;
    }
}