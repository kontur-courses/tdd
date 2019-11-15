using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class SegmentStacker
    {
        public static void StackSegments(List<Segment> addedSegments, HashSet<Segment> borderSegments)
        {
            foreach (var segment in addedSegments)
            {
                if (segment.Horizontal())
                {
                    List<Segment> parallelSegments = borderSegments
                        .Where(a => a.start.Y == segment.start.Y && a.Horizontal()).ToList();
                    parallelSegments.Add(segment);
                    var stacked = RemoveIntersections(parallelSegments, segment.start.Y, segment.type);
                    borderSegments.ExceptWith(parallelSegments);
                    borderSegments.UnionWith(stacked);
                }
                else
                {
                    List<Segment> parallelSegments = borderSegments
                        .Where(a => a.start.X == segment.start.X && !a.Horizontal()).ToList();
                    parallelSegments.Add(segment);
                    var stacked = RemoveIntersections(parallelSegments, segment.start.X, segment.type);
                    borderSegments.ExceptWith(parallelSegments);
                    borderSegments.UnionWith(stacked);
                }
            }
        }


        private static List<Segment> RemoveIntersections(List<Segment> segments, int coord, Segment.Type type)
        {
            List<SegmentPoint> points = new List<SegmentPoint>();
            List<Segment> outSegments = new List<Segment>();
            foreach (var segment in segments)
            {
                if (Segment.Horizontal(type))
                {
                    points.Add(new SegmentPoint(segment.start.X, segment.type, SegmentPoint.PointType.Start));
                    points.Add(new SegmentPoint(segment.end.X, segment.type, SegmentPoint.PointType.End));
                }
                else
                {
                    points.Add(new SegmentPoint(segment.start.Y, segment.type, SegmentPoint.PointType.Start));
                    points.Add(new SegmentPoint(segment.end.Y, segment.type, SegmentPoint.PointType.End));
                }
            }
            Segment.Type currentType = points[0].SegmentType;
            var currentPoint = points[0];
            var startFound = true;
            bool isIntersection = false;
            bool isFirst = true;
            int lastAdded = int.MinValue;
            var sortedPoints = points.OrderBy(a => a.Coord).ToList();
            for (int i = 0; i < sortedPoints.Count - 1; i++)
            {
                if (sortedPoints[i].Coord == sortedPoints[i + 1].Coord
                    && sortedPoints[i].SegmentType == sortedPoints[i + 1].SegmentType)
                {
                    sortedPoints[i].Type = SegmentPoint.PointType.Start;
                    sortedPoints[i + 1].Type = SegmentPoint.PointType.Start;
                    /*удаление из середины листа слишком долгая операция, сделаем точку стартовой
                     * и она не будет рассматриваться в автомате
                     */
                }
            }
            foreach (var point in sortedPoints)
            {
                if (isFirst)
                {
                    currentType = point.SegmentType;
                    currentPoint = point;
                    startFound = true;
                    isFirst = false;
                    continue;
                }
                if (isIntersection)
                {
                    startFound = true;
                    currentPoint = point;
                    currentType = Segment.OppositeType(point.SegmentType);
                    isIntersection = false;
                    continue;
                }
                if (!startFound && point.Type == SegmentPoint.PointType.Start)
                {
                    if (lastAdded == point.Coord && outSegments.Last().type == point.SegmentType)
                    {
                        var lastSegment = outSegments.Last();
                        outSegments.RemoveAt(outSegments.Count - 1);
                        startFound = true;
                        if (Segment.Horizontal(type))
                            currentPoint = new SegmentPoint(lastSegment.start.X, point.SegmentType, SegmentPoint.PointType.Start);
                        else
                            currentPoint = new SegmentPoint(lastSegment.start.Y, point.SegmentType, SegmentPoint.PointType.Start);
                        continue;
                    }
                    currentPoint = point;
                    startFound = true;
                    currentType = point.SegmentType;
                    continue;
                }
                if (startFound && point.SegmentType == currentType && point.Type == SegmentPoint.PointType.End)
                {
                    if (Segment.Horizontal(type))
                        outSegments.Add(new Segment(currentPoint.Coord, coord, point.Coord, coord, currentType));
                    else
                        outSegments.Add(new Segment(coord, currentPoint.Coord, coord, point.Coord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    continue;
                }
                if (startFound && point.SegmentType != currentType)
                {
                    if (Segment.Horizontal(type))
                        outSegments.Add(new Segment(currentPoint.Coord, coord, point.Coord, coord, currentType));
                    else
                        outSegments.Add(new Segment(coord, currentPoint.Coord, coord, point.Coord, currentType));
                    lastAdded = point.Coord;
                    startFound = false;
                    isIntersection = true;
                    continue;
                }
            }
            return outSegments.Where(a => a.Length > 0).ToList();
        }

        private class SegmentPoint
        {
            public int Coord;
            public Segment.Type SegmentType;
            public PointType Type;
            public SegmentPoint(int coord, Segment.Type segmentType, PointType type)
            {
                Coord = coord;
                SegmentType = segmentType;
                Type = type;
            }
            public enum PointType
            {
                Start,
                End
            }
        }
    }
}
