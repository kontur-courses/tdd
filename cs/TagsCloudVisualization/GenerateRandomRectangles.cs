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


// public class RectangleGenerator
// {
//     private readonly CircularCloudLayouter _layouter;
//
//     public RectangleGenerator(CircularCloudLayouter layouter)
//     {
//         _layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));
//     }
//
//     public List<Rectangle> GenerateRandomRectangles(int count)
//     {
//         if (count <= 0)
//             throw new ArgumentOutOfRangeException(nameof(count), "Count must be a positive number");
//
//         var rectangles = new List<Rectangle>();
//         var random = new Random(1);
//
//         for (var i = 0; i < count; i++)
//         {
//             var width = random.Next(40, 100);
//             var height = random.Next(20, 60);
//             var size = new Size(width, height);
//             var rectangle = _layouter.PutNextRectangle(size);
//             rectangles.Add(rectangle);
//         }
//
//         return rectangles;
//     }
// }
