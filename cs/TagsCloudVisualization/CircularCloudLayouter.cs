using System.Drawing;
using TagsCloudVisualization.Interface;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> placedRectangles = new();
    private readonly IPointGenerator pointGenerator;
    private readonly AppSettings appSettings;
    private readonly Dictionary<Point, List<Rectangle>> grid = new Dictionary<Point, List<Rectangle>>();
    private readonly int gridSize;
    public List<Rectangle> PlacedRectangles => placedRectangles;

    public CircularCloudLayouter(AppSettings appSettings,  IPointGenerator pointGenerator)
    {
        this.appSettings = appSettings;
        this.pointGenerator = pointGenerator;
        gridSize = appSettings.MaxFontSize;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (placedRectangles.Count == 0)
        {
            var topLeft = CalculateTopLeft(pointGenerator.Center, rectangleSize);
            var centerRect = new Rectangle(topLeft.X, topLeft.Y, rectangleSize.Width, rectangleSize.Height);
            if (!IsWithinImageBounds(centerRect))
            {
                throw new InvalidOperationException($"Прямоугольник не помещается в изображение.");
            }
            placedRectangles.Add(centerRect);
            AddRectangleToGrid(centerRect);
            return centerRect;
        }

        foreach (var point in pointGenerator.GeneratePoints())
        {
            var topLeft = CalculateTopLeft(point, rectangleSize);
            var potentialRect = new Rectangle(topLeft.X, topLeft.Y, rectangleSize.Width, rectangleSize.Height);

            if (!IsIntersection(potentialRect))
            {
                if (!IsWithinImageBounds(potentialRect))
                    throw new InvalidOperationException($"Прямоугольник не помещается в изображение.");
                AddRectangleToGrid(potentialRect);
                var compactedRect = CompactRectangle(potentialRect);
                placedRectangles.Add(compactedRect);
                return compactedRect;
            }
        }
        
        throw new InvalidOperationException("Не удалось найти место для прямоугольника.");
    }

    private bool IsIntersection(Rectangle newRectangle)
    {
        var rectWithIndentation = AddIndentation(newRectangle, appSettings.Padding);
        
        var minCellX = rectWithIndentation.Left / gridSize;
        var maxCellX = rectWithIndentation.Right / gridSize;
        var minCellY = rectWithIndentation.Top / gridSize;
        var maxCellY = rectWithIndentation.Bottom / gridSize;
        
        for (int x = minCellX; x <= maxCellX; x++)
        {
            for (int y = minCellY; y <= maxCellY; y++)
            {
                var cellKey = new Point(x, y);
                if (grid.TryGetValue(cellKey, out List<Rectangle> cellRectangles))
                {
                    foreach (var existingRect in cellRectangles)
                    {
                        var existingRectWithIndentation = AddIndentation(existingRect, appSettings.Padding);
                    
                        if (rectWithIndentation.IntersectsWith(existingRectWithIndentation))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
    
    private void AddRectangleToGrid(Rectangle rect)
    {
        var minCellX = rect.Left / gridSize;
        var maxCellX = rect.Right / gridSize;
        var minCellY = rect.Top / gridSize;
        var maxCellY = rect.Bottom / gridSize;

        for (int x = minCellX; x <= maxCellX; x++)
        {
            for (int y = minCellY; y <= maxCellY; y++)
            {
                var cellKey = new Point(x, y);
            
                if (!grid.ContainsKey(cellKey))
                {
                    grid[cellKey] = new List<Rectangle>();
                }
                grid[cellKey].Add(rect);
            }
        }
    }
    
    private Rectangle AddIndentation(Rectangle rect, int padding)
    {
        return new Rectangle(
            rect.X - padding,
            rect.Y - padding,
            rect.Width + 2 * padding,
            rect.Height + 2 * padding
        );
    }

    private Point CalculateTopLeft(Point targetCenter, Size size)
    {
        var x = targetCenter.X - size.Width / 2;
        var y = targetCenter.Y - size.Height / 2;
        return new Point(x, y);
    }
    
    private Rectangle CompactRectangle(Rectangle rect)
    {
        var currentRect = rect;
        var centerX = pointGenerator.Center.X;
        var centerY = pointGenerator.Center.Y;
        var stepX = (currentRect.X + currentRect.Width / 2 < centerX) ? 1 : -1;
        var stepY = (currentRect.Y + currentRect.Height / 2 < centerY) ? 1 : -1;
        
        while (true) 
        {
            var nextRect = new Rectangle(currentRect.X + stepX, currentRect.Y, currentRect.Width, currentRect.Height);
            if (IsIntersection(nextRect))
                break; 
            
            var nextCenterX = nextRect.X + nextRect.Width / 2;
            if (stepX > 0 && nextCenterX >= centerX || stepX < 0 && nextCenterX <= centerX)
            {
                currentRect = nextRect;
                break;
            }
            
            currentRect = nextRect;
        }
        
        while (true)
        {
            var nextRect = new Rectangle(currentRect.X, currentRect.Y + stepY, currentRect.Width, currentRect.Height);
            if (IsIntersection(nextRect))
                break; 
        
            var nextCenterY = nextRect.Y + nextRect.Height / 2;
            if (stepY > 0 && nextCenterY >= centerY || stepY < 0 && nextCenterY <= centerY)
            {
                currentRect = nextRect; 
                break;
            }

            currentRect = nextRect;
        }

        return currentRect;
    }
    
    private bool IsWithinImageBounds(Rectangle rect)
    {
        var w = appSettings.ImageSize.Width;
        var h = appSettings.ImageSize.Height;
        
        return rect.Left >= 0 && rect.Right <= w &&
               rect.Top >= 0 && rect.Bottom <= h;
    }
    
}