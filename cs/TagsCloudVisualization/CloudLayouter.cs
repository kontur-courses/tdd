using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization;

public class CloudLayouter
{
    private readonly IPointGenerator pointsGenerator;
    private readonly List<Rectangle> createdRectangles = new();

    public List<Rectangle> CreatedRectangles => createdRectangles.ToList();

    public CloudLayouter(IPointGenerator pointGenerator)
    {
        pointsGenerator = pointGenerator;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
            throw new ArgumentException("Rectangle can't have negative width or height");

        while (true)
        {
            var nextPoint = pointsGenerator.GetNextPoint();

            var rectangleLocation = new Point(nextPoint.X - rectangleSize.Width / 2,
                nextPoint.Y - rectangleSize.Height / 2);

            var newRectangle = new Rectangle(rectangleLocation, rectangleSize);
            if (createdRectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle))) continue;

            createdRectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}