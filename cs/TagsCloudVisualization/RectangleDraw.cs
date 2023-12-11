﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization;

public class DrawCloud
{
    private Bitmap _bitmap;
    private readonly Size shiftToBitmapCenter;
    private List<Rectangle> _rectangles;
    public DrawCloud(int width, int height)
    {
        _bitmap = new Bitmap(width, height);
        shiftToBitmapCenter = new Size(_bitmap.Width / 2, _bitmap.Height / 2);
    }
    public void CreateImage(List<Rectangle> rectangles, string filename)
    {
        _rectangles = rectangles;
        using (Graphics g = Graphics.FromImage(_bitmap))
        {
            g.Clear(Color.White);
            foreach (var r in _rectangles)
            {
                var rectangle =  new Rectangle(r.Location + shiftToBitmapCenter, r.Size);
                g.DrawRectangle(new Pen(Color.BlueViolet), rectangle);
            }
        }
        _bitmap.Save(filename, ImageFormat.Png);
    }
}