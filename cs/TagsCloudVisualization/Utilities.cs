using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace TagsCloudVisualization
{
    public static class Utilities
    {
        public static IEnumerable<Size> GenerateRectangleSize(
            int count, int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var rnd = new Random();
            for (var i = 0; i < count; i++)
            {
                var width = rnd.Next(minWidth, maxWidth);
                var height = rnd.Next(minHeight, maxHeight);
                yield return new Size(width, height);
            }
        }

        public static IEnumerable<Size> GenerateRectangleSize(
            int count, Size minSize, Size maxSize)
        {
            return GenerateRectangleSize(
                count, minSize.Width, maxSize.Width, minSize.Height, maxSize.Height);
        }

        public static (int count, Size minSize, Size maxSize, string filename)[] GetTestData()
        {
            return new ValueTuple<int, Size, Size, string>[]
            {
                ValueTuple.Create(25, new Size(30, 30), new Size(50, 50), "image_1.png"),
                ValueTuple.Create(50, new Size(30, 30), new Size(50, 50), "image_2.png"),
                ValueTuple.Create(100, new Size(30, 30), new Size(50, 50), "image_3.png"),
            };
        }

        public static string GetProjectPath()
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory());
            return directoryInfo.Parent.Parent.Parent.FullName;
        }
    }
}