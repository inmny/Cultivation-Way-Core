using System.Collections.Generic;
using System.IO;
using Cultivation_Way.Addon;
using Newtonsoft.Json;

namespace Cultivation_Way;

internal static class Localizer
{
    /// <summary>
    ///     存储额外加载的所有语言的key和text
    ///     能写多少，顶多多用几MB的内存，全存了
    /// </summary>
    private static readonly Dictionary<string, Dictionary<string, string>> languages_key_text = new();

    private static readonly HashSet<string> loaded_path = new();

    public static void init()
    {
        load_json(Path.Combine(CW_Core.mod_state.mod_info.Path, "Locales/cz.json"), "cz");
        load_json(Path.Combine(CW_Core.mod_state.mod_info.Path, "Locales/ch.json"), "ch");
        load_json(Path.Combine(CW_Core.mod_state.mod_info.Path, "Locales/en.json"), "en");
    }

    private static void load_json(string path, string language)
    {
        if (loaded_path.Contains(path)) return;
        string json = File.ReadAllText(path);
        //Logger.Log(json);
        //Dictionary<string, string> key_text = fastJSON.JSON.ToObject<Dictionary<string, string>>(json);
        Dictionary<string, string> key_text = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        if (!languages_key_text.ContainsKey(language)) languages_key_text[language] = key_text;
        else
        {
            foreach (KeyValuePair<string, string> key_text_pair in key_text)
            {
                languages_key_text[language][key_text_pair.Key] = key_text_pair.Value;
            }
        }

        loaded_path.Add(path);
    }

    public static void apply_localization(Dictionary<string, string> target_dict, string language)
    {
        if (!languages_key_text.ContainsKey(language))
        {
            CW_Core.LogInfo($"Language {language} not found, fallback to Chinese.");
            language = "cz";
            //load_json(Path.Combine(CW_Core.mod_state.mod_info.Path, "Locales/cz.json"), language);
        }

        foreach (CW_Addon addon in CW_Core.mod_state.addons)
        {
            string locale_dir = Path.Combine(addon.this_mod.info.Path, "Locales");

            DirectoryInfo dir = new(locale_dir);
            // 检查本地化文件夹是否存在
            if (!dir.Exists) continue;

            FileInfo[] files = new DirectoryInfo(locale_dir).GetFiles("*.json");

            bool language_locale_file_found = false;
            // 正常加载对应语言的本地化文件
            foreach (FileInfo locale_file in files)
            {
                if (locale_file.Name != language + ".json") continue;
                load_json(locale_file.FullName, language);
                language_locale_file_found = true;
                break;
            }

            // 如果对应语言的本地化文件不存在，则尝试加载简中的本地化文件
            if (language_locale_file_found) continue;
            foreach (FileInfo locale_file in files)
            {
                if (locale_file.Name != "cz.json") continue;
                load_json(locale_file.FullName, language);
                break;
            }
        }

        Dictionary<string, string> key_text = languages_key_text[language];
        foreach (KeyValuePair<string, string> key_text_pair in key_text)
        {
            target_dict[key_text_pair.Key] = key_text_pair.Value;
        }
    }
}