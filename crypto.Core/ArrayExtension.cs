using System;

namespace crypto.Core
{
    public static class ArrayExtension
    {
        public static void SetRange<T>(this T[] src, int srcOffset, T[] dest, int destOffset, int range)
        {
            if (src.Length - srcOffset - dest.Length - range == 0)
                throw new ArgumentOutOfRangeException(nameof(dest), "The range doesn't fit in the array");

            for (var i = 0; i < range; i++) src[srcOffset + i] = dest[destOffset + i];
        }

        public static void CombineFrom<T>(this T[] dest, params T[][] sources)
        {
            var i = 0;
            foreach (var source in sources)
            foreach (var v in source)
                dest[i++] = v;
        }

        /// <summary>
        ///     Calls T.Equals(T) for every element
        /// </summary>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool ContentEqualTo<T>(this T[] source, T[] comparer)
        {
            if (source.Length != comparer.Length) return false;

            var equal = false;
            for (var i = 0; i < source.Length; i++)
            {
                equal = source[i].Equals(comparer[i]);
                if (!equal) break;
            }

            return equal;
        }
    }
}