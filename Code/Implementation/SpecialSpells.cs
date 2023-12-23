using Cultivation_Way.Core;

namespace Cultivation_Way.Implementation;

internal enum DoomType
{
    Wind,
    Lightning,
    Fire,
    Water
}

internal static class SpecialSpells
{
    public static void init()
    {
        add_wind_doom();
        add_lightning_doom();
        add_fire_doom();
        add_water_doom();
    }

    private static void add_water_doom()
    {
    }

    private static void add_fire_doom()
    {
    }

    private static void add_lightning_doom()
    {
    }

    private static void add_wind_doom()
    {
    }

    public static void CastDoom(CW_Actor pActor, DoomType pDoomType)
    {
        switch (pDoomType)
        {
            case DoomType.Wind:
                break;
            case DoomType.Lightning:
                break;
            case DoomType.Fire:
                break;
            case DoomType.Water:
                break;
        }
    }
}