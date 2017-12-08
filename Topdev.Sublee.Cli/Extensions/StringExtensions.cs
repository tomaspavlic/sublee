public static class StringExtensions
{
    public static string Ellipsis(this string value, int limit)
    {
        if (value.Length > limit)
        {
            return value.Substring(0, limit - 3) + "...";
        }

        return value;
    }
}