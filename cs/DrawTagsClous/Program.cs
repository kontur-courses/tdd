using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace DrawTagsCloud
{
	class Program
	{
        
		static void Main(string[] args)
		{
            var cloud = new TagsCloudVisualization.TagsCloudVisualization(new Point(3000 / 2, 3000 / 2));
            var image = new Bitmap(3000, 3000);
            var drawImage = Graphics.FromImage(image);
            drawImage.Clear(Color.White);
            
            for (var i = 0; i < 1000; i++)
            {
                var rand = new Random();
                var size = new Size(100, 100);
                var rect = cloud.PutNextRectangle(size);
                var r = rand.Next(0, 255);
                var g = rand.Next(0, 255);
                var b = rand.Next(0, 255);
                drawImage.FillRectangle(new SolidBrush(Color.FromArgb(255, r, g, b)), rect);
            }
            image.Save("test.jpg");
        }
	}
}
