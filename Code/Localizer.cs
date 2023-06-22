using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    internal static class Localizer
    {
        /// <summary>
        /// 存储额外加载的所有语言的key和text
        /// 能写多少，顶多多用几MB的内存，全存了
        /// </summary>
        private readonly static Dictionary<string, Dictionary<string, string>> languages_key_text = new();
        private readonly static HashSet<string> loaded_path = new();

        public static void init()
        {
            load_json(System.IO.Path.Combine(CW_Core.instance.state.mod_info.Path, "GameResources/cw_locales/cz.json"), "cz");
            load_json(System.IO.Path.Combine(CW_Core.instance.state.mod_info.Path, "GameResources/cw_locales/tc.json"), "tc");
            load_json(System.IO.Path.Combine(CW_Core.instance.state.mod_info.Path, "GameResources/cw_locales/en.json"), "en");
        }
        public static void load_json(string path, string language)
        {
            string json = System.IO.File.ReadAllText(path);

            Dictionary<string, string> key_text = fastJSON.JSON.ToObject<Dictionary<string, string>>(json);
            //Dictionary<string, string> key_text = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (!languages_key_text.ContainsKey(language)) languages_key_text[language] = key_text;
            else
            {
                foreach(KeyValuePair<string,string> key_text_pair in key_text)
                {
                    languages_key_text[language][key_text_pair.Key] = key_text_pair.Value;
                }
            }
            loaded_path.Add(path);
        }
        public static void apply_localization(Dictionary<string, string> target_dict, string language)
        {
            if(language!="cz" && language!="tc" && language != "en")
            {
                load_json(System.IO.Path.Combine(CW_Core.instance.state.mod_info.Path, "GameResources/cw_locales/en.json"), language);
            }

            foreach(Addon.CW_Addon addon in CW_Core.instance.state.addons)
            {
                string locale_dir = System.IO.Path.Combine(addon.mod_info.Path, "Locales");

                DirectoryInfo dir = new(locale_dir);
                // 检查本地化文件夹是否存在
                if (!dir.Exists) continue;

                FileInfo[] files = new DirectoryInfo(locale_dir).GetFiles("*.json");

                bool language_locale_file_found = false;
                // 正常加载对应语言的本地化文件
                foreach(FileInfo locale_file in files)
                {
                    if(locale_file.Name == language + ".json")
                    {
                        load_json(locale_file.FullName, language);
                        language_locale_file_found = true;
                        break;
                    }
                }
                // 如果对应语言的本地化文件不存在，则尝试加载英文的本地化文件
                if(!language_locale_file_found)
                {
                    foreach (FileInfo locale_file in files)
                    {
                        if (locale_file.Name == "en.json")
                        {
                            load_json(locale_file.FullName, language);
                            language_locale_file_found = true;
                            break;
                        }
                    }
                }
            }

            Dictionary<string, string> key_text = languages_key_text[language];
            foreach(KeyValuePair<string, string> key_text_pair in key_text)
            {
                target_dict[key_text_pair.Key] = key_text_pair.Value;
            }
        }
    }
}
