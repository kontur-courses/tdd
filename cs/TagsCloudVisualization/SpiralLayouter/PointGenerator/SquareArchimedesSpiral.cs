using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization.SpiralLayouter.PointGenerator;

public class SquareArchimedesSpiral : IPointGenerator
{
    public int Step { get; }

    public SquareArchimedesSpiral(int step)
    {
        if (step <= 0)
            throw new ArgumentException("Step should be positive number");
        
        Step = step;
    }
    
    public IEnumerable<Point> StartFrom(Point startPoint)
    {
        var neededPoints = 1;
        var pointsToPlace = 1;
        var direction = Direction.Up;
        var currentPoint = Point.Empty;

        while (true)
        {
            yield return currentPoint;
            
            pointsToPlace--;
            currentPoint += GetOffsetSize(direction);
            
            if (pointsToPlace == 0)
            {
                direction = direction.AntiClockwiseRotate();
                if (direction is Direction.Up or Direction.Down) neededPoints++;
                pointsToPlace = neededPoints;
            }
        }
    }
    
    private Size GetOffsetSize(Direction direction) => direction switch
    {
        Direction.Up => new Size(0, Step),
        Direction.Right => new Size(Step, 0),
        Direction.Down => new Size(0, -Step),
        Direction.Left => new Size(-Step, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}