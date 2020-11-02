using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

public class CircularCloudLayouter
{
    private Point CenterPoint;
    public List<Rectangle> Rects { get;}
    private double angle, radius;
    private double spiralParameter = 0.01;

    public CircularCloudLayouter(Point point)
    {
        if(point.X < 0 || point.Y < 0)
            throw new ArgumentException();
        CenterPoint = point;
        Rects = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            throw new ArgumentException();
        var rect = GetRectangle(rectangleSize);
        Rects.Add(rect);
        return rect;
    }

    private Point GetPossiblePoint()
    {
        var x = (int)Math.Round(radius * Math.Cos(angle));
        var y = (int)Math.Round(radius * Math.Sin(angle));

        radius += spiralParameter;
        angle += Math.PI / 180;

        return new Point(CenterPoint.X - x, CenterPoint.Y - y);
    }

    private Rectangle GetRectangle(Size rectSize)
    {
        Rectangle rect;
        do
        {
            rect = new Rectangle(GetPossiblePoint() - new Size(rectSize.Width / 2, rectSize.Height / 2),
                rectSize);

        } while (Rects.Where(rect.IntersectsWith).Any());

        return rect;
    }

    public void CreateImage(string path, string fileName)
    {
        if (!Rects.Any())
            throw new ArgumentException();
        var image = new Bitmap(1920, 1080);
        var graphics = Graphics.FromImage(image);
        graphics.DrawRectangles(new Pen(Color.Red), Rects.ToArray());

        image.Save($"{path}\\{fileName}.bmp", ImageFormat.Bmp);
    }
}

