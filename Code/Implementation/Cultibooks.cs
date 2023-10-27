using Cultivation_Way.Constants;
using Cultivation_Way.General.AboutCultibook;
using Cultivation_Way.Library;

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
    }
}