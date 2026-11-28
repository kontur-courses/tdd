using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Examples;

public class RandomSizeGenerator
{
    private readonly Size minSize;
    private readonly Size maxSize;
    private readonly Random random;

    public RandomSizeGenerator(Size minSize, Size maxSize, int seed)
    {
        ValidateSizeRange(minSize, maxSize);
        this.minSize = minSize;
        this.maxSize = maxSize;
        random = new Random(seed);
    }

    public IEnumerable<Size> Generate(int count)
    {
        throw new NotImplementedException();
    }

    private static void ValidateSizeRange(Size minSize, Size maxSize)
    {
        throw new NotImplementedException();
    }
}
