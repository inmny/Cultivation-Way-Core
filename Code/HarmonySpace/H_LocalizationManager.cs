using HarmonyLib;

namespace Cultivation_Way.HarmonySpace;

internal static class H_LocalizationManager
{
    /// <summary>
    ///     语言设置后，重新应用本地化
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(LocalizedTextManager), nameof(LocalizedTextManager.setLanguage))]
    public static void setLanguage_postfix(string pLanguage)
    {
        Localizer.apply_localization(LocalizedTextManager.instance.localizedText, pLanguage);
    }
}