using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    [Serializable]
    public class CW_Family_Member_Info
    {
        public string actor_id;
        public string name;
        public ActorGender gender;
        public CW_Family_Member_Info()
        {
            this.actor_id = "NULL";
            this.name = "NO FOUND";
            this.gender = ActorGender.Unknown;
        }
        public CW_Family_Member_Info(ActorStatus actor_status)
        {
            this.actor_id = actor_status.actorID;
            this.name = String.IsNullOrEmpty(actor_status.firstName) ? actor_status.firstName : null;
            this.gender = actor_status.gender;
        }
    }
    // 暂不使用
    internal class CW_Asset_Family : Asset
    {

    }
    internal class CW_Library_Family : CW_Dynamic_Library<CW_Asset_Family>
    {
        public override CW_Asset_Family add_to_static(CW_Asset_Family asset)
        {
            throw new NotImplementedException();
        }

        internal override void load_as(List<CW_Asset_Family> list)
        {
            throw new NotImplementedException();
        }

        internal override void register()
        {
            throw new NotImplementedException();
        }
        internal override void reset()
        {
            throw new NotImplementedException();
        }
    }
}
