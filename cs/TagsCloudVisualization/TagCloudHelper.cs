﻿using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagCloudHelper
    {
        public static Bitmap DrawTagCloud(List<Rectangle> tags, int width, int height)
        {
            var bitmap = new Bitmap(width, height);
            var gp = Graphics.FromImage(bitmap);
            
            gp.FillRectangle(new SolidBrush(Color.FromArgb(255, 0, 34, 43)), new Rectangle(0,0, width, height));
            gp.DrawRectangles(new Pen(Color.FromArgb(255, 217,92,6)), tags.ToArray());

            return bitmap;
        }

        public static List<Size> GenerateRandomListSize(int amount)
        {
            var rnd = new Random();
            var listSize = new List<Size>();
            
            for (var i = 0; i < amount; i++)
                listSize.Add(new Size(rnd.Next(60, 100), rnd.Next(30, 85)));

            return listSize;
        }
    }
}