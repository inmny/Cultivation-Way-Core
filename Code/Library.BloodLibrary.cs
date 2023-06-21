using System;

namespace Cultivation_Way.Library
{
    /// <summary>
    /// 血脉, id为先祖id
    /// </summary>
    public class BloodNodeAsset : Asset
    {
        /// <summary>
        /// 先祖数据
        /// <para>在先祖存活时与其同步, 在先祖死亡时通过patch进行拷贝保存</para>
        /// </summary>
        public ActorData ancestor_data;
        /// <summary>
        /// 祖先自身属性, 包括自身,心情,功法,修炼体系,以及血脉的影响下的属性, 应当是一个拷贝.
        /// <para>在先祖存活时与其同步, 在先祖死亡后不做修改</para>
        /// </summary>
        public readonly BaseStats ancestor_stats = new();
        /// <summary>
        /// 存活后代数量
        /// </summary>
        public int alive_descendants_count { get; internal set; }
        /// <summary>
        /// 历史最大后代数量
        /// </summary>
        public int max_descendants_count { get; internal set; }

        /// <summary>
        /// 减少存活后代计数
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void decrease()
        {
            alive_descendants_count--;
            if (alive_descendants_count < 0 && Constants.Others.strict_mode)
            {
                max_descendants_count = Constants.Others.blood_node_lock_line;
                throw new Exception($"Error current users {alive_descendants_count} for BloodNode {id}. Set its max_users up to remove line");
            }
        }
        /// <summary>
        /// 增加存活后代计数
        /// </summary>
        public void increase()
        {
            alive_descendants_count++;
            if (alive_descendants_count > max_descendants_count)
            {
                max_descendants_count = alive_descendants_count;
            }
        }
    }
    public class BloodNodeLibrary : CW_DynamicLibrary<BloodNodeAsset>
    {
        /// <summary>
        /// 删除无传承血脉
        /// </summary>
        public override void update()
        {
            base.update();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].alive_descendants_count <= 0 && list[i].max_descendants_count < Constants.Others.blood_node_lock_line)
                {
                    dict.Remove(list[i].id);
                    list.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
