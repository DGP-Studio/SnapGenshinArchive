using System;
using System.Collections.Generic;

namespace Snap.Net.QueryString
{
    internal static class ListExtensions
    {
        public static bool RemoveFirstWhere<T>(this IList<T> list, Func<T, bool> shouldRemovePredicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (shouldRemovePredicate.Invoke(list[i]))
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }
    }
}