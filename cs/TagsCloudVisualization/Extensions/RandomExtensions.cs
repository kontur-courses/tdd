using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class RandomExtensions
{
    public static Size NextSize(this Random random, int minLength, int maxLength)
    {
        if (minLength <= 0)
            throw new ArgumentOutOfRangeException(nameof(minLength), minLength, $"{nameof(minLength)} must be greater than zero.");
        
        var width = random.Next(minLength, maxLength);
        var height = random.Next(minLength, maxLength);
        
        return new Size(width, height);
    }
    
    public static Point NextPoint(this Random random, int minPos, int maxPos) => 
        new(random.Next(minPos, maxPos), random.Next(minPos, maxPos));
}