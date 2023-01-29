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
        
        public __Mod this_mod { get; private set; }
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
            if (!initialized)
            {
                initialize();
                __finish_init();
            }

        }
        protected void load_mod_info(Type this_mod_type)
        {
            if (this_mod_type == null) throw new Exception("DO NOT CHANGE THE FIRST LINE IN AWAKE");
            if (this_mod_type.Name != "Mod") throw new Exception("DO NOT CHANGE THE FIRST LINE IN AWAKE");
            if (this_mod != null) throw new Exception("DO NOT LOAD REPEATEDLY");

            this_mod = new __Mod();
            this_mod.Info = Reflection.GetField((Type)this_mod_type, null, "Info") as ModDeclaration.Info;
            this_mod.gameObject = this.gameObject;
            ModState.instance.addons.Add(this);

            print(string.Format("[CW Addon]:'{0}' Awake", this_mod.Info.Name));
        }
        public abstract void awake();
        public abstract void initialize();
    }
}
