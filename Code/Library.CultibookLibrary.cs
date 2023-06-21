using System;
namespace Cultivation_Way.Library
{
    public class Cultibook : Asset
    {
        /// <summary>
        /// 功法名
        /// </summary>
        public string name = "";
        /// <summary>
        /// 功法描述
        /// </summary>
        public string description = "";
        /// <summary>
        /// 作者名
        /// </summary>
        public string author_name = "";
        /// <summary>
        /// 最后编者名
        /// </summary>
        public string editor_name = "";
        /// <summary>
        /// 属性加成
        /// </summary>
        public BaseStats bonus_stats = new();
        /// <summary>
        /// 自带法术
        /// </summary>
        public string[] spells;
        /// <summary>
        /// 当前使用人数
        /// </summary>
        public int cur_users { get; internal set; }
        /// <summary>
        /// 历史最大使用人数
        /// </summary>
        public int max_users { get; internal set; }
        /// <summary>
        /// 减少使用人数
        /// </summary>
        /// <exception cref="Exception">使用人数为负数</exception>
        public void decrease()
        {
            cur_users--;
            if (cur_users < 0 && Constants.Others.strict_mode)
            {
                max_users = Constants.Others.cultibook_lock_line;
                throw new Exception($"Error current users {cur_users} for Cultibook {id}. Set its max_users up to remove line");
            }
        }
        /// <summary>
        /// 增加使用人数
        /// </summary>
        public void increase()
        {
            cur_users++;
            if (cur_users > max_users)
            {
                max_users = cur_users;
            }
        }
    }
    public class CultibookLibrary : CW_DynamicLibrary<Cultibook>
    {
        /// <summary>
        /// 删除使用人数为0的功法
        /// </summary>
        public override void update()
        {
            base.update();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].cur_users <= 0 && list[i].max_users < Constants.Others.cultibook_lock_line)
                {
                    dict.Remove(list[i].id);
                    list.RemoveAt(i);
                    i--;
                }
            }
        }
        public override void post_init()
        {
            base.post_init();
        }
    }
}
