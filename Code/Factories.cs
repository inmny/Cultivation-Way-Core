using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Cultivation_Way.Core;
using Cultivation_Way.Factory;

namespace Cultivation_Way;

public static class Factories
{
    public static Factory<CW_Element> element_factory;
    public static NoClearFactory<BinaryFormatter> formatter_factory;
    public static NoClearFactory<List<CW_StatusEffectData>> status_list_factory;
    private static readonly List<BaseFactory> factories = new();

    internal static void init()
    {
        add_factory(element_factory = new Factory<CW_Element>());
        add_factory(formatter_factory = new NoClearFactory<BinaryFormatter>());
        add_factory(status_list_factory = new NoClearFactory<List<CW_StatusEffectData>>());
    }

    public static void add_factory(BaseFactory factory)
    {
        factories.Add(factory);
    }

    internal static void recycle_items()
    {
        foreach (BaseFactory fact in factories)
        {
            fact.recycle_items();
        }
    }

    internal static void recycle_memory()
    {
        foreach (BaseFactory fact in factories)
        {
            fact.recycle_memory(12 * fact.size() / 16);
        }
    }
}