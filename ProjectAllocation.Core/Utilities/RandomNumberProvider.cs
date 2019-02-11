using System;

namespace ProjectAllocation.Core.Utilities
{
    public class RandomNumberProvider : IRandomNumberProvider
    {
        // This is random enough for our purposes
        private static readonly Random Random = new Random();

        public int NextInt(int maxValue)
        {
            return Random.Next(maxValue);
        }
    }
}
