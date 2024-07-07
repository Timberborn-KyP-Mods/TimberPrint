using System;
using System.Collections.Generic;
using Bindito.Core;

namespace TimberApi.UIBuilder.UIBuilderSystem;

internal class DependencyContainer
{
    internal static IContainer Container = null!;

    public DependencyContainer(IContainer container)
    {
        Container = container;
    }

    public static T GetInstance<T>()
    {
        return Container.GetInstance<T>();
    }

    public static object GetInstance(Type type)
    {
        return Container.GetInstance(type);
    }

    public static IEnumerable<object> GetBoundInstances()
    {
        return Container.GetBoundInstances();
    }
}