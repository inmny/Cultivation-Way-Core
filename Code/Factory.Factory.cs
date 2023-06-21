using System.Collections.Generic;

namespace Cultivation_Way.Factory
{
    public interface FactoryItem<T>
    {
        public abstract void set(T item);
        public abstract void clear();
    }
    public abstract class BaseFactory
    {
        public abstract int size();
        internal abstract void recycle_items();
        internal abstract void recycle_memory(int target_count);
    }
    public class NoClearFactory<T> : BaseFactory
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
            else
            {
                T ret = default;

                items.Push(ret);

                return default;
            }
        }
        internal override void recycle_items()
        {
            while (items.Count > 0)
            {
                empty_items.Push(items.Pop());
                empty_items.Peek();
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
    public class Factory<T> : BaseFactory where T : FactoryItem<T>
    {
        private readonly Stack<T> empty_items = new(4);
        private readonly Stack<T> items = new(4);

        private readonly T tmp_item_to_fill = default;
        public override int size()
        {
            return items.Count + empty_items.Count;
        }
        public T get_item_to_fill()
        {
            tmp_item_to_fill.clear();
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
                item_created = default(T);
                item_created.clear();
            }
            item_created.set(filled_item);

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
                item_created = default(T);
                item_created.clear();
            }

            items.Push(item_created);

            return item_created;
        }
        internal override void recycle_items()
        {
            while (items.Count > 0)
            {
                empty_items.Push(items.Pop());
                empty_items.Peek().clear();
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
}
