﻿using System.Drawing;
using System.Drawing.Drawing2D;

namespace TagsCloudVisualization;

public static class CloudDrawer
{
    public static Bitmap DrawCloud(Cloud cloud, int imageWidth, int imageHeight)
    {
        if (cloud.Rectangles.Count == 0)
            throw new ArgumentException("rectangles are empty");
        if (imageWidth <= 0 || imageHeight <= 0)
            throw new ArgumentException("either width or height of rectangle size is not positive");

        var drawingScale = CalculateObjectDrawingScale(cloud.GetWidth(), cloud.GetHeight(), imageWidth, imageHeight);
        var bitmap = new Bitmap(imageWidth, imageHeight);
        var graphics = Graphics.FromImage(bitmap);
        var pen = new Pen(Color.Black);

        graphics.TranslateTransform(-cloud.Center.X, -cloud.Center.Y);
        graphics.ScaleTransform(drawingScale, drawingScale, MatrixOrder.Append);
        graphics.TranslateTransform(cloud.Center.X, cloud.Center.Y, MatrixOrder.Append);
        graphics.Clear(Color.White);
        graphics.DrawRectangles(pen, cloud.Rectangles.ToArray());

        return bitmap;
    }

    public static float CalculateObjectDrawingScale(float width, float height, float imageWidth, float imageHeight)
    {
        var scale = 1f;
        var scaleAccuracy = 0.03f;
        var widthScale = scale;
        var heightScale = scale;
        if (width * scale > imageWidth)
            widthScale = imageWidth / width - scaleAccuracy;
        if (height * scale > imageHeight)
            heightScale = imageHeight / height - scaleAccuracy;
        return Math.Max(widthScale, heightScale);
    }
}