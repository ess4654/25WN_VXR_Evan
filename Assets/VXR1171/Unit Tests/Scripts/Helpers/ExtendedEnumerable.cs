using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ExtendedEnumerable
{
    /// <summary>
    ///     Shuffles an enumerable list returning the new shuffled list.
    /// </summary>
    /// <typeparam name="T">Type of elements in list.</typeparam>
    /// <param name="list">Reference to the list.</param>
    /// <returns>Shuffled list.</returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list)
    {
        while (list == null) return null;
        list = list.OrderBy(x => Random.value);
        return list;
    }
}
