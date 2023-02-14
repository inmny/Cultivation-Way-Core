using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    internal class Spell_Arg
    {
        public Library.CW_Asset_Spell asset;
        public BaseSimObject pUser;
        public BaseSimObject pTarget;
        public WorldTile pTargetTile;
        public float cost;
        public void set(Library.CW_Asset_Spell asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            this.asset = asset;
            this.pUser = pUser;
            this.pTarget = pTarget;
            this.cost = cost;
            this.pTargetTile = pTargetTile;
        }
        public void clear()
        {
            set(null, null, null, null, -1);
        }
    }
    internal class CW_Spell_Manager
    {
        public static CW_Spell_Manager instance;
        private Queue<Spell_Arg> args_to_deal;
        private Stack<Spell_Arg> empty_args;
        public CW_Spell_Manager()
        {
            args_to_deal = new Queue<Spell_Arg>(Others.CW_Constants.default_spell_container_size);
            empty_args = new Stack<Spell_Arg>(Others.CW_Constants.default_spell_container_size);
        }
        public bool enqueue_spell(Library.CW_Asset_Spell asset, BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile, float cost)
        {
            Spell_Arg arg_to_enqueue = empty_args.Count == 0 ? new Spell_Arg() : empty_args.Pop();
            arg_to_enqueue.set(asset, pUser, pTarget, pTargetTile, cost);
            args_to_deal.Enqueue(arg_to_enqueue);
            return true;
        }
        public void deal_all()
        {
            int deal_count = args_to_deal.Count;
            while (deal_count > 0)
            {
                deal_count--;
                Spell_Arg arg_to_deal = args_to_deal.Dequeue();
                CW_Spell.__cast(arg_to_deal.asset, arg_to_deal.pUser, arg_to_deal.pTarget, arg_to_deal.pTargetTile, arg_to_deal.cost);
                arg_to_deal.clear();
                empty_args.Push(arg_to_deal);
            }
        }
        public void clear()
        {
            int deal_count = args_to_deal.Count;
            while (deal_count > 0)
            {
                deal_count--;
                Spell_Arg arg_to_deal = args_to_deal.Dequeue();
                arg_to_deal.clear();
                empty_args.Push(arg_to_deal);
            }
        }
    }
}
