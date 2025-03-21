using System;
using UnityEngine;

/// <summary>
///     Extends the functionality of Predicates
/// </summary>
public static class ExtendedPredicate
{
    /// <summary>
    ///     Converts a predicate to type Func.
    /// </summary>
    /// <typeparam name="T">Type of the variable enclosed.</typeparam>
    /// <param name="predicate">Reference to the predicate.</param>
    /// <returns>Converted Predicate to Func</returns>
    public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate) =>
        x => predicate(x);
}