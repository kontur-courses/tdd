using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization;

public class CircleLayer
{
    public enum Sector
    {
        TopRight,
        BottomRight,
        BottomLeft,
        TopLeft
    }

    public Point Center { get; }
    public int Radius { get; private set; }


    private Sector currentSector;
    private readonly List<Rectangle> storage;
    private readonly List<int> layerRectangles = [];

    private CircleLayer(Point center, int radius, List<Rectangle> storage)
    {
        Center = center;
        Radius = radius;
        currentSector = Sector.TopRight;
        this.storage = storage;
    }

    public CircleLayer(Point center, List<Rectangle> storage) : this(center, 0, storage)
    { }

    public void OnSuccessInsertRectangle()
    {
        if (storage.Count == 0) throw new InvalidOperationException("Rectangle was not added");
        if (storage.Count != 1) currentSector = GetNextClockwiseSector();
        layerRectangles.Add(storage.Count - 1);
        if (ShouldCreateNewLayer())
            CreateNextLayerAndChangeCurrentOnNext();
    }

    private bool ShouldCreateNewLayer()
    {
        return currentSector == Sector.TopRight;
    }

    private Sector GetNextClockwiseSector()
    {
        return currentSector == Sector.TopLeft ? Sector.TopRight : currentSector + 1;
    }

    private void CreateNextLayerAndChangeCurrentOnNext()
    {
        var nextLayer = new CircleLayer(Center, CalculateRadiusForNextLayer(), storage);
        Radius = nextLayer.Radius;
        currentSector = nextLayer.currentSector;
        var rectanglesForNextRadius = RemoveRectangleInCircle();
        layerRectangles.Clear();
        layerRectangles.AddRange(rectanglesForNextRadius);
    }

    private int CalculateRadiusForNextLayer()
    {
        return layerRectangles
            .Select(ind => CalculateDistanceBetweenCenterAndRectangleFarCorner(storage[ind]))
            .Min();
    }

    private List<int> RemoveRectangleInCircle()
    {
        return layerRectangles
            .Where(i => CalculateDistanceBetweenCenterAndRectangleFarCorner(storage[i]) > Radius)
            .ToList();
    }

    private int CalculateDistanceBetweenCenterAndRectangleFarCorner(Rectangle rectangle)
    {
        var distanceToCorners = new List<int>
        {
            Center.CalculateDistanceBetween(new Point(rectangle.Right, rectangle.Top)),
            Center.CalculateDistanceBetween(new Point(rectangle.Right, rectangle.Bottom)),
            Center.CalculateDistanceBetween(new Point(rectangle.Left, rectangle.Bottom)),
            Center.CalculateDistanceBetween(new Point(rectangle.Left, rectangle.Top))
        };
        return distanceToCorners.Max();
    }

    private Point PutToCenter(Size rectangleSize)
    {
        var rectangleX = Center.X - rectangleSize.Width / 2;
        var rectangleY = Center.Y - rectangleSize.Height / 2;

        return new Point(rectangleX, rectangleY);
    }

    public Point AddNextRectangle(Size rectangleSize)
    {
        if (Radius == 0) return PutToCenter(rectangleSize);
        var rectangleStartPositionOnCircle = GetStartSectorPointOnCircleBySector(currentSector);
        return currentSector switch
        {
            Sector.TopRight => new Point(rectangleStartPositionOnCircle.X,
                rectangleStartPositionOnCircle.Y -= rectangleSize.Height),
            Sector.BottomRight =>
                rectangleStartPositionOnCircle,
            Sector.BottomLeft =>
                new Point(rectangleStartPositionOnCircle.X - rectangleSize.Width,
                    rectangleStartPositionOnCircle.Y),
            _ =>
                new Point(rectangleStartPositionOnCircle.X - rectangleSize.Width,
                    rectangleStartPositionOnCircle.Y - rectangleSize.Height)
        };
    }

    private Point GetStartSectorPointOnCircleBySector(Sector s)
    {
        return s switch
        {
            Sector.TopRight => new Point(Center.X, Center.Y - Radius),
            Sector.BottomRight => new Point(Center.X + Radius, Center.Y),
            Sector.BottomLeft => new Point(Center.X, Center.Y + Radius),
            _ => new Point(Center.X - Radius, Center.Y)
        };
    }

