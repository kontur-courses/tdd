using System.Drawing;

namespace TagsCloudVisualization.Task_2;

public static class CloudLayout
{ 
    public static IEnumerable<Rectangle> GenerateCloudLayout(
        int amountRectangles,
        Size minSize,
        Size maxSize,
        CircularCloudLayouter cloudLayouter)
        => GenerateRectangleSizes(amountRectangles, minSize, maxSize)
            .Select(cloudLayouter.PutNextRectangle);

    private static IEnumerable<Size> GenerateRectangleSizes(
        int amountData,
        Size minSize,
        Size maxSize)
    {
        if (minSize.Width > maxSize.Width || minSize.Height > maxSize.Width)
            throw new ArgumentException("Минимальные размеры не могут быть больше максимальных");

        var random = new Random();

        for (var i = 0; i < amountData; i++)
        {
            var width = random.Next(minSize.Width, maxSize.Width + 1);
            var height = random.Next(minSize.Height, maxSize.Height + 1);

            yield return new Size(width, height);
        }
    }
}
