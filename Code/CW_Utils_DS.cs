using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Utils
{
    
    internal class CW_ForwardLinkedList<T>
    {
        public class Node
        {
            public T Value { get; set; }
            public Node Previous { get; internal set; }
            public Node Next { get; internal set; }
            public CW_ForwardLinkedList<T> List { get; internal set; }
            internal Node(T value, Node previous, Node next, CW_ForwardLinkedList<T> list)
            {
                Value = value;
                Previous = previous;
                Next = next;
                List = list;
            }
        }

        public Node First { get; private set; }
        public Node Last { get; private set; }
        public Node Current { get; private set; }
        public int Count { get; private set; }
        /// <summary>
        /// 添加至最后
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (this.First == null)
            {
                this.First = new Node(item, null, null, this);
                this.Last = this.First;
            }
            else
            {
                this.Last.Next = new Node(item, this.Last, null, this);
                this.Last = this.Last.Next;
            }
            Count++;
        }
        /// <summary>
        /// 将当前节点移除，但仍指向移除后的节点，访问下一个仍需要MoveNext
        /// </summary>
        /// <returns></returns>
        public T RemoveCurrent()
        {
            if (this.Current == null) return default(T);

            if (this.Current.Next == null && this.Current.Previous != null) { this.Current.Previous.Next = null; this.Last = this.Current.Previous; }
            else if (this.Current.Next != null && this.Current.Previous == null) { this.Current.Next.Previous = null; this.First = this.Current.Next; }
            else if(this.Current.Next==null && this.Current.Previous == null) { this.First = null; this.Last = null; }
            else
            {
                this.Current.Next.Previous = this.Current.Previous;
                this.Current.Previous.Next = this.Current.Next;
            }
            Count--;
            return this.Current.Value;
        }
        /// <summary>
        /// 当前节点移动至首位
        /// </summary>
        public void SetToFirst()
        {
            this.Current = First;
        }
        /// <summary>
        /// 获取当前节点的值
        /// </summary>
        /// <returns></returns>
        public T GetCurrent()
        {
            return Current == null ? default(T) : Current.Value;
        }
        /// <summary>
        /// 移动至下一个
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (Current == null) return false;
            Current = Current.Next;
            return Current!=null;
        }
    }
}