    public Point GetRectanglePositionWithoutIntersection(Rectangle forInsertion, Rectangle intersected)
    {
        if (IsRectangleIntersectSymmetryAxis(new Rectangle(forInsertion.Location, forInsertion.Size)))
        {
            currentSector = GetNextClockwiseSector();
            if (ShouldCreateNewLayer()) CreateNextLayerAndChangeCurrentOnNext();
            forInsertion.Location = AddNextRectangle(forInsertion.Size);
        }

        var nextPosition = CalculateNewPositionWithoutIntersectionBySector(currentSector, forInsertion, intersected);

        return nextPosition;
    }

    private bool IsRectangleIntersectSymmetryAxis(Rectangle rectangle)
    {
        return (rectangle.Left < Center.X && rectangle.Right > Center.X) ||
               (rectangle.Bottom > Center.Y && rectangle.Top < Center.Y);
    }

    private Point CalculateNewPositionWithoutIntersectionBySector(Sector whereIntersected, Rectangle forInsertion,
        Rectangle intersected)
    {
        var isMovingAxisIsX = IsMovingAxisIsXBySector(whereIntersected);
        var distanceForMoving =
            CalculateDistanceForMoveClockwiseToPositionWithoutIntersection(whereIntersected, forInsertion, intersected);

        int distanceForBringBackOnCircle;
        if (IsRectangleBetweenSectors(distanceForMoving, forInsertion.Location, isMovingAxisIsX))
        {
            distanceForBringBackOnCircle = Radius;
        }
        else
        {
            var nearestForCenterCorner =
                CalculateCornerNearestForCenterAfterMove(whereIntersected, distanceForMoving, forInsertion);
            distanceForBringBackOnCircle =
                CalculateDeltaForBringRectangleBackOnCircle(nearestForCenterCorner, isMovingAxisIsX, forInsertion);
        }

        distanceForMoving *= CalculateMoveMultiplierForMoveClockwise(isMovingAxisIsX, forInsertion);
        distanceForBringBackOnCircle *= CalculateMoveMultiplierForMoveFromCenter(!isMovingAxisIsX, forInsertion);
        return isMovingAxisIsX
            ? new Point(forInsertion.X + distanceForMoving, forInsertion.Y + distanceForBringBackOnCircle)
            : new Point(forInsertion.X + distanceForBringBackOnCircle, forInsertion.Y + distanceForMoving);
    }

    private bool IsRectangleBetweenSectors(int distanceForMoving, Point forInsertionLocation, bool isMovingAxisIsX)
    {
        var distanceToCenter = Math.Abs(isMovingAxisIsX
            ? forInsertionLocation.X - Center.X
            : forInsertionLocation.Y - Center.Y);
        return distanceForMoving > distanceToCenter;
    }

    private int CalculateDeltaForBringRectangleBackOnCircle(Point nearestForCenterCorner, bool isMovingAxisIsX,
        Rectangle forInsertion)
    {
        Func<Point, int> getAxisForBringBackOnCircle = isMovingAxisIsX ? p => p.Y : p => p.X;
        Func<Point, int> getStaticAxis = isMovingAxisIsX ? p => p.X : p => p.Y;

        var distanceOnStaticAxis = Math.Abs(getStaticAxis(nearestForCenterCorner) - getStaticAxis(Center));
        var distanceOnAxisForBringBackOnCircle = Math.Abs(getAxisForBringBackOnCircle(nearestForCenterCorner) -
                                                          getAxisForBringBackOnCircle(Center));
        var distanceBetweenCornerAndCenter = Center.CalculateDistanceBetween(nearestForCenterCorner);
        if (distanceBetweenCornerAndCenter > Radius)
            return CalculateMoveMultiplierForMoveToCenter(!isMovingAxisIsX, forInsertion)
                   * WhenRectangleOutsideCircle(distanceOnStaticAxis, distanceBetweenCornerAndCenter,
                       distanceOnAxisForBringBackOnCircle);

        return CalculateMoveMultiplierForMoveFromCenter(!isMovingAxisIsX, forInsertion)
               * WhenRectangleInCircle(distanceOnStaticAxis, distanceOnAxisForBringBackOnCircle);
    }

    private int WhenRectangleOutsideCircle(int distanceOnStaticAxis, int distanceBetweenCornerAndCenter,
        int distanceOnAxisForBringBackOnCircle)
    {
        var inCircleCathetusPart = Math.Sqrt(Math.Abs(Radius * Radius - distanceOnStaticAxis * distanceOnStaticAxis));
        return CalculatePartCathetus(distanceBetweenCornerAndCenter, inCircleCathetusPart,
            distanceOnAxisForBringBackOnCircle);
    }

