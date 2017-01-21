using System;

namespace GGJ2017.Shared.Extensions
{
    public static class EnumExtensions
    {
        public static T ParseEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}