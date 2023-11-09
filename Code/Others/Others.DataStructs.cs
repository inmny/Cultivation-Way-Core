namespace Cultivation_Way.Others.DataStructs;

internal class CW_ForwardLinkedList<T>
{
    public Node First { get; private set; }
    public Node Last { get; private set; }
    public Node Current { get; private set; }
    public int Count { get; private set; }

    /// <summary>
    ///     添加至最后
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        if (First == null)
        {
            First = new Node(item, null, null, this);
            Last = First;
        }
        else
        {
            Last.Next = new Node(item, Last, null, this);
            Last = Last.Next;
        }

        Count++;
    }

    /// <summary>
    ///     将当前节点移除，但仍指向移除后的节点，访问下一个仍需要MoveNext
    /// </summary>
    /// <returns></returns>
    public T RemoveCurrent()
    {
        if (Current == null) return default;

        if (Current.Next == null && Current.Previous != null)
        {
            Current.Previous.Next = null;
            Last = Current.Previous;
        }
        else if (Current.Next != null && Current.Previous == null)
        {
            Current.Next.Previous = null;
            First = Current.Next;
        }
        else if (Current.Next == null && Current.Previous == null)
        {
            First = null;
            Last = null;
        }
        else
        {
            Current.Next.Previous = Current.Previous;
            Current.Previous.Next = Current.Next;
        }

        Count--;
        return Current.Value;
    }

    /// <summary>
    ///     当前节点移动至首位
    /// </summary>
    public void SetToFirst()
    {
        Current = First;
    }

    /// <summary>
    ///     获取当前节点的值
    /// </summary>
    /// <returns></returns>
    public T GetCurrent()
    {
        return Current == null ? default : Current.Value;
    }

    /// <summary>
    ///     移动至下一个
    /// </summary>
    /// <returns></returns>
    public bool MoveNext()
    {
        if (Current == null) return false;
        Current = Current.Next;
        return Current != null;
    }

    public class Node
    {
        internal Node(T value, Node previous, Node next, CW_ForwardLinkedList<T> list)
        {
            Value = value;
            Previous = previous;
            Next = next;
            List = list;
        }

        public T Value { get; set; }
        public Node Previous { get; internal set; }
        public Node Next { get; internal set; }
        public CW_ForwardLinkedList<T> List { get; internal set; }
    }
}