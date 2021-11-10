using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Polygon
    {
        public List<PointF> Vertexes;

        public Polygon(RectangleF rectangle)
        {
            Vertexes = rectangle.GetPoints().ToList();
        }

        public Polygon(IEnumerable<PointF> vertexes)
        {
            Vertexes = vertexes.ToList();
        }

        public IEnumerable<(PointF, PointF)> GetSegments()
        {
            for (var i = 0; i < Vertexes.Count - 1; i++)
            {
                yield return (Vertexes[i], Vertexes[i + 1]);
            }
            yield return (Vertexes[^1], Vertexes[0]);
        }

        public void Normalize()
        {
            var vertexes = new List<PointF>() { Vertexes[0] };
            var prev = Vertexes[0];
            foreach (var vertex in Vertexes.Skip(1))
            {
                if (vertex != prev)
                {
                    prev = vertex;
                    vertexes.Add(vertex);
                }
            }
            var excessivePoints = new List<PointF>();
            
            for (var i = 1; i < vertexes.Count - 1; i++)
            {
                if (IsOnSameLine(vertexes[i - 1], vertexes[i],vertexes[i + 1]))
                {
                    excessivePoints.Add(vertexes[i]);
                }
            }
            if (IsOnSameLine(vertexes[^2], vertexes[^1], vertexes[0]))
                excessivePoints.Add(vertexes[^1]);
            if (IsOnSameLine(vertexes[^1], vertexes[0], vertexes[1]))
                excessivePoints.Add(vertexes[0]);
            vertexes.RemoveAll(p => excessivePoints.Contains(p));
            Vertexes = vertexes;
        }

        private bool IsOnSameLine(PointF first, PointF middle, PointF end)
        {
            if (first.Y == middle.Y && middle.Y == end.Y)
                return true;
            return first.X == middle.X && middle.X == end.X;
        }
    }
}