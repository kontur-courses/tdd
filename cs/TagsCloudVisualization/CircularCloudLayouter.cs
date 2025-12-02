using System.Drawing;

namespace TagsCloudVisualization;

public sealed class CircularCloudLayouter
{
    private const int MaxTouchNeighbors = 300; 
    private const int MaxCandidatesToEvaluate = 800;
    private const int PullMaxIterations = 1800;
    private const int BigStep = 16;
    private const int MaxCommonSteps = 3000;
    private const int MaxBigSteps = 1000;
    private const int CellSize = 60;
    
    private readonly Point center;
    private readonly int centerX;
    private readonly int centerY;
    private readonly Spiral spiral;
    private const int SpiralCandidateCount = 40;
    
    private readonly Dictionary<(int gx, int gy), List<Rectangle>> grid = new();
    private readonly List<Rectangle> cellBuffer = new(128); 
    private readonly List<Rectangle> candidatesBuffer = new(1024);
    private readonly HashSet<int> seenHashes = [];
    private readonly List<Rectangle> placedRectangles = [];
    public IReadOnlyList<Rectangle> PlacedRectangles => placedRectangles;
    
    private const int SpiralSearchMaxSteps = 20_000;
    private const int FallbackOffset = 50_000;
    
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        centerX = center.X;
        centerY = center.Y;
        spiral = new Spiral(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0)
            throw new ArgumentException("Rectangle width must be positive");
        if(rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle height must be positive");

        var rect = FindFreeSpaceForRectangle(rectangleSize);
        rect = ShiftToCenter(rect);

        placedRectangles.Add(rect);
        AddToGrid(rect);
        return rect;
    }
    
    private Rectangle FindFreeSpaceForRectangle(Size size)
    {
        if (placedRectangles.Count == 0)
            return CreateRectangleByCenter(center, size);

        candidatesBuffer.Clear();
        seenHashes.Clear();

        foreach (var neighbor in GetNearbyRectangles())
        {
            foreach (var c in GetTouchCandidates(neighbor, size))
            {
                TryAddCandidateForPlace(c);
            }
        }

        if (candidatesBuffer.Count == 0)
        {
            foreach (var c in GetSpiralCandidates(size, SpiralCandidateCount))
            {
                TryAddCandidateForPlace(c);
            }
        }

        if (CandidatesLimitExceeded())
        {
            candidatesBuffer.Sort(ByDistanceToCenter());
            candidatesBuffer.RemoveRange(MaxCandidatesToEvaluate, candidatesBuffer.Count - MaxCandidatesToEvaluate);
        }

        var best = Rectangle.Empty;
        var bestDist = double.MaxValue;
        foreach (var c in candidatesBuffer)
        {
            if (IntersectsPlacedRectangles(c)) continue;
            var d = DistanceToCenter(c);
            
            if (!(d < bestDist)) continue;
            bestDist = d;
            best = c;
        }

        return Math.Abs(bestDist - double.MaxValue) < 0.00000001 ? FindBySpiralFallback(size) : best;
    }

    private bool CandidatesLimitExceeded()
    {
        return candidatesBuffer.Count > MaxCandidatesToEvaluate;
    }

    private Comparison<Rectangle> ByDistanceToCenter()
    {
        return (a, b) => DistanceToCenter(a).CompareTo(DistanceToCenter(b));
    }

    private void TryAddCandidateForPlace(Rectangle rectangle)
    {
        var hash = rectangle.GetHashCode();
        if (seenHashes.Add(hash))
            candidatesBuffer.Add(rectangle);
    }

