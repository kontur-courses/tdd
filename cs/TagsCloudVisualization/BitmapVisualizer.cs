﻿using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class BitmapVisualizer
    {
        private readonly Bitmap bmp;
        private readonly Graphics graphics;
        private readonly CircularCloudLayouter layouter;

        public BitmapVisualizer(int width, int height)
        {
            bmp = new Bitmap(width, height);
            graphics = Graphics.FromImage(bmp);
            layouter = new CircularCloudLayouter(new Point(Width / 2, Height / 2));
        }

        public int Width => bmp.Width;
        public int Height => bmp.Height;


        public void GenerateRandomRectangles(int count)
        {
            var rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var width = rnd.Next(50, 60);
                var height = rnd.Next(50, 60);
                layouter.PutNextRectangle(new Size(width, height));
            }
        }

        public void GenerateRectanglesWithSize(int count, Size size)
        {
            for (var i = 0; i < count; i++) layouter.PutNextRectangle(size);
        }

        public void SaveToFile(string fileName)
        {
            bmp.Save(fileName);
        }

        public void DrawRectangles(Color backgroundColor, Color outlineColor)
        {
            var rectangles = layouter.Rectangles.ToArray();
            graphics.Clear(backgroundColor);
            graphics.DrawRectangles(new Pen(outlineColor), rectangles);
        }
    }
}