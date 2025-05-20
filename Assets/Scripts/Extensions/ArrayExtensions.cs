using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ArrayExtensions
{
    public static bool IsNullOrEmpty<T>(this T[] array)
    => array == null || array.Length == 0;

    public static T[] Coalesce<T>(this T[] array, T[] defaultArray)
    => array.IsNullOrEmpty() ? defaultArray : array;

    public static T PickRandom<T>(this T[] array)
    => array[Random.Range(0, array.Length)];

    public static T PickRandomExcluding<T>(this T[] array, T exclude)
    {
        List<T> arrayCopy = array.ToList();
        arrayCopy.Remove(exclude);
        return arrayCopy.ToArray().PickRandom();
    }
}