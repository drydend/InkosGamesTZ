using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class ListExtention
    {
        public static void Add<T>(this List<T> origin,List<T> list)
        {
            foreach (var item in list)
            {
                origin.Add(item);
            }
        }

        public static T GetRandom<T>(this List<T> source)
        {
            return source[UnityEngine.Random.Range(0, source.Count)];
        }

        public static T GetRandom<T>(this List<T> source, out int index)
        {
            index = UnityEngine.Random.Range(0, source.Count);
            return source[index];
        }
    }
}
