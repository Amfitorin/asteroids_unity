using System;
using System.Reflection;

namespace Core.Utils.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsAssignableFromGeneric(this Type generic, Type type)
        {
            return IsAssignableFromGeneric(generic, type, out _);
        }

        public static bool IsAssignableFromGeneric(this Type generic, Type type, out Type[] arguments)
        {
            arguments = null;
            if (!generic.IsGenericTypeDefinition)
            {
                arguments = generic.IsGenericType ? generic.GetGenericArguments() : new[] {generic};
                return generic.IsAssignableFrom(type);
            }

            var interfaceTypes = type.GetInterfaces();
            foreach (var it in interfaceTypes)
                if (it.IsGenericType && it.GetGenericTypeDefinition() == generic)
                {
                    arguments = it.GetGenericArguments();
                    return true;
                }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
            {
                arguments = type.GetGenericArguments();
                return true;
            }

            var baseType = type.BaseType;
            return baseType != null && IsAssignableFromGeneric(generic, baseType, out arguments);
        }

        public static bool HasAttribute<TAttribute>(this FieldInfo info)
        {
            var objs = info.GetCustomAttributes(typeof(TAttribute), true);
            return objs.Length > 0;
        }
    }
}