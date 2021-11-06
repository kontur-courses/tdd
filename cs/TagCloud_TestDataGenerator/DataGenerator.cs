using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud_TestDataGenerator
{
    public static class DataGenerator
    {
        private const int LowerSize = 15;
        private const int HigherSize = 150;

        private static readonly Random Rnd;

        static DataGenerator()
        {
            Rnd = new Random();
        }

        public static IEnumerable<Size> GetNextNSizes(int n)
        {
            for (var i = 0; i < n; i++)
            {
                var width = Rnd.Next(LowerSize, HigherSize);
                var height = Rnd.Next(LowerSize, HigherSize);
                yield return new Size(width, height);
            }
        }
    }
}