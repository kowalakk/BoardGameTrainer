using ArtificialInteligence;
using Game.IGame;

namespace ArtificialIntelligence
{
    internal static class RandomElementFromIEnumerableExtension
    {
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElementUsing<T>(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
    
}
