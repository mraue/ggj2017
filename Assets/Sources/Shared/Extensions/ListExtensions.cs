using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GGJ2017.Shared.Extensions
{
    public static class ListExtensions
    {
        public static T Next<T>(this List<T> list, T entry)
        {
            int index = list.IndexOf(entry);
            return list[list.NextIndex(index)];
        }

        public static T Previous<T>(this List<T> list, T entry)
        {
            int index = list.IndexOf(entry);
            return list[list.PreviousIndex(index)];
        }

        public static int NextIndex(this IList list, int index)
        {
            return index == list.Count - 1 ? 0 : index + 1;
        }

        public static int PreviousIndex(this IList list, int index)
        {
            return index == 0 ? list.Count - 1 : index - 1;
        }

        public static List<int> GetRange(int start, int exclusiveEnd)
        {
            var list = new List<int>();
            for (int i = start; i < exclusiveEnd; i++)
            {
                list.Add(i);
            }
            return list;
        }

        public static List<int> GetRange(int exclusiveEnd)
        {
            return GetRange(0, exclusiveEnd);
        }

        public static void Shuffle(this IList list)
        {
            var random = new Random();
            var indices = GetRange(list.Count);

            int randomIndex = indices[random.Next(indices.Count)];
            indices.Remove(randomIndex);
            int initialIndex = randomIndex;
            var val = list[initialIndex];
            var valNext = val;

            for (int i = 0; i < indices.Count; i++)
            {
                randomIndex = indices[random.Next(indices.Count)];
                indices.Remove(randomIndex);

                valNext = list[randomIndex];
                list[randomIndex] = val;                    
                val = valNext;
            }

            list[initialIndex] = val;
        }

        public static string ToFormattedString(this IList list)
        {
            var prefix = "";
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            foreach (var item in list)
            {
                stringBuilder.Append(string.Format("{0}{1}", prefix, item));
                prefix = ", ";
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }
    }
}