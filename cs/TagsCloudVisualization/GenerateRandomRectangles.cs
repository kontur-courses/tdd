using System.Drawing;

namespace TagsCloudVisualization;

public class GenerateRandomRectangles
{
    public List<Rectangle> RectangleGenerator(CircularCloudLayouter layouter, int count)
    {
        var rectangles = new List<Rectangle>();
        var random = new Random(1);
        for (var i = 0; i < count; i++)
        {
            var size = new Size(random.Next(40,100), random.Next(20, 60));
            rectangles.Add(layouter.PutNextRectangle(size));
        }

        return rectangles;
    }
}
