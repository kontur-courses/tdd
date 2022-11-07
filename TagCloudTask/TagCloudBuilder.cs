using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class TagCloudBuilder
    {
        private int width, height;
        
        // УБРАТЬ
        public Bitmap Bitmap { get; }
        // УБРАТЬ
        public Graphics Graphics { get; }
        
        private Point centerPoint;
        private double spiralAngle = 0;
        private float scale = 0f;
        private int pointsSkip;
        private const float Density = 0.00001f;
        private const float AngleOffset = (float) (Math.PI / (360));
        public List<Rectangle> Rectangles { get; } = new();
        

        public TagCloudBuilder(int width, int height, int pointsSkip=0)
        {
            this.height = height;
            this.width = width;
            centerPoint = new Point(width / 2, height / 2);
            this.pointsSkip = pointsSkip;
            
            // УБРАТЬ
            Bitmap = new Bitmap(width, height);
            Graphics = Graphics.FromImage(Bitmap);
        }

        
        public Point GetNextSpiralPoint()
        {
            var point = new Point(
                (int) (centerPoint.X * (1 + scale * Math.Cos(spiralAngle))),
                (int) (centerPoint.Y * (1 + scale * Math.Sin(spiralAngle)))
            );
            spiralAngle += AngleOffset;
            scale += Density;
            return point;
        }

        public void IterationOfDrawSpiral()
        {
            var p = GetNextSpiralPoint();
            spiralAngle += AngleOffset * pointsSkip;
            scale += Density * pointsSkip;

            //Console.WriteLine($"{p.X} {p.Y}");
            Graphics.DrawLine(new Pen(Brushes.Black, 1f), p, new Point(p.X+1, p.Y+1));
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            var p = GetNextSpiralPoint();
            spiralAngle += AngleOffset * pointsSkip;
            scale += Density * pointsSkip;
            
            var foundRectangle = new Rectangle(p - rectangle.Size/2, rectangle.Size);
            bool isIntersectWithSpiral = Rectangles
                .Any(r => r.IntersectsWith(foundRectangle));
            
            while (isIntersectWithSpiral)
            {
                p = GetNextSpiralPoint();
                Graphics.DrawLine(new Pen(Brushes.IndianRed, 1f), p, new Point(p.X+1, p.Y+1));
                spiralAngle += AngleOffset * pointsSkip;
                scale += Density * pointsSkip;
                
                isIntersectWithSpiral = Rectangles
                    .Any(r => r.IntersectsWith(new Rectangle(
                        p - rectangle.Size/2
                        , rectangle.Size)));
            }
            
            foundRectangle = new Rectangle(p - rectangle.Size/2, rectangle.Size);
            Rectangles.Add(foundRectangle);

            Graphics.DrawRectangle(new Pen(Color.Blue, 1f), foundRectangle);
        }
    }
}