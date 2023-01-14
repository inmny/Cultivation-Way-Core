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
namespace Cultivation_Way
{
    public class __Mod
    {
        public ModDeclaration.Info Info;
        public GameObject GameObject;
        internal void awake()
        {
            throw new NotImplementedException();
        }
        internal void finish_init()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class CW_Addon : MonoBehaviour
    {
        public bool initialized = false;
        public __Mod this_mod;
    }
}
