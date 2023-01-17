using Cultivation_Way.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public enum CW_Event_Trigger_Type
    {
        PER_MONTH,
        EXACTLY_TIME,
        CUSTOM,
        HAND
    }
    public class CW_Asset_WorldEvent : Asset
    {
        public CW_Event_Trigger_Type trigger_type;
        
        public float trigger_val;
        public float action_val;
        
        public Others.CW_Delegates.CW_WorldEvent_Action action;

        public CW_Asset_WorldEvent(
            string id, 
            CW_Event_Trigger_Type trigger_type,
            float trigger_val,
            CW_Delegates.CW_WorldEvent_Action action,
            float action_val)
        {
            this.id = id;
            this.trigger_type = trigger_type;
            this.trigger_val = trigger_val;
            this.action = action;
            this.action_val = action_val;
        }
    }
    public class CW_Library_WorldEvent : CW_Asset_Library<CW_Asset_WorldEvent>
    {
        internal override void register()
        {
            throw new NotImplementedException();
        }
    }
}
