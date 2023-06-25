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
        internal float _update_action_timer;
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
        /// <summary>
        /// 创建状态数据, 注意动画需要自行创建
        /// </summary>
        /// <param name="status_asset"></param>
        /// <param name="source"></param>
        public CW_StatusEffectData(Library.CW_StatusEffect status_asset, BaseSimObject source)
        {
            this.status_asset = status_asset;
            this.source = source;
            this.id = status_asset.id;
            this.left_time = status_asset.duration;
            this.bonus_stats = status_asset.bonus_stats;
            this.anim = null;
            this.finished = false;
            this._update_action_timer = 0;
        }
        internal void update_timer(float delta_time)
        {
            if (this.finished)
                return;
            if(_update_action_timer > 0)
            {
                _update_action_timer -= delta_time;
            }
            this.left_time -= delta_time;
            if (this.left_time <= 0)
            {
                this.left_time = 0;
                this.finished = true;
            }
            if(finished && anim != null)
            {
                anim.force_stop(true);
            }
        }
    }
}
