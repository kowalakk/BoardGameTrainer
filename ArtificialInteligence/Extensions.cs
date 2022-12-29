namespace AI
{
    internal static class RandomWalkExtension
    {
        private static readonly Random rand = new(); // TODO: add seed

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
    
}
