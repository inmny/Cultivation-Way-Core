using System.Collections.Generic;

namespace Cultivation_Way.Factory;

public interface FactoryItem<T>
{
    public abstract void Set(T item);
    public abstract void Clear();
}

public abstract class BaseFactory
{
    public abstract int size();
    internal abstract void recycle_items();
    internal abstract void recycle_memory(int target_count);
}

public class NoClearFactory<T> : BaseFactory where T : new()
{
    private readonly Stack<T> empty_items = new(4);
    private readonly Stack<T> items = new(4);

    public override int size()
    {
        return items.Count + empty_items.Count;
    }

    public T get_next()
    {
        if (empty_items.Count > 0)
        {
            return empty_items.Pop();
        }

        T ret = new();

        items.Push(ret);

        return ret;
    }

    internal override void recycle_items()
    {
        while (items.Count > 0)
        {
            empty_items.Push(items.Pop());
        }
    }

    internal override void recycle_memory(int target_count = 4)
    {
        while (empty_items.Count > target_count)
        {
            empty_items.Pop();
        }
    }
}

public class Factory<T> : BaseFactory where T : FactoryItem<T>, new()
{
    private readonly Stack<T> empty_items = new(4);
    private readonly Stack<T> items = new(4);

    private readonly T tmp_item_to_fill = new();

    public override int size()
    {
        return items.Count + empty_items.Count;
    }

    public T get_item_to_fill()
    {
        tmp_item_to_fill.Clear();
        return tmp_item_to_fill;
    }

    public T get_next(T filled_item)
    {
        T item_created;
        if (empty_items.Count > 0)
        {
            item_created = empty_items.Pop();
        }
        else
        {
            item_created = new T();
            item_created.Clear();
        }

        item_created.Set(filled_item);

        items.Push(item_created);

        return item_created;
    }

    public T get_next()
    {
        T item_created;
        if (empty_items.Count > 0)
        {
            item_created = empty_items.Pop();
        }
        else
        {
            item_created = new T();
            item_created.Clear();
        }

        items.Push(item_created);

        return item_created;
    }

    internal override void recycle_items()
    {
        while (items.Count > 0)
        {
            empty_items.Push(items.Pop());
            empty_items.Peek().Clear();
        }
    }

    internal override void recycle_memory(int target_count = 4)
    {
        while (empty_items.Count > target_count)
        {
            empty_items.Pop();
        }
    }
}