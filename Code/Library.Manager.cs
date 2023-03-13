using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class Manager
    {
        public static Manager instance;
        public void init()
        {
            instance = this;
            CW_BaseStatsLibrary.init();
        }
    }
}
