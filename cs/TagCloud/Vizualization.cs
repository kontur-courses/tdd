using System;
using System.Drawing;
using System.IO;

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
            Console.WriteLine(GetNewPngPath());

            bitmap.Save(GetNewPngPath());
        }

        private static string GetNewPngPath()
        {
            var workingDirectory = Directory.GetCurrentDirectory();
            var index = workingDirectory.IndexOf("TagCloud");
            var tagCloudPath = workingDirectory.Substring(0, index);
            return tagCloudPath + "MyPng" +  DateTime.Now.Millisecond + ".png";
        }

        private static void DrawAndFillRectangle(Graphics graphics, Rectangle rectangle)
        {
            var brushColor = Color.FromArgb(rectangle.X % 255, rectangle.Y % 255, 100);
            var brush = new SolidBrush(brushColor);
            graphics.DrawRectangle(new Pen(Color.Black), rectangle);
            graphics.FillRectangle(brush, rectangle);
        }
    }
}