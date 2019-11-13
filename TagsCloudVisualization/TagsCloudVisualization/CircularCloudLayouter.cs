using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

public class CircularCloudLayouter
{
    readonly public Point cloudCenter;
    public List<Rectangle> RectanglesList { get; private set; } = new List<Rectangle>();
    public int RectangleCount => RectanglesList.Count;
    private double spiralK = 1;
    private double spiralAngle = 0;
    private readonly double angleDelta = 0.01;


    public CircularCloudLayouter(Point center)
    {
        if (center.Y < 0 || center.X < 0)
            throw new ArgumentException("Coordinates of center must be not negative");
        cloudCenter = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectangle = Rectangle.Empty;
        if (RectanglesList.Count == 0)
        {
            var newLeftTopCorner = new Point(
                Math.Max(cloudCenter.X - rectangleSize.Width / 2,0),
                Math.Max(cloudCenter.Y - rectangleSize.Height / 2, 0) + rectangleSize.Height);
            rectangle = new Rectangle(newLeftTopCorner, rectangleSize);
        }
        else
        {
            int possibleX = 0;
            int possibleY = 0;
            do
            {
                spiralAngle += angleDelta;
                possibleX = (int)(spiralK * spiralAngle * Math.Cos(spiralAngle) + cloudCenter.X -
                                rectangleSize.Width / 2.0);
                possibleY = (int)(spiralK * spiralAngle * Math.Sin(spiralAngle) + cloudCenter.Y +
                                rectangleSize.Height / 2.0);
                rectangle = new Rectangle(new Point(possibleX, possibleY), rectangleSize);
            } while ((possibleX < 0 || possibleY < 0) || 
                     RectanglesList.Any(r => r.IntersectsWith(rectangle)));
        }
        RectanglesList.Add(rectangle);
        return rectangle;
    }
}