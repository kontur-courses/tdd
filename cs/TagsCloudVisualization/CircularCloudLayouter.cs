using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private const double CircularStep = 2;
    private const double PolarAngleStep = 1;
    private readonly Point center;

    private readonly List<Rectangle> rectangles = new();

    private readonly Rectangle viewBoard;

    private int circularExitViewBoardCount;

    private Point circularPoint;

    private double polarCircularAngle;

    public CircularCloudLayouter(Point center)
    {
        if (center.IsEmpty || center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("center point is invalid", nameof(center));
        this.center = center;
        viewBoard = new(Point.Empty, new Size(center) * 2);
        circularPoint = center;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width > viewBoard.Width || rectangleSize.Height > viewBoard.Height)
            throw new ArgumentException("rectangleSize is greater than view board", nameof(rectangleSize));

        if (rectangles.Count == 0)
        {
            var position = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
            var rectangle = new Rectangle(position, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        while (circularExitViewBoardCount < 100)
        {
            if (!viewBoard.Contains(circularPoint))
            {
                circularExitViewBoardCount++;
                var polarCircularP = CircularStep * polarCircularAngle;
                circularPoint = new((int)Math.Round(center.X + polarCircularP * Math.Cos(polarCircularAngle)),
                    (int)Math.Round(center.Y + polarCircularP * Math.Sin(polarCircularAngle)));
                polarCircularAngle += PolarAngleStep;
            }

            var rectangle = new Rectangle(circularPoint, rectangleSize);
            var intersectsWithAny = rectangles.Any(x => x.IntersectsWith(rectangle));
            if (intersectsWithAny)
            {
                var polarCircularP = CircularStep * polarCircularAngle;
                circularPoint = new((int)Math.Round(center.X + polarCircularP * Math.Cos(polarCircularAngle)),
                    (int)Math.Round(center.Y + polarCircularP * Math.Sin(polarCircularAngle)));
                polarCircularAngle += PolarAngleStep;
            }
            else
            {
                for (var i = 0; i < 10; i++)
                {
                    var rectangleCenterX = rectangle.X - rectangle.Width / 2;
                    var rectangleCenterY = rectangle.Y - rectangle.Height / 2;
                    var shiftOffset = Point.Empty;
                    if (rectangleCenterX < center.X && rectangleCenterY < center.Y)
                        shiftOffset = new(1, 1);
                    else if (rectangleCenterX > center.X && rectangleCenterY < center.Y)
                        shiftOffset = new(-1, 1);
                    else if (rectangleCenterX > center.X && rectangleCenterY > center.Y)
                        shiftOffset = new(-1, -1);
                    else if (rectangleCenterX < center.X && rectangleCenterY > center.Y)
                        shiftOffset = new(1, -1);
                    else if (rectangleCenterX < center.X && rectangleCenterY == center.Y)
                        shiftOffset = new(1, 0);
                    else if (rectangleCenterX > center.X && rectangleCenterY == center.Y)
                        shiftOffset = new(-1, 0);
                    else if (rectangleCenterY < center.Y && rectangleCenterX == center.X)
                        shiftOffset = new(0, 1);
                    else if (rectangleCenterY > center.Y && rectangleCenterX == center.X) shiftOffset = new(0, -1);

                    var shiftPosition = rectangle.Location;
                    shiftPosition.Offset(shiftOffset);
                    var shiftRectangle = new Rectangle(shiftPosition, rectangleSize);
                    var checkRectangleWithAny = rectangles.Any(x => x.IntersectsWith(shiftRectangle));
                    if (checkRectangleWithAny)
                        break;
                    rectangle = shiftRectangle;
                }

                rectangles.Add(rectangle);

                return rectangle;
            }
        }

        throw new InvalidOperationException("circular exited out of viewBoard");
    }
}