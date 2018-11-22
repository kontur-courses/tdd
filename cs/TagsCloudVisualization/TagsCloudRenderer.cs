using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class TagsCloudRenderer
    {
        private readonly Size boundary = new Size(100, 100);

        public void RenderIntoFile(string filePath, ITagsCloud tagsCloud)
        {
            var shiftedRectangles = ShiftRectanglesToMainQuarter(tagsCloud.AddedRectangles);
            var tagCloudToDraw = new TagsCloud(tagsCloud.Center, shiftedRectangles);
            var pictureSize = GetPictureSize(tagCloudToDraw);
            var btm = new Bitmap(pictureSize.Width, pictureSize.Height);
            var obj = Graphics.FromImage(btm);
            foreach (var r in tagCloudToDraw.AddedRectangles)
            {
                obj.DrawRectangle(new Pen(Color.Brown), r);
            }

            btm.Save(filePath);
        }

        public void RenderIntoFile(string filePath, ITagsCloud tagsCloud, Size pictureSize)
        {
            var btm = new Bitmap(pictureSize.Width, pictureSize.Height);
            var obj = Graphics.FromImage(btm);
            foreach (var r in tagsCloud.AddedRectangles)
            {
                obj.DrawRectangle(new Pen(Color.Brown), r);
            }

            btm.Save(filePath);
        }

        private Size GetPictureSize(ITagsCloud tagsCloud)
        {
            var rectangles = tagsCloud.AddedRectangles;
            var maxX = rectangles.Max(x => x.Right);
            var minX = rectangles.Min(x => x.X);
            var maxY = rectangles.Max(x => x.Top);
            var minY = rectangles.Min(x => x.Bottom);
            if (minY < 0 || minX < 0)
            {
                throw new ArgumentException("Rectangles must have positive coordinates");
            }

            return new Size(maxX - minX + Math.Abs(minX * 2),
                maxY - minY + Math.Abs(minY * 2));
        }

        private List<Rectangle> ShiftRectanglesToMainQuarter(List<Rectangle> rectangles)
        {
            var minX = rectangles.Min(x => x.X);
            var minY = rectangles.Min(x => x.Bottom);
            var shiftX = 0;
            var shiftY = 0;
            if (minX < 0)
                shiftX = minX * -1 + boundary.Width;
            if (minY < 0)
                shiftY = minY * -1 + boundary.Height;
            return rectangles.Select(x => new Rectangle(x.X + shiftX, x.Y + shiftY, x.Width, x.Height)).ToList();
        }
    }
}