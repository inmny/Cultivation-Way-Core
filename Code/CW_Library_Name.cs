using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Name_Template_Elm
    {

    }
    public class CW_Name_Template
    {

    }
    public class CW_Asset_Name : Asset
    {
        // public string id; 模板的id，用于匹配
        public List<CW_Name_Template> name_templates;
    }
}
