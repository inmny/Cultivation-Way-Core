using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way.Addon
{
    public class CW_Addon : MonoBehaviour
    {
        private bool loaded = false;
        internal bool initialized = false;
        public ModDeclaration.Info mod_info;
        internal void Awake()
        {
            if(loaded) return;
            loaded = true;
        }
    }
}
