using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Cultivation_Way.UI;

internal static class Fonts
{
    public static Font STLiti;
    public static Font STKaiti;

    public static void init()
    {
        STLiti = Font.CreateDynamicFontFromOSFont("STLiti", 18);
        STKaiti = Font.CreateDynamicFontFromOSFont("STKaiti", 18);
    }
}