    private IEnumerable<Rectangle> GetNearbyRectangles()
    {
        var yielded = 0;

        const int radiusCells = 6;
        var cx = centerX / CellSize;
        var cy = centerY / CellSize;

        foreach (var r in EnumerateGridNeighborsAroundCenter(radiusCells, cx, cy, MaxTouchNeighbors - yielded))
        {
            yield return r;
            yielded++;
            if (yielded >= MaxTouchNeighbors) 
                yield break;
        }
        
        if (placedRectangles.Count > 0)
        {
            var last = placedRectangles[^1];
            foreach (var rectangle in GetRectanglesAround(last, maxCells: 4))
            {
                yield return rectangle;
                yielded++;
                if (yielded >= MaxTouchNeighbors) yield break;
            }
        }

        if (yielded >= MaxTouchNeighbors) 
            yield break;

        for (var i = placedRectangles.Count - 1; i >= 0 && yielded < MaxTouchNeighbors; i--)
        {
            yield return placedRectangles[i];
            yielded++;
        }

    }

    private IEnumerable<Rectangle> EnumerateGridNeighborsAroundCenter(int radiusCells, int cx, int cy, int maxToYield)
    {
        var produced = 0;
        for (var dx = -radiusCells; dx <= radiusCells && produced < maxToYield; dx++)
        {
            for (var dy = -radiusCells; dy <= radiusCells && produced < maxToYield; dy++)
            {
                if (!grid.TryGetValue((cx + dx, cy + dy), out var list))
                    continue;

                for (var i = list.Count - 1; i >= 0 && produced < maxToYield; i--)
                {
                    yield return list[i];
                    produced++;
                }
            }
        }
    }

    private IEnumerable<Rectangle> GetRectanglesAround(Rectangle r, int maxCells)
    {
        var x1 = Math.Max((r.Left / CellSize) - maxCells, int.MinValue);
        var x2 = Math.Min((r.Right / CellSize) + maxCells, int.MaxValue);
        var y1 = Math.Max((r.Top / CellSize) - maxCells, int.MinValue);
        var y2 = Math.Min((r.Bottom / CellSize) + maxCells, int.MaxValue);

        for (var x = x1; x <= x2; x++)
        for (var y = y1; y <= y2; y++)
            if (grid.TryGetValue((x, y), out var rectanglesInCell))
                foreach (var neighborRectangle  in rectanglesInCell)
                    yield return neighborRectangle;
    }

    private static IEnumerable<Rectangle> GetTouchCandidates(Rectangle o, Size size)
    {
        // left
        yield return new Rectangle(
            o.Left - size.Width,
            o.Top + (o.Height - size.Height) / 2,
            size.Width, size.Height);

        // right
        yield return new Rectangle(
            o.Right,
            o.Top + (o.Height - size.Height) / 2,
            size.Width, size.Height);

        // top
        yield return new Rectangle(
            o.Left + (o.Width - size.Width) / 2,
            o.Top - size.Height,
            size.Width, size.Height);

        // bottom
        yield return new Rectangle(
            o.Left + (o.Width - size.Width) / 2,
            o.Bottom,
            size.Width, size.Height);
    }

    private Rectangle ShiftToCenter(Rectangle rect)
    {
        var dx = (rect.X + (rect.Width >> 1) < centerX) ? +1 : -1;
        var dy = (rect.Y + (rect.Height >> 1) < centerY) ? +1 : -1;

        rect = ShiftAxis(rect, dx, 0);
        rect = ShiftAxis(rect, 0, dy);

        rect = PullTowardsCenter(rect);

        return rect;
    }

    private Rectangle ShiftAxis(Rectangle rect, int dx, int dy)
    {
        if (dx == 0 && dy == 0)
            return rect;

        rect = ShiftWithSteps(rect, dx, dy, BigStep, MaxBigSteps);
        rect = ShiftWithSteps(rect, dx, dy, 1, MaxCommonSteps);

        return rect;
    }
    
    private Rectangle ShiftWithSteps(Rectangle rect, int dx, int dy, int step, int maxIterations)
    {
        for (var i = 0; i < maxIterations; i++)
        {
            var shifted = rect with 
            { 
                X = rect.X + dx * step, 
                Y = rect.Y + dy * step 
            };

            if (IntersectsPlacedRectangles(shifted) || CrossedCenter(rect, shifted, dx, dy))
                return rect;

            rect = shifted;
        }

        return rect;
    }


