﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer
    {
        private const int IndentFromRectangles = 200;

        private readonly Rectangle[] rectangles;

        private Rectangle rectanglesContainer;


        public BitmapVisualizer(Rectangle[] rectangles)
        {
            if (rectangles == null) throw new ArgumentNullException(nameof(rectangles));
            if (rectangles.Length == 0) throw new ArgumentException("rectangles should contain at least 1 rectangle", nameof(rectangles));
            this.rectangles = rectangles.ToArray();
            rectanglesContainer = rectangles.GetRectanglesContainer();
        }


        private Size GetOptimalBitmapSize()
        {
            return new Size(rectanglesContainer.Width + IndentFromRectangles, rectanglesContainer.Height + IndentFromRectangles);
        }

        private void OffsetRectanglesToCenter(Bitmap bmp)
        {
            var containerCenterX = (rectanglesContainer.Left + rectanglesContainer.Right) / 2;
            var containerCenterY = (rectanglesContainer.Top + rectanglesContainer.Bottom) / 2;
            var bmpCenter = new Point(bmp.Width / 2, bmp.Height / 2);
            var offset = new Point(bmpCenter.X - containerCenterX, bmpCenter.Y - containerCenterY);
            for (var i = 0; i < rectangles.Length; i++) rectangles[i].Offset(offset);
        }

        public void Save(string fileName, double scale, Color backgroundColor, Color outlineColor, DirectoryInfo dir = null)
        {
            dir = dir ?? new DirectoryInfo(Environment.CurrentDirectory);
            if (!dir.Exists) dir.Create();
            var path = Path.Combine(dir.FullName, fileName);
            using (var bmpToSave = DrawRectangles(backgroundColor, outlineColor, scale))
            {
                SaveToPath(bmpToSave, path);
            }

        }

        private static void SaveToPath(Bitmap bmp, string path)
        {
            try
            {
                bmp.Save(path);
            }
            catch (Exception ex)
            {
                throw new Exception($"Can't save file to: {path}", ex);
            }
        }

        public Bitmap DrawRectangles(Color backgroundColor, Color outlineColor, double scale)
        {
            var initialSize = GetOptimalBitmapSize();
            using (var bmp = new Bitmap(initialSize.Width, initialSize.Height))
            {
                OffsetRectanglesToCenter(bmp);
                using (var graphics = Graphics.FromImage(bmp))
                {
                    graphics.Clear(backgroundColor);
                    using (var pen = new Pen(outlineColor))
                    {
                        graphics.DrawRectangles(pen, rectangles);
                    }
                }

                var scaledSize = new Size((int)(initialSize.Width * scale), (int)(initialSize.Height * scale));
                return new Bitmap(bmp, scaledSize);
            }
        }
    }
}