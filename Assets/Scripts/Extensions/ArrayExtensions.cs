using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ArrayExtensions
{
    public static T PickRandom<T>(this T[] array)
    => array[Random.Range(0, array.Length)];

    public static T PickRandomExcluding<T>(this T[] array, T exclude)
    {
        List<T> arrayCopy = array.ToList();
        arrayCopy.Remove(exclude);
        return arrayCopy.ToArray().PickRandom();
    }
}