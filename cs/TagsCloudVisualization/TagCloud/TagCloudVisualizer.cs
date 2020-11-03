using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.TagCloud
{
    public static class TagCloudVisualizer
    {
        public static void PrintTagCloud(ICloudLayouter cloudLayouter, ImageFormat imageFormat,
            string pathToFile = @"..\..\..\Images\", string fileName = "TagCloud")
        {
            var bitmap = new Bitmap(cloudLayouter.PointGenerator.CanvasSize.Height,
                cloudLayouter.PointGenerator.CanvasSize.Width);
            var graphics = Graphics.FromImage(bitmap);
            var colors = new[]
                {Color.MediumVioletRed, Color.Orange, Color.White, Color.Red, Color.LawnGreen, Color.SpringGreen};

            graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, cloudLayouter.PointGenerator.CanvasSize.Width, cloudLayouter.PointGenerator.CanvasSize.Height));
            for (int i = 0; i < cloudLayouter.AddedRectanglesCount; i++)
                graphics.DrawRectangle(new Pen(colors[i % colors.Length]),
                    cloudLayouter.GetAddedRectangle(i));

            bitmap.Save(pathToFile + fileName + "." + imageFormat.ToString().ToLower(), imageFormat);
        }
    }
}