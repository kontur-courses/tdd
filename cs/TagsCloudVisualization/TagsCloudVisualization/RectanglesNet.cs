using System.Drawing;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace TagsCloudVisualization;

public class RectanglesNet
{
    private const float speed = 1;
    
    private Point _center;
    
    public Point CenterMass
    {
        get { return _centerOfMass.ToPoint(); }
    }
    public Point Center
    {
        get { return _center; }
    }

    private List<Rectangle> _rectangles;
    private FloatPoint _centerOfMass;

    public RectanglesNet(Point center)
    {
        _center = center;
        _centerOfMass = center.ToFloatPoint();
        _rectangles = new();
    }

    public Rectangle AddRectToNet(Size rectangleSize)
    {
        Vector2 direction = -new Vector2(
            _centerOfMass.X - _center.X, 
            _centerOfMass.Y - _center.Y
            );
        // Vector2 direction = Vector2.Zero;
        direction =
            direction
                .RandomRotate();
            //.Normalize();
        Point position = _center;
        Rectangle rect = new(position, rectangleSize);
        Rectangle intersectedRect;
        while (true)
        {
            intersectedRect = IntersectedRectangle(rect);
            if (intersectedRect == Rectangle.Empty)
            {
                UpdateRectangleList(rect);
                return rect;
            }
            else
            {
                rect = OustRect(rect, intersectedRect, direction);
            }

            position = UpdateCoord(position, direction); //TODO: out или ref
            rect.X = position.X;
            rect.Y = position.Y;
        }
    }

    public Point UpdateCoord(Point position, Vector2 direction)
    {
        position.X = (int)(position.X + direction.X * speed);
        position.Y = (int)(position.Y + direction.Y * speed);
        return position;
    }
    
    public Rectangle OustRect(Rectangle rect, Rectangle intersectedRect, Vector2 direction)
    {
        if (direction.X > 0)
            rect.X = intersectedRect.X + rect.Width;
        else
            rect.X = intersectedRect.X - rect.Width;
        
        if (direction.Y > 0)
            rect.Y = intersectedRect.Y + rect.Height;
        else
            rect.Y = intersectedRect.Y - rect.Height;
        
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
        var n = _rectangles.Count;
        _rectangles.Add(rect);
        _centerOfMass = (_centerOfMass*n + rect.Location.ToFloatPoint()) / (n+1);
    }
}

public class FloatPoint
{
    public float X { get; set; }
    public float Y { get; set; }
    
    public FloatPoint(float x, float y)
    {
        X = x;
        Y = y;
    }

    public FloatPoint(Point p)
    {
        X = p.X;
        Y = p.Y;
    }
    
    public static FloatPoint operator *(FloatPoint p, float n)
    {
        return new FloatPoint(
            p.X * n,
            p.Y * n
        );
    }
    
    public static FloatPoint operator +(FloatPoint p, FloatPoint other)
    {
        return new FloatPoint(
            p.X + other.X,
            p.Y + other.Y
        );
    }

    public static FloatPoint operator /(FloatPoint p, float n)
    {
        return new FloatPoint(
            p.X / n,
            p.Y / n
        );
    }

    public Point ToPoint()
    {
        return new Point(
            (int)X,
            (int)Y
        );
    }
}

public static class PointExtensions
{
    public static FloatPoint ToFloatPoint(this Point p)
    {
        return new FloatPoint(
            p.X,
            p.Y
            );
    }
}

public static class Vector2Extensions
{
    private const float dispersion = 3f;
    
    public static Vector2 RandomRotate(this Vector2 v)
    {
        if (v == Vector2.Zero)
            return Vector2.One.RandomRotate();
        return v.Rotate(new Random().NextSingle() * dispersion*2 - dispersion);
    }
    
    public static Vector2 Rotate(this Vector2 v, float angle) {
        var sin = Math.Sin(angle);
        var cos = Math.Cos(angle);
         
        var tx = v.X;
        var ty = v.Y;
        v.X = (float)((cos * tx) - (sin * ty));
        v.Y = (float)((sin * tx) + (cos * ty));
        return v;
    }

    public static Vector2 Normalize(this Vector2 v)
    {
        return new Vector2(
            v.X/v.Length(),
            v.Y/v.Length()
        );
    }
}