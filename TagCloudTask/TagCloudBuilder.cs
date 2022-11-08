using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class TagCloudBuilder : ITagCloudEngine
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
        

        public TagCloudBuilder(Size size, int pointsSkip=0)
        {
            this.height = size.Height;
            this.width = size.Width;
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

        // for testing spiral 
        // public void IterationOfDrawSpiral()
        // {
        //     var p = GetNextSpiralPoint();
        //     spiralAngle += AngleOffset * pointsSkip;
        //     scale += Density * pointsSkip;
        //
        //     //Console.WriteLine($"{p.X} {p.Y}");
        //     Graphics.DrawLine(new Pen(Brushes.Black, 1f), p, new Point(p.X+1, p.Y+1));
        // }

        // public void DrawRectangle(Rectangle rectangle)
        // {
        //     var p = GetNextSpiralPoint();
        //     spiralAngle += AngleOffset * pointsSkip;
        //     scale += Density * pointsSkip;
        //     
        //     var foundRectangle = new Rectangle(p - rectangle.Size / 2, rectangle.Size);
        //     bool isIntersectWithSpiral = Rectangles
        //         .Any(r => r.IntersectsWith(foundRectangle));
        //     
        //     while (isIntersectWithSpiral)
        //     {
        //         p = GetNextSpiralPoint();
        //         Graphics.DrawLine(new Pen(Brushes.IndianRed, 1f), p, new Point(p.X+1, p.Y+1));
        //         spiralAngle += AngleOffset * pointsSkip;
        //         scale += Density * pointsSkip;
        //         
        //         isIntersectWithSpiral = Rectangles
        //             .Any(r => r.IntersectsWith(new Rectangle(
        //                 p - rectangle.Size/2
        //                 , rectangle.Size)));
        //     }
        //     
        //     foundRectangle = new Rectangle(p - rectangle.Size/2, rectangle.Size);
        //     Rectangles.Add(foundRectangle);
        //
        //     Graphics.DrawRectangle(new Pen(Color.Blue, 1f), foundRectangle);
        // }

        public Rectangle GetNextRectangle(Size sizeOfRectangle)
        {
            if (sizeOfRectangle.Height <= 0
                || sizeOfRectangle.Width <= 0)
                throw new ArgumentException();
            
            // получаем новую точку на спирали
            // TODO вынести в отдельный метод
            var currentPoint = GetNextSpiralPoint();
            spiralAngle += AngleOffset * pointsSkip;
            scale += Density * pointsSkip;
            
            // проверим, есть ли пересечение прямоугольника, построенного
            // на этой точке, с другими прямоугольниками
            var foundRectangle = new Rectangle(currentPoint - sizeOfRectangle / 2, sizeOfRectangle);
            var isIntersectWithSpiral = Rectangles
                .Any(r => r.IntersectsWith(foundRectangle));
            
            // если есть, то будем получать до тех пор, пока
            // не найдем подходящую точку
            while (isIntersectWithSpiral)
            {
                // TODO вынести в отдельный метод
                currentPoint = GetNextSpiralPoint();
                spiralAngle += AngleOffset * pointsSkip;
                scale += Density * pointsSkip;
                
                isIntersectWithSpiral = Rectangles
                    .Any(r => r.IntersectsWith(new Rectangle(
                        currentPoint - sizeOfRectangle/2
                        , sizeOfRectangle)));
            }
            
            foundRectangle = new Rectangle(currentPoint - sizeOfRectangle / 2, sizeOfRectangle);
            Rectangles.Add(foundRectangle);
            return foundRectangle;
        }
    }
}