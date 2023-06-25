using Cultivation_Way.Factory;
using Cultivation_Way.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    internal class SpellArg
    {
        public CW_SpellAsset spell;
        public BaseSimObject user;
        public BaseSimObject target;
        public WorldTile target_tile;
        public float cost;
        public void clear()
        {
            spell = null; user = null; target = null; target_tile = null; cost = 0;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void set(CW_SpellAsset spell, BaseSimObject user, BaseSimObject target, WorldTile target_tile, float cost)
        {
            this.spell = spell;
            this.user = user;
            this.target = target;
            this.target_tile = target_tile;
            this.cost = cost;
        }
    }
    internal class SpellManager
    {
        private readonly Queue<SpellArg> args_to_deal = new();
        private readonly Stack<SpellArg> empty_args = new();
        /// <summary>
        /// 添加新的法术到队列中
        /// </summary>
        public void enqueue_spell(CW_SpellAsset spell, BaseSimObject user, BaseSimObject target, WorldTile target_tile, float cost)
        {
            SpellArg arg_to_enqueue = empty_args.Count > 0 ? empty_args.Pop() : new SpellArg();
            arg_to_enqueue.set(spell, user, target, target_tile, cost);
            args_to_deal.Enqueue(arg_to_enqueue);
        }
        /// <summary>
        /// 处理所有队列中的法术
        /// </summary>
        public void deal_all()
        {
            int deal_count = args_to_deal.Count;
            while(deal_count-- > 0)
            {
                SpellArg arg_to_deal = args_to_deal.Dequeue();

                if (arg_to_deal.user != null || arg_to_deal.user.isAlive())
                {
                    arg_to_deal.spell.anim_action?.Invoke(arg_to_deal.spell, arg_to_deal.user, arg_to_deal.target, arg_to_deal.target_tile, arg_to_deal.cost);
                    arg_to_deal.spell.spell_action?.Invoke(arg_to_deal.spell, arg_to_deal.user, arg_to_deal.target, arg_to_deal.target_tile, arg_to_deal.cost);
                }

                arg_to_deal.clear();
                empty_args.Push(arg_to_deal);
            }
        }
        public void clear()
        {
            int deal_count = args_to_deal.Count;
            while (deal_count-- > 0)
            {
                SpellArg arg_to_deal = args_to_deal.Dequeue();
                arg_to_deal.clear();
                empty_args.Push(arg_to_deal);
            }
        }
        /// <summary>
        /// 内存回收
        /// </summary>
        public void update_per_while()
        {
            int target_size = empty_args.Count < 100 ? empty_args.Count : (empty_args.Count * 4 /5);
            while(empty_args.Count > target_size)
            {
                empty_args.Pop();
            }
        }
    }
}
