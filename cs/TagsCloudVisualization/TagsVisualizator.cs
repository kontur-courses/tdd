using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public class TagsVisualizator
    {
        private string Root { get; }
        private int RightBorder { get; set; }
        private int BottomBorder { get; set; }

        public TagsVisualizator()
        {
            Root = new DirectoryInfo("..\\..\\..\\Sampels").FullName;
            RightBorder = 0;
            BottomBorder = 0;
        }

        public void SaveVisualization(List<Rectangle> rectangles, string savePath)
        {
            rectangles.ForEach(UpdateImageBorders);

            var pen = new Pen(Color.MediumVioletRed, 4);
            using var bitmap = new Bitmap(RightBorder + (int)pen.Width, BottomBorder + (int)pen.Width);

            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawRectangles(pen, rectangles.ToArray());

            if (!Directory.Exists(savePath))
            {
                throw new ArgumentException("directory doesn't exist");
            }

            bitmap.Save(savePath, ImageFormat.Jpeg);
        }

        private void UpdateImageBorders(Rectangle rectangle)
        {
            if (rectangle.Left < 0 || rectangle.Top < 0)
            {
                throw new ArgumentException("rectangle out of image boundaries");
            }

            if (RightBorder < rectangle.Right)
            {
                RightBorder = rectangle.Right;
            }

            if (BottomBorder < rectangle.Bottom)
            {
                BottomBorder = rectangle.Bottom;
            }
        }

        public string GenerateNewFilePath()
        {
            var dateTime = DateTime.Now;
            return $"{Root}\\{dateTime:MMddyy-HHmmssffffff}.jpg";
        }
    }
}