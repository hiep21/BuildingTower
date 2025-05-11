using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    public class ToolRandom
    {
        private static uint seed = (uint)DateTime.UtcNow.Ticks; // có thể thay bằng Environment.TickCount

        public static void SetSeed(uint s)
        {
            seed = s != 0 ? s : 1; // tránh seed = 0
        }

        private static uint NextUInt()
        {
            seed ^= seed << 13;
            seed ^= seed >> 17;
            seed ^= seed << 5;
            return seed;
        }

        public static int Range(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException("min phải nhỏ hơn max");

            return (int)(NextUInt() % (max - min)) + min;
        }

        public static float NextFloat01()
        {
            return (NextUInt() & 0xFFFFFF) / (float)0x1000000; // 24-bit precision
        }
        // FLOAT: [min, max)

        public static float Range(float min, float max)
        {
            return min + (max - min) * NextFloat01();
        }
        public static int RangeInclusive(int min, int max)
        {
            return Range(min, max + 1);
        }


        // FLOAT: [0, 1)
        public static float Value()
        {
            return Range(0f, 1f);
        }

        // BOOL: 50/50
        public static bool RandomBool()
        {
            return (NextUInt() % 2) == 0;
        }

        // VECTOR2: random từ min đến max
        public static Vector2f RangeVector2(Vector2f min, Vector2f max)
        {
            return new Vector2f(
                Range(min.x, max.x),
                Range(min.y, max.y)
            );
        }

        public static Vector3f RangeVector3(Vector3f min, Vector3f max)
        {
            return new Vector3f(
                Range(min.x, max.x),
                Range(min.y, max.y),
                Range(min.z, max.z)
            );
        }


        // SHUFFLE: List<T>
        public static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        // SHUFFLE: Array
        public static void Shuffle<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Range(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        // PICK: random 1 phần tử từ List<T>
        public static T Pick<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List rỗng hoặc null.");
            int index = Range(0, list.Count);
            return list[index];
        }

        // PICK ENUM: random 1 giá trị enum
        public static T RandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = Range(0, values.Length);
            return (T)values.GetValue(index);
        }
        public static T RandomEnumExclude<T>(params T[] exclude) where T : Enum
        {
            Array allValues = Enum.GetValues(typeof(T));
            List<T> validValues = new List<T>();

            foreach (T value in allValues)
            {
                if (Array.IndexOf(exclude, value) == -1)
                    validValues.Add(value);
            }

            if (validValues.Count == 0)
                throw new InvalidOperationException("Không còn giá trị enum sau khi loại trừ.");

            int index = Range(0, validValues.Count);
            return validValues[index];
        }
    }
    public struct Vector2f
    {
        public float x;
        public float y;

        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"({x}, {y})";
    }
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() => $"({x}, {y}, {z})";
    }
    public static class ToolRandomND
    {
        /// <summary>
        /// Shuffle mảng n chiều (Array đa chiều: [,], [,,], ...)
        /// </summary>
        public static void Shuffle(Array arr)
        {
            if (arr == null || arr.Length == 0)
                throw new ArgumentException("Array is null or empty.");

            // Flatten array
            List<object> flat = new List<object>();
            foreach (var item in arr)
                flat.Add(item);

            // Shuffle
            ToolRandom.Shuffle(flat);

            // Gán lại vào array đa chiều
            int index = 0;
            foreach (var idx in arr.Indices())
            {
                arr.SetValue(flat[index++], idx);
            }
        }

        /// <summary>
        /// Shuffle list lồng nhau n cấp, giữ nguyên cấu trúc
        /// </summary>
        public static void ShuffleRecursive<T>(IList list)
        {
            // Flatten list n cấp
            List<T> flat = new List<T>();
            Flatten<T>(list, flat);

            // Shuffle
            ToolRandom.Shuffle(flat);

            // Gán lại
            int index = 0;
            Refill<T>(list, flat, ref index);
        }

        // Đệ quy flatten list n chiều
        private static void Flatten<T>(IList source, List<T> result)
        {
            foreach (var item in source)
            {
                if (item is IList subList && !(item is string))
                    Flatten<T>(subList, result);
                else
                    result.Add((T)item);
            }
        }

        // Đệ quy gán lại giá trị đã shuffle
        private static void Refill<T>(IList target, List<T> flat, ref int index)
        {
            for (int i = 0; i < target.Count; i++)
            {
                if (target[i] is IList subList && !(target[i] is string))
                {
                    Refill<T>((IList)target[i], flat, ref index);
                }
                else
                {
                    target[i] = flat[index++];
                }
            }
        }

        /// <summary>
        /// Extension: tạo IEnumerable tất cả index cho mảng n chiều
        /// </summary>
        private static IEnumerable<int[]> Indices(this Array array)
        {
            int[] lengths = new int[array.Rank];
            for (int i = 0; i < lengths.Length; i++)
                lengths[i] = array.GetLength(i);

            int[] current = new int[lengths.Length];
            while (true)
            {
                yield return (int[])current.Clone();

                int d = lengths.Length - 1;
                while (d >= 0)
                {
                    current[d]++;
                    if (current[d] < lengths[d]) break;
                    current[d] = 0;
                    d--;
                }
                if (d < 0) break;
            }
        }
    }

}