using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace TagsCloudVisualization;

public class RectanglesNet
{
    private const float alpha = 0.1f;
    
    private Point _center;
    
    private List<Rectangle> _rectangles;
    private Spiral _spiral;

    public List<Rectangle> Rectangles
    {
        get
        {
            return _rectangles;
        }
    }

    public RectanglesNet(Point center)
    {
        _center = center;
        _rectangles = new();
        _spiral = new(alpha, center);
    }

    public Rectangle AddRectToNet(Size rectangleSize)
    {
        _spiral.SetNewStartAngle(new Random().NextSingle() * (float)Math.PI * 2);
        Rectangle rect = new(_center, rectangleSize);
        Rectangle intersectedRect;
        while (true)
        {
            intersectedRect = IntersectedRectangle(rect);
            if (intersectedRect == Rectangle.Empty)
            {
                UpdateRectangleList(rect);
                return rect;
            }
            rect = UpdateCoord(rect);
        }
    }

    public Rectangle UpdateCoord(Rectangle r)
    {
        _spiral.IncreaseAngle(Math.Min(r.Width, r.Height));
        return new Rectangle(
            _spiral.ToCartesian(),
            r.Size
            );
    }
    
    public Rectangle OustRect(Rectangle rect, Rectangle intersectedRect, Vector2 direction)
    {
        if (intersectedRect.Width > intersectedRect.Height)
        {
            if (direction.Y > 0)
                rect.Y = intersectedRect.Y + rect.Height;
            else
                rect.Y = intersectedRect.Y - rect.Height;
        }
        else
        {
            if (direction.X > 0)
                rect.X = intersectedRect.X + rect.Width;
            else
                rect.X = intersectedRect.X - rect.Width;
        }
        return rect;
    }

    public Rectangle IntersectedRectangle(Rectangle rect)
    {
        var intersectedRect = 
            _rectangles
            .FirstOrDefault(r => r.IntersectsWith(rect));
        intersectedRect.Intersect(rect);
        return intersectedRect;
    }

    private void UpdateRectangleList(Rectangle rect)
    {
        _rectangles.Add(rect);
    }
}

class Spiral
{
    private float _alpha;
    private float _phi0;
    private Point _center;
    
    private float _phi;
    private float R
    {
        get { return _alpha * (_phi - _phi0); }
    }

    public Spiral(float alpha, Point center, float phi0 = 0)
    {
        _alpha = alpha;
        _phi0 = phi0;
        _center = center;
    }
    
    public void SetNewStartAngle(float phi0)
    {
        _phi0 = phi0;
        _phi = phi0;
    }

    public void IncreaseAngle(float angle)
    {
        _phi += angle;
    }

    public Point ToCartesian()
    {
        return new Point(
            (int)(R * Math.Cos(_phi)) + _center.X,
            (int)(R * Math.Sin(_phi) + _center.Y)
            );
    }
}