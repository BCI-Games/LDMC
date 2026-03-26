using System;
using System.Linq;
using System.Reflection;

public static class ReflectionExtensions
{
    const BindingFlags StaticBindingFlags = BindingFlags.Public | BindingFlags.Static;
    const BindingFlags InstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;

    public static bool GetStaticFieldValue<T>
    (
        this Type caller,
        string name, out T output
    ) where T : class
    {
        FieldInfo[] fields = caller.GetFields(StaticBindingFlags);
        return fields.GetNamedMemberValue(name, out output);
    }

    public static bool GetInstanceFieldValue<TCaller, TValue>
    (
        this TCaller caller,
        string name, out TValue output
    ) where TCaller : class where TValue : class
    {
        FieldInfo[] fields = typeof(TCaller).GetFields(InstanceBindingFlags);
        return fields.GetNamedMemberValue(name, out output, caller);
    }


    public static bool GetStaticPropertyValue<T>
    (
        this Type caller,
        string name, out T output
    ) where T : class
    {
        PropertyInfo[] properties = caller.GetProperties(StaticBindingFlags);
        return properties.GetNamedMemberValue(name, out output);
    }
    public static bool GetInstancePropertyValue<T>
    (
        this Type caller,
        string name, out T output
    ) where T : class
    {
        PropertyInfo[] properties = caller.GetProperties(InstanceBindingFlags);
        return properties.GetNamedMemberValue(name, out output);
    }


    private static bool GetNamedMemberValue<T>
    (
        this MemberInfo[] infos, string name, out T output
    ) where T : class
    => infos.GetNamedMemberValue<Type, T>(name, out output);
    private static bool GetNamedMemberValue<TCaller, TValue>
    (
        this MemberInfo[] infos,
        string name, out TValue output,
        TCaller instance = null
    )
    where TValue : class where TCaller : class
    {
        MemberInfo match = infos.FindNamedMember(name);
        output = match switch
        {
            FieldInfo field => field?.GetValue(instance),
            PropertyInfo property => property?.GetValue(instance),
            _ => null
        } as TValue;
        return output != null;
    }


    public static bool HasStaticField(this Type caller, string name)
    => caller.HasField(name, StaticBindingFlags);
    public static bool HasInstanceField(this Type caller, string name)
    => caller.HasField(name, InstanceBindingFlags);

    public static bool HasField
    (
        this Type caller, string name,
        BindingFlags bindingFlags
    )
    => caller.GetFields(bindingFlags).HasNamedMember(name);
    public static bool HasField(this Type caller, string name)
    => caller.GetFields().HasNamedMember(name);


    public static bool HasStaticProperty(this Type caller, string name)
    => caller.HasProperty(name, StaticBindingFlags);
    public static bool HasInstanceProperty(this Type caller, string name)
    => caller.HasProperty(name, InstanceBindingFlags);

    public static bool HasProperty
    (
        this Type caller, string name,
        BindingFlags bindingFlags
    )
    => caller.GetProperties(bindingFlags).HasNamedMember(name);
    public static bool HasProperty(this Type caller, string name)
    => caller.GetProperties().HasNamedMember(name);


    private static T FindNamedMember<T>(this T[] infos, string name) where T : MemberInfo
    => infos.FirstOrDefault(BindNamePredicate<T>(name));
    private static bool HasNamedMember<T>(this T[] infos, string name) where T : MemberInfo
    => infos.Any(BindNamePredicate<T>(name));
    private static Func<T, bool> BindNamePredicate<T>(string name) where T : MemberInfo
    => info => info.Name == name;
}