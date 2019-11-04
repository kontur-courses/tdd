using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DrawTagsCloud
{
	class Program
	{
        
		static void Main(string[] args)
		{
            var cloud = new TagsCloudVisualization.CircularCloudLayouter(new Point(800, 600));
            var rectangles = new List<Rectangle>();
            
            for (var i = 0; i < 100; i++)
            {
                var rand = new Random();
                var size = new Size(20, 100);
                var rect = cloud.PutNextRectangle(size);
                rectangles.Add(rect);
            }

            var sizeImage = cloud.GetSizeImage();
            var image = new Bitmap(sizeImage.Width, sizeImage.Height);
            var drawImage = Graphics.FromImage(image);
            drawImage.Clear(Color.White);
            foreach (var rect in rectangles)
            {
                var rand = new Random();
                var r = rand.Next(0, 255);
                var g = rand.Next(0, 255);
                var b = rand.Next(0, 255);
                drawImage.FillRectangle(new SolidBrush(Color.FromArgb(255, r, g, b)), rect);
            }
            var enviroment = System.Environment.CurrentDirectory;
            image.Save(Directory.GetParent(Directory.GetParent(enviroment).Parent.FullName) + "\\8000heighstrect.jpg", ImageFormat.Jpeg);
        }
	}
}
