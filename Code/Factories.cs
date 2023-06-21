using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public static class Factories
    {
        public static Factory.Factory<Core.CW_Element> element_factory;
        public static Factory.NoClearFactory<BinaryFormatter> formatter_factory;
        private static readonly List<Factory.BaseFactory> factories = new();
        internal static void init()
        {
            add_factory(element_factory = new Factory.Factory<Core.CW_Element>());
            add_factory(formatter_factory = new Factory.NoClearFactory<BinaryFormatter>());
        }
        public static void add_factory(Factory.BaseFactory factory)
        {
            factories.Add(factory);
        }
        internal static void recycle_items()
        {
            foreach(Factory.BaseFactory fact in factories)
            {
                fact.recycle_items();
            }
        }
        internal static void recycle_memory()
        {
            foreach(Factory.BaseFactory fact in factories)
            {
                fact.recycle_memory(12 * fact.size() / 16);
            }
        }
    }
}
