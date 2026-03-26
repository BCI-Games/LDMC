using System;
using System.Reflection;

public static class ReflectionExtensions
{
    const BindingFlags StaticBindingFlags = BindingFlags.Public | BindingFlags.Static;
    const BindingFlags InstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    public static bool TryGetStaticFieldValue<T>
    (
        this Type caller,
        string name, out T output
    )
    {
        FieldInfo field = caller.GetField(name, StaticBindingFlags);
        return field.TryGetValue(out output);
    }

    public static bool TryGetInstanceFieldValue<TCaller, TValue>
    (
        this TCaller caller,
        string name, out TValue output
    ) where TCaller : class
    {
        FieldInfo field = typeof(TCaller).GetField(name, InstanceBindingFlags);
        return field.TryGetValue(out output, caller);
    }


    public static bool TryGetStaticPropertyValue<T>
    (
        this Type caller,
        string name, out T output
    )
    {
        PropertyInfo property = caller.GetProperty(name, StaticBindingFlags);
        return property.TryGetValue(out output);
    }
    public static bool TryGetInstancePropertyValue<TCaller, TValue>
    (
        this TCaller caller,
        string name, out TValue output
    ) where TCaller : class
    {
        PropertyInfo property = typeof(TCaller).GetProperty(name, InstanceBindingFlags);
        return property.TryGetValue(out output, caller);
    }


    private static bool TryGetValue<T>
    (
        this MemberInfo info, out T output
    )
    => info.TryGetValue<Type, T>(out output);
    private static bool TryGetValue<TCaller, TValue>
    (
        this MemberInfo info,
        out TValue output,
        TCaller instance = null
    ) where TCaller : class
    {
        output = (TValue)info?.GetValue(instance);
        return output != null;
    }
    private static object GetValue<TCaller>
    (
        this MemberInfo info,
        TCaller instance = null
    ) where TCaller : class
    => info switch
    {
        FieldInfo field => field?.GetValue(instance),
        PropertyInfo property => property?.GetValue(instance, null),
        _ => null
    };


    public static bool HasStaticField(this Type caller, string name)
    => caller.HasField(name, StaticBindingFlags);
    public static bool HasInstanceField(this Type caller, string name)
    => caller.HasField(name, InstanceBindingFlags);

    public static bool HasField
    (
        this Type caller, string name,
        BindingFlags bindingFlags
    )
    => caller.GetField(name, bindingFlags) != null;
    public static bool HasField(this Type caller, string name)
    => caller.GetField(name) != null;


    public static bool HasStaticProperty(this Type caller, string name)
    => caller.HasProperty(name, StaticBindingFlags);
    public static bool HasInstanceProperty(this Type caller, string name)
    => caller.HasProperty(name, InstanceBindingFlags);

    public static bool HasProperty
    (
        this Type caller, string name,
        BindingFlags bindingFlags
    )
    => caller.GetProperty(name, bindingFlags) != null;
    public static bool HasProperty(this Type caller, string name)
    => caller.GetProperty(name) != null;
}