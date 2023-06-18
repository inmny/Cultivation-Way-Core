using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Constants;
namespace Cultivation_Way.Library
{
    public class CW_Library<T> : AssetLibrary<T> where T : Asset
    {
        public int size => list.Count;
        public override T get(string pID)
        {
            T ret = base.get(pID);

            if (ret == null && Constants.Others.strict_mode) 
                throw new KeyNotFoundException($"Not found {pID} in {this.id}");

            return ret;
        }
    }
    public class CW_DynamicLibrary<T> : CW_Library<T> where T : Asset
    {
        public List<T> static_list = new();
        public virtual void reset()
        {
            this.list.Clear();
            this.dict.Clear();
            foreach(T asset in static_list)
            {
                this.add(asset);
            }
        }
        public virtual void add_to_static_list(T static_asset)
        {
            static_list.Add(static_asset);
        }
    }
}
