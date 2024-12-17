using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class BruteForceNearestFinder
    {
        public static Rectangle? FindNearestByDirection(Rectangle r, Direction direction, List<Rectangle> rectangles)
        {
            if (rectangles.FirstOrDefault() == default)
                return null;
            var nearestByDirection = rectangles
                .Select(possibleNearest => 
                (Distance: CalculateMinDistanceBy(direction, possibleNearest, r), Nearest: possibleNearest ))
                .Where(el => el.Distance >= 0)
                .ToList();

            return nearestByDirection.Count > 0 ? nearestByDirection.MinBy(el => el.Distance).Nearest : null;
        }


        public static int CalculateMinDistanceBy(Direction direction, 
            Rectangle possibleNearest, Rectangle rectangleForFind)
        {
            return direction switch
            {
                Direction.Left => rectangleForFind.Left - possibleNearest.Right,
                Direction.Right => possibleNearest.Left - rectangleForFind.Right,
                Direction.Top => rectangleForFind.Top - possibleNearest.Bottom,
                Direction.Bottom => possibleNearest.Top - rectangleForFind.Bottom,
            };
        }
    }
}