    private int WhenRectangleInCircle(int distanceOnStaticAxis, int distanceOnAxisForBringBackOnCircle)
    {
        return CalculatePartCathetus(Radius, distanceOnStaticAxis, distanceOnAxisForBringBackOnCircle);
    }

    private int CalculatePartCathetus(int hypotenuse, double a, int b)
    {
        return (int)Math.Ceiling(Math.Sqrt(Math.Abs(hypotenuse * hypotenuse - a * a))) - b;
    }

    private Point CalculateCornerNearestForCenterAfterMove(Sector whereIntersected, int distanceForMoving,
        Rectangle forMove)
    {
        var isAxisForMoveIsX = IsMovingAxisIsXBySector(whereIntersected);
        var moveMultiplier = CalculateMoveMultiplierForMoveClockwise(isAxisForMoveIsX, forMove);
        distanceForMoving *= moveMultiplier;
        var nearestCorner = GetCornerNearestForCenterBySector(whereIntersected, forMove);
        return isAxisForMoveIsX
            ? new Point(nearestCorner.X + distanceForMoving, nearestCorner.Y)
            : new Point(nearestCorner.X, nearestCorner.Y + distanceForMoving);
    }

    private int CalculateMoveMultiplierForMoveFromCenter(bool isAxisForMoveIsX, Rectangle forMove)
    {
        if (forMove.Bottom < Center.Y && forMove.Left > Center.X) return isAxisForMoveIsX ? 1 : -1;
        if (forMove.Bottom < Center.Y && forMove.Right < Center.X) return -1;
        if (forMove.Top > Center.Y && forMove.Left > Center.X) return 1;
        if (forMove.Top > Center.Y && forMove.Right < Center.X) return isAxisForMoveIsX ? -1 : 1;
        return isAxisForMoveIsX ? forMove.Bottom < Center.Y ? -1 : 1
            : forMove.Left > Center.X ? 1 : -1;
    }

    private int CalculateMoveMultiplierForMoveToCenter(bool isAxisForMoveIsX, Rectangle forMove)
    {
        return CalculateMoveMultiplierForMoveFromCenter(isAxisForMoveIsX, forMove) * -1;
    }

    private int CalculateMoveMultiplierForMoveClockwise(bool isAxisForMoveIsX, Rectangle forMove)
    {
        if (forMove.Bottom < Center.Y && forMove.Left > Center.X) return 1;
        if (forMove.Bottom < Center.Y && forMove.Right < Center.X) return isAxisForMoveIsX ? 1 : -1;
        if (forMove.Top > Center.Y && forMove.Left > Center.X) return isAxisForMoveIsX ? -1 : 1;
        if (forMove.Top > Center.Y && forMove.Right < Center.X) return -1;
        return isAxisForMoveIsX ? forMove.Bottom < Center.Y ? 1 : -1
            : forMove.Left > Center.X ? -1 : 1;
    }

    private int CalculateDistanceForMoveClockwiseToPositionWithoutIntersection(
        Sector whereIntersected, Rectangle forInsertion, Rectangle intersected)
    {
        return whereIntersected switch
        {
            Sector.TopRight => Math.Abs(forInsertion.Top - intersected.Bottom),
            Sector.BottomRight => Math.Abs(forInsertion.Right - intersected.Left),
            Sector.BottomLeft => Math.Abs(forInsertion.Bottom - intersected.Top),
            _ => Math.Abs(forInsertion.Left - intersected.Right)
        };
    }

    private static Point GetCornerNearestForCenterBySector(Sector rectangleLocationSector, Rectangle forInsertion)
    {
        return rectangleLocationSector switch
        {
            Sector.TopRight => new Point(forInsertion.Left, forInsertion.Bottom),
            Sector.BottomRight => new Point(forInsertion.Left, forInsertion.Top),
            Sector.BottomLeft => new Point(forInsertion.Right, forInsertion.Top),
            _ => new Point(forInsertion.Right, forInsertion.Bottom)
        };
    }

    private bool IsMovingAxisIsXBySector(Sector forInsertionRectangleSector)
    {
        return forInsertionRectangleSector == Sector.BottomRight || forInsertionRectangleSector == Sector.TopLeft;
    }
}