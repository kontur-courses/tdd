﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<RectangleF> rectangles;
        public IReadOnlyList<RectangleF> Rectangles { get => rectangles; }
        private readonly PointF layouterCenter;
        private readonly Spiral spiral;

        public CircularCloudLayouter(PointF center)
        {
            layouterCenter = center;
            rectangles = new List<RectangleF>();
            spiral = new Spiral(center);
        }

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Size should be valid: " +
                    "width and height must be positive");

            var rectangle = RectangleFExtensions
                .GetRectangleByCenter(rectangleSize, layouterCenter);
            var positionedRectangle = FindPositionToRectangle(rectangle);
            rectangles.Add(positionedRectangle);
        }

        public void Visualise(string filename)
        {
            var bitmap = new Bitmap
                ((int)(layouterCenter.X * 2), (int)(layouterCenter.Y * 2));
            var gr = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.DarkGreen, 1);

            gr.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save(filename);
        }

        private RectangleF GetShiftedRectangle
            (SizeF rectSize, PointF center, Vector2 offset)
            => RectangleFExtensions.GetRectangleByCenter(rectSize, new PointF
                (center.X + offset.X, center.Y + offset.Y));

        private RectangleF FindPositionToRectangle(RectangleF rect)
        {
            while (IsRectangleIntersectedByAnother(rect))
            {
                var rectCenter = spiral.GetNextPointOnSpiral();
                rect = RectangleFExtensions.GetRectangleByCenter(rect.Size, rectCenter);
            }
            return ShiftRectangleToCenter(rect);
        }

        private RectangleF ShiftRectangleToCenter(RectangleF rect)
        {
            var shifted = new RectangleF();
            var rectCenter = rect.GetCenter();
            var normal = GetNormalToCenter(rectCenter);
            if (float.IsNaN(normal.X) || float.IsNaN(normal.Y))
                return rect;
            var k = 1;

            while (!IsRectangleIntersectedByAnother(rect))
            {
                shifted = rect;
                rect = GetShiftedRectangle(rect.Size, rectCenter, normal * k);
                k += 1;
            }
            return shifted;
        }

        private Vector2 GetNormalToCenter(PointF point)
        {
            var direction = new Vector2(layouterCenter.X - point.X,
                layouterCenter.Y - point.Y);
            var length = direction.Length();

            return new Vector2(direction.X / length,
                direction.Y / length);
        }

        private bool IsRectangleIntersectedByAnother(RectangleF rect)
            => rectangles.Any(r => r.IntersectsWith(rect));
    }
}
