namespace Cultivation_Way.Core
{
    public class CW_StatusEffectData
    {
        /// <summary>
        /// id
        /// </summary>
        public string id;
        /// <summary>
        /// 结束标记, 准备回收
        /// </summary>
        public bool finished;
        /// <summary>
        /// 剩余时间
        /// </summary>
        public float left_time;
        /// <summary>
        /// action_on_update计时器
        /// </summary>
        private float _update_action_timer;
        /// <summary>
        /// Asset访问
        /// </summary>
        public Library.CW_StatusEffect status_asset;
        /// <summary>
        /// 属性加成, 修改时注意深拷贝
        /// </summary>
        public BaseStats bonus_stats;
        /// <summary>
        /// 该状态的动画, 可能为null
        /// </summary>
        public Animation.SpriteAnimation anim;
        /// <summary>
        /// 该状态的施加者, 可能为null
        /// </summary>
        public BaseSimObject source;
    }
}
