using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

public class CircularCloudLayouter
{
    public Point CenterPoint;
    public List<Rectangle> Rects;
    private double angle, radius;
    private double spiralParameter = 0.010;

    public CircularCloudLayouter(Point point)
    {
        CenterPoint = point;
        Rects = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rect = GetRectangle(rectangleSize);
        Rects.Add(rect);
        return rect;
    }

    public Point GetPossiblePoint()
    {
        var x = (int)Math.Round(radius * Math.Cos(angle));
        var y = (int)Math.Round(radius * Math.Sin(angle));

        radius += spiralParameter;
        angle += Math.PI / 180;

        return new Point(CenterPoint.X - x, CenterPoint.Y - y);
    }

    public Rectangle GetRectangle(Size rectSize)
    {
        Rectangle rect;
        do
        {
            rect = new Rectangle(GetPossiblePoint() - new Size(rectSize.Width / 2, rectSize.Height / 2),
                rectSize);

        } while (Rects.Where(rect.IntersectsWith).Any());

        return rect;
    }
}

