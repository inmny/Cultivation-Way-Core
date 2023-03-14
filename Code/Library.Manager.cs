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
        public static CW_ActorAssetLibrary actors = new CW_ActorAssetLibrary();
        public static CultisysLibrary cultisys = new CultisysLibrary();
        public static ElementLibrary elements = new ElementLibrary();
        public static Dictionary<string, BaseAssetLibrary> libraries = new Dictionary<string, BaseAssetLibrary>();
        public void init()
        {
            instance = this;
            add(actors, "actors");
            add(cultisys, "cultisys");
            add(elements, "elements");
            CW_BaseStatsLibrary.init();
        }
        public void post_init()
        {
            foreach(BaseAssetLibrary library in libraries.Values)
            {
                library.post_init();
            }
        }
        private void add(BaseAssetLibrary library, string name = "")
        {
            if (string.IsNullOrEmpty(name)) name = library.GetType().Name;
            library.init();
            library.id = name;
            libraries.Add(name, library);
        }
    }
}
