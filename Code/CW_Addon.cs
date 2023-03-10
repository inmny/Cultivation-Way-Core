using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NCMS.Extensions;
using ReflectionUtility;
using UnityEngine.Purchasing.MiniJSON;

namespace Cultivation_Way
{
    public class __Mod
    {
        public ModDeclaration.Info Info;
        public GameObject gameObject;
        
    }
    public abstract class CW_Addon : MonoBehaviour
    {
        internal bool initialized = false;
        public string name { get; private set; }
        public __Mod this_mod { get; private set; }
        internal string core_version = "";
        internal bool adapt_to_core_version = false;
        private void __finish_init()
        {
            this.initialized = true;
        }
        internal void Awake()
        {
            awake();
        }
        internal void Update()
        {
            if (!initialized && Main.instance.initialized)
            {
                try
                {
                    adapt_to_core_version = core_version == ModState.instance.mod_info.version;
                    if (!adapt_to_core_version)
                    {
                        Warn("适配核心版本为{0}，当前核心版本为{1}", core_version, ModState.instance.mod_info.version);
                    }
                    load_localized_text(ModState.instance.cur_language);
                    initialize();
                    __finish_init();
                }
                catch(Exception e)
                {
                    if(!adapt_to_core_version) Error("核心版本不匹配");
                    Error(e.Message);
                    Error(e.StackTrace);
                }
            }
            if (initialized)
            {
                update(Time.deltaTime);
            }
        }
        internal void load_localized_text(string language)
        {
            if (language != "cz") language = "en";

            string path = string.Format("{0}/Locales/{1}{2}", this_mod.Info.Path, language, ".json");

            if (!File.Exists(path)) return;

            string file_str = File.ReadAllText(path);
            
            Dictionary<string, object> textDic = Json.Deserialize(file_str) as Dictionary<string, object>;

            if (textDic == null) return;

            Dictionary<string, string> localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            if (localizedText == null) return;
            foreach (string key in textDic.Keys)
            {
                localizedText[key] = textDic[key] as string;
            }

            if (language == "en")
            {
                path = string.Format("{0}/Locales/{1}{2}", this_mod.Info.Path, "cz", ".json");

                if (!File.Exists(path)) return;

                file_str = File.ReadAllText(path);

                textDic = Json.Deserialize(file_str) as Dictionary<string, object>;

                if (textDic == null) return;

                localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;

                if (localizedText == null) return;

                foreach (string key in textDic.Keys)
                {
                    if (!localizedText.ContainsKey(key)) localizedText[key] = key.Replace("_", " ").Replace(" title","").Replace(" description","");
                }
            }
        }
        protected void load_mod_info(Type this_mod_type, string core_version = "1.0.4")
        {
            if (this_mod_type == null) throw new Exception("DO NOT CHANGE THE FIRST LINE IN AWAKE");
            if (this_mod_type.Name != "Mod") throw new Exception("DO NOT CHANGE THE FIRST LINE IN AWAKE");
            if (this_mod != null) throw new Exception("DO NOT LOAD REPEATEDLY");
            this_mod = new __Mod();
            this_mod.Info = Reflection.GetField((Type)this_mod_type, null, "Info") as ModDeclaration.Info;
            this_mod.gameObject = this.gameObject;
            ModState.instance.addons.Add(this);
            int name_begin_idx = this_mod.Info.Name.LastIndexOf('.');
            name = this_mod.Info.Name.Substring(name_begin_idx + 1);
            print(string.Format("[CW Addon]:'{0}' Awake", name));
            this.core_version = core_version;
            
        }
        public abstract void awake();
        public abstract void initialize();
        public virtual void update(float elapsed) { return; }
        public void Log(string format, params object[] _objects)
        {
            Debug.LogFormat("[{0}]:{1}", name, string.Format(format, _objects));
        }
        public void Warn(string format, params object[] _objects)
        {
            Debug.LogWarningFormat("[{0}]:{1}", name, string.Format(format, _objects));
        }
        public void Error(string format, params object[] _objects)
        {
            Debug.LogErrorFormat("[{0}]:{1}", name, string.Format(format, _objects));
        }
    }
}