    private Rectangle PullTowardsCenter(Rectangle rect)
    {
        var iter = 0;
        var moved = true;

        while (moved && iter++ < PullMaxIterations)
        {
            moved = false;
            var dx = rect.X + rect.Width / 2 < centerX ? -1 : 1;
            var dy = rect.Y + rect.Height / 2 < centerY ? -1 : 1;

            var shiftedX = rect with { X = rect.X - dx };
            if (!IntersectsPlacedRectangles(shiftedX))
            {
                rect = shiftedX;
                moved = true;
            }

            var shiftedY = rect with { Y = rect.Y - dy };
            if (IntersectsPlacedRectangles(shiftedY)) continue;
            rect = shiftedY;
            moved = true;
        }

        return rect;
    }

    private bool CrossedCenter(Rectangle oldRect, Rectangle newRect, int dx, int dy)
    {
        var oldCx = oldRect.X + oldRect.Width / 2.0;
        var oldCy = oldRect.Y + oldRect.Height / 2.0;

        var newCx = newRect.X + newRect.Width / 2.0;
        var newCy = newRect.Y + newRect.Height / 2.0;

        if (dx != 0)
            return Math.Abs(newCx - centerX) > Math.Abs(oldCx - centerX);

        if (dy != 0)
            return Math.Abs(newCy - centerY) > Math.Abs(oldCy - centerY);

        return false;
    }

    private void AddToGrid(Rectangle r)
    {
        foreach (var cell in GetCells(r))
        {
            if (!grid.TryGetValue(cell, out var list))
                grid[cell] = list = new List<Rectangle>(4);
            list.Add(r);
        }
    }

    private bool IntersectsPlacedRectangles(Rectangle rect)
    {
        cellBuffer.Clear();

        foreach (var cellCoords in GetCells(rect))
        {
            if (grid.TryGetValue(cellCoords, out var cellRectangles))
                cellBuffer.AddRange(cellRectangles);
        }

        return cellBuffer.Any(existing => rect.IntersectsWith(existing));
    }

    private static IEnumerable<(int, int)> GetCells(Rectangle r)
    {
        var x1 = (int)Math.Floor(r.Left / (double)CellSize);
        var x2 = (int)Math.Floor((r.Right - 1) / (double)CellSize);
        var y1 = (int)Math.Floor(r.Top / (double)CellSize);
        var y2 = (int)Math.Floor((r.Bottom - 1) / (double)CellSize);

        for (var x = x1; x <= x2; x++)
        for (var y = y1; y <= y2; y++)
            yield return (x, y);
    }

    private double DistanceToCenter(Rectangle rectangle)
    {
        var dx = rectangle.X + rectangle.Width/2 - centerX;
        var dy = rectangle.Y + rectangle.Width/2 - centerY;
        return dx * dx + dy * dy;
    }

    private static Rectangle CreateRectangleByCenter(Point center, Size size) =>
        new(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);

    private Rectangle FindBySpiralFallback(Size size)
    {
        for (var i = 0; i < SpiralSearchMaxSteps; i++)
        {
            var points = spiral.GetNextPoint();
            var rectangles = CreateRectangleByCenter(points, size);
            if (!IntersectsPlacedRectangles(rectangles))
                return rectangles;
        }

        return new Rectangle(centerX + FallbackOffset, centerY + FallbackOffset , size.Width, size.Height);
    }

    private IEnumerable<Rectangle> GetSpiralCandidates(Size size, int count = 10)
    {
        var found = 0;
        while (found < count)
        {
            var p = spiral.GetNextPoint();
            var rect = CreateRectangleByCenter(p, size);
            yield return rect;
            found++;
        }
    }
}
