using System.Collections.Generic;
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
        public virtual bool contains(string pID)
        {
            return dict.ContainsKey(pID);
        }
    }
    /// <summary>
    /// 动态库, 每隔一段时间(不确定长度)调用一次
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CW_DynamicLibrary<T> : CW_Library<T> where T : Asset
    {
        /// <summary>
        /// 静态列表
        /// </summary>
        public List<T> static_list = new();
        /// <summary>
        /// 每隔一段时间(不确定长度)调用一次
        /// </summary>
        public virtual void update()
        {

        }
        public virtual void reset()
        {
            this.list.Clear();
            this.dict.Clear();
            foreach (T asset in static_list)
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
