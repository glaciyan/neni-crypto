#nullable enable

namespace crypto.Desktop.Cnsl
{
    public static class StringArrayUtil
    {
        public static string? TryGetString(this string[] array, int index)
        {
            if (array.Length > index)
                return array[index];
            return null;
        }
    }
}