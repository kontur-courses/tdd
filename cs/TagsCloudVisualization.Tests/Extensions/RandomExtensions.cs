using System;
using System.Linq;

namespace TagsCloudVisualization.Tests.Extensions
{
    public static class RandomExtensions
    {
        public static int[] GetRandomSequence(this Random random, int minLength = 100, int maxLength = 500) =>
            Enumerable.Range(0, random.Next(minLength, maxLength)).Select(i => random.Next()).ToArray();
    }
}