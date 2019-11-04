using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class LayoutItem
    {
        public string Title;
        public Rectangle Rectangle;

        public LayoutItem() { }
        public LayoutItem(string title, Rectangle rectangle)
        {
            Title = title;
            Rectangle = rectangle;
        }
    }

    public class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<LayoutItem> Items;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Items = new List<LayoutItem>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newItem = new LayoutItem(Items.Count.ToString(), new Rectangle(default, rectangleSize));
            Items.Add(newItem);
            ReallocRectangles();
            return newItem.Rectangle;
        }

        public void PutRectangles(IEnumerable<Size> sizes)
        {
            foreach (var size in sizes)
            {
                var newItem = new LayoutItem(Items.Count.ToString(), new Rectangle(default, size));
                Items.Add(newItem);
            }
            ReallocRectangles();
        }

        private void ReallocRectangles()
        {
            if (Items.Count == 0) return;

            //сортируем прямоугольники по уменьшению площади
            Items.Sort((i1, i2) =>
            {
                var s1 = i1.Rectangle.Width * i1.Rectangle.Height;
                var s2 = i2.Rectangle.Width * i2.Rectangle.Height;
                return s2.CompareTo(s1);
            });

            //самый большой ставим в центре
            var biggestItem = Items[0];
            biggestItem.Rectangle.X = -biggestItem.Rectangle.Width / 2;
            biggestItem.Rectangle.Y = -biggestItem.Rectangle.Height / 2;
            Items[0] = biggestItem;

            //выбираем лучшее место куда поставить каждый последующий
            //лучшее место - когда расстояние от центра до дальней вершины прямоугольника минимально
            for (int i = 1; i < Items.Count; i++)
            {
                var size = Items[i].Rectangle.Size;
                List<KeyValuePair<Rectangle, double>> variants = new List<KeyValuePair<Rectangle, double>>();
                //рассматриваем разные направления от центра, шаг угла 10 градусов
                for (double angle = new Random().NextDouble() * Math.PI / 18; angle < 1.99 * Math.PI; angle += (Math.PI / 18))
                {
                    //пытаемся поставить на этом направлении за самым дальним прямоугольником
                    Point farthestPoint = Utils.GetFarthestRectanglePointIntersectedByRay(this, i, angle);
                    var r = Utils.GetRayLengthFromCenter(Items[i].Rectangle, angle);
                    //подбираем расстояние, чтобы не пересекался с другими
                    const int step = 2;
                    double dist = Math.Sqrt(farthestPoint.X * farthestPoint.X + farthestPoint.Y * farthestPoint.Y) + r;
                    Point location;
                    Rectangle newRect;
                    bool isIntersects;
                    do
                    {
                        dist += step;
                        location = Utils.GetPointByAngleAndDistance(angle, dist);
                        location.Offset(-size.Width / 2, -size.Height / 2);
                        newRect = new Rectangle(location, size);
                        isIntersects = false;
                        for (int j = 0; j < i && !isIntersects; j++)
                        {
                            isIntersects |= newRect.IntersectsWith(Items[j].Rectangle);
                        }                       
                    } while (isIntersects);

                    Utils.GetFathestPointFromCenter(newRect, out double vertexDist);
                    variants.Add(new KeyValuePair<Rectangle, double>(newRect, vertexDist));                    
                }

                //вибираем ближайший к центру вариант
                var minVertexDist = variants.Min(kv => kv.Value);
                Rectangle bestRect = variants.First(kv => kv.Value == minVertexDist).Key;

                Items[i].Rectangle = bestRect;
            }
        }

        public void SaveToFile(string filename)
        {
            if (Items.Count == 0)
                throw new ArgumentException("There are no items.");

            var left = int.MaxValue;
            var right = int.MinValue;
            var top = int.MaxValue;
            var bottom = int.MinValue;
            foreach (var item in Items)
            {
                left = Math.Min(left, item.Rectangle.Left);
                right = Math.Max(right, item.Rectangle.Right);
                top = Math.Min(top, item.Rectangle.Top);
                bottom = Math.Max(bottom, item.Rectangle.Bottom);
            }

            var bmp = new Bitmap(right - left, bottom - top);
            var gr = Graphics.FromImage(bmp);
            gr.Clear(Color.RosyBrown);
            Brush br = new SolidBrush(Color.Green);
            Pen pen = new Pen(Color.Black);
            Brush textBrush = new SolidBrush(Color.Black);
            foreach (var item in Items)
            {
                gr.FillRectangle(br, item.Rectangle.X - left, item.Rectangle.Y - top, item.Rectangle.Width, item.Rectangle.Height);
                gr.DrawRectangle(pen, item.Rectangle.X - left, item.Rectangle.Y - top, item.Rectangle.Width, item.Rectangle.Height);
                gr.DrawString(item.Title, new Font("Tahoma", item.Rectangle.Height, GraphicsUnit.Pixel), textBrush, item.Rectangle.X - left, item.Rectangle.Y - top - 1);
            }
            bmp.Save(filename);
        }
    }
}
