namespace Superdoku.Tools
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            Random rnd = new Random();
            return source.OrderBy(i => rnd.Next());
        }
    }
}
