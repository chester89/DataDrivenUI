using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDrivenUI
{
    public static class ReflectionExtensions
    {
        public static bool IsItselfOrSubclassOf<T>(this Type type)
        {
            return type == typeof (T) || type.IsSubclassOf(typeof (T));
        }

        public static bool IsSubclassOf<T>(this Type type)
        {
            return type.IsSubclassOf(typeof (T));
        }

        public static bool IsAssignableFrom<T>(this Type type)
        {
            return type.IsAssignableFrom(typeof (T));
        }

        public static bool IsAssignableTo<T>(this Type type)
        {
            return typeof (T).IsAssignableFrom(type);
        }
    }
}
