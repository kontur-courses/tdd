﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public class Picture
    {
        private Bitmap pictureBitmap;
        private Graphics graphics;
        private SolidBrush brush = new SolidBrush(Color.Black);

        private Random random = new Random();
        
        public Picture(Size pictureSize)
        {
            pictureBitmap = new Bitmap(pictureSize.Width, pictureSize.Height);
            graphics = Graphics.FromImage(pictureBitmap);
        }

        public void FillRectangle(Rectangle rectangle, Color? color = null)
        {
            SetBrushColor(color);
            graphics.FillRectangle(brush, rectangle);
        }
        
        public void DrawCircle(Point position, float radius, Color? color = null)
        {
            SetBrushColor(color);
            graphics.DrawEllipse(new Pen(brush), 
                position.X - radius, position.Y - radius, 
                2 * radius, 2 * radius);
        }

        public void Save(string path = null, string outputFileName = "output")
        {
            if (path == null)
                path = Directory.GetCurrentDirectory();
            
            pictureBitmap.Save($"{path}/{outputFileName}.bmp");
        }

        private void SetBrushColor(Color? color = null)
        {
            var brushColor = color ?? GetRandomColor();
            brush.Color = brushColor;
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(
                    random.Next(0, 256),
                    random.Next(0, 256),
                    random.Next(0, 256)
                    );
        }
    }
}