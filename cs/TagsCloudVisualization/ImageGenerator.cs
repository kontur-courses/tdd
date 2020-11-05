using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class ImageGenerator
    {
        public void GenerateImage(IEnumerable<Size> rectangleSizes, string outputFile)
        {
            var layouter = new CircularCloudLayouter(new Point(400, 400));
            var rectangles = rectangleSizes.Select(s => layouter.PutNextRectangle(s)).ToArray();
            
            var bitmap = new Bitmap(800, 800);
            var graphics = Graphics.FromImage(bitmap);
            var currentColor = Color.FromArgb(0, 255, 0);
            for(var i = 0; i < rectangles.Length; i++)
            {
                graphics.FillRectangle(new SolidBrush(currentColor), rectangles[i]);
                currentColor = Color.FromArgb(
                    currentColor.R + 255 / rectangles.Length,
                    currentColor.G - 255 / rectangles.Length,
                    currentColor.B);
                graphics.DrawRectangle(Pens.Black, rectangles[i]);
                var font = new Font("Arial", 24, GraphicsUnit.Pixel);
                graphics.DrawString($"{i}", font, Brushes.Black, rectangles[i]);
            }
            
            bitmap.Save(outputFile);
        }

        public Size[][] Images = {
            new[]
            {
                new Size(25, 30),
                new Size(30, 40),
                new Size(50, 60),
                new Size(70, 80),
            },
            new[]
            {
                new Size(25, 160),
                new Size(30, 150),
                new Size(30, 140),
                new Size(40, 130),
                new Size(50, 120),
                new Size(60, 110),
                new Size(70, 100),
                new Size(80, 90),
                new Size(90, 80),
                new Size(100, 70),
                new Size(110, 60),
                new Size(120, 50),
                new Size(130, 40),
                new Size(140, 30),
                new Size(150, 25),
                new Size(160, 25),
                new Size(160, 25),
                new Size(160, 25),
            },
            new []
            {
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
                new Size(40, 40),
            },
            new []
            {
                new Size(20, 40),
                new Size(25, 40),
                new Size(30, 40),
                new Size(35, 40),
                new Size(40, 40),
                new Size(45, 40),
                new Size(50, 40),
                new Size(55, 40),
                new Size(60, 40),
                new Size(65, 40),
                new Size(70, 40),
                new Size(75, 40),
                new Size(80, 40),
                new Size(85, 40),
                new Size(90, 40),
                new Size(95, 40),
                new Size(100, 40),
                new Size(105, 40),
                new Size(110, 40),
                new Size(115, 40),
                new Size(120, 40),
                new Size(125, 40),
                new Size(130, 40),
                new Size(135, 40),
                new Size(140, 40),
                new Size(145, 40),
                
            }
        };
        
        [Test]
        public void GenerateTestImages()
        {
            var directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory)
                .Parent.Parent.CreateSubdirectory("Images");
            for (var i = 0; i < Images.Length; i++)
                GenerateImage(Images[i], $"{directory.FullName}/Image{i}.bmp");
        }
    }
}