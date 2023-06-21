using System.Collections.Generic;

namespace Cultivation_Way.Library
{
    public class Manager
    {
        public static Manager instance;
        public readonly static CW_ActorAssetLibrary actors = new();
        public readonly static BloodNodeLibrary bloods = new();
        public readonly static CultibookLibrary cultibooks = new();
        public readonly static CultisysLibrary cultisys = new();
        public readonly static ElementLibrary elements = new();
        public readonly static Dictionary<string, BaseAssetLibrary> libraries = new();
        public void init()
        {
            instance = this;
            add(actors, "actors");
            add(bloods, "bloods");
            add(cultibooks, "cultibooks");
            add(cultisys, "cultisys");
            add(elements, "elements");
            CW_BaseStatsLibrary.init();
        }
        public void update_per_while()
        {
            bloods.update();
            cultibooks.update();
        }
        public void post_init()
        {
            foreach (BaseAssetLibrary library in libraries.Values)
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
