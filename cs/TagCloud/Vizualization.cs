using System;
using System.Drawing;

namespace TagCloud
{
    public static class Vizualizator
    {
        public static void Main()
        {
            Draw();
        }
        
        public static void Draw()
        {
            var bitmap = new Bitmap(700, 800);
            var graphics = Graphics.FromImage(bitmap);
            
            var layouter = new CircularCloudLayouter(new Point(350, 400));
            var width = 200;
            var height = 100;
            for (var i = 0; i < 50; i++)
            {
                var rectangle = layouter.PutNextRectangle(new Size(width, height));
                DrawAndFillRectangle(graphics, rectangle);
                
                width = (int)Math.Round(width/1.1);
                height = (int)Math.Round(height/1.1);
            }

            bitmap.Save("C:\\Users\\Наталья\\Desktop\\myPng.bmp");
        }

        public static void DrawAndFillRectangle(Graphics graphics, Rectangle rectangle)
        {
            var brushColor = Color.FromArgb(rectangle.X % 255, rectangle.Y % 255, 100);
            var brush = new SolidBrush(brushColor);
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            graphics.FillRectangle(brush, rectangle);
        }
    }
}