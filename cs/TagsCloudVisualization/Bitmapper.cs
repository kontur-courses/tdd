using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    public class Bitmapper
    {
        private CircularCloudLayouter layouter;
        private Bitmap bitmap;
        private Graphics graphics
        {
            get
            {
                return Graphics.FromImage(bitmap);
            }
        }

        private readonly Pen centralRect = new Pen(Color.White, 4);
        private readonly Pen surroundRects = new Pen(Color.FromArgb(249, 100, 0), 4);
        private readonly Color backGround = Color.FromArgb(0, 34, 43);

        public readonly string parentDirectory;

        public Bitmapper(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            layouter = new CircularCloudLayouter(new Point((int)(width / 2), (int)(height / 2)));
            var workingDir = Environment.CurrentDirectory;
            parentDirectory = Directory.GetParent(workingDir).Parent.FullName;
        }

        private void FillBackground()
        {
            using (var solidBrush = new SolidBrush(backGround))
            {
                graphics.FillRectangle(solidBrush, 0, 0, bitmap.Width, bitmap.Height);
            }
        }

        private void DrawSingleRectangle(Pen pen, Rectangle rect)
        {
            using (graphics)
            {
                graphics.DrawRectangle(pen, rect);
            }
        }

        private void ClearFrame()
        {
            bitmap = new Bitmap(bitmap.Width, bitmap.Height);
            layouter = new CircularCloudLayouter(new Point((int)(bitmap.Width / 2), (int)(bitmap.Height / 2)));
        }

        public void DrawRectangles(List<Rectangle> rectangles, string fileName)
        {
            FillBackground();
            rectangles.Reverse();

            foreach (var rect in rectangles)
            {
                if (rect == rectangles.Last())
                {
                    DrawSingleRectangle(centralRect, rect);
                }
                else
                {
                    DrawSingleRectangle(surroundRects, rect);
                }
            }

            SaveFile(parentDirectory, fileName);
            ClearFrame();
        }

        private void SaveFile(string directory, string name)
        {
            var fullPath = directory + "\\" + name + ".jpg";
            bitmap.Save(fullPath);
        }

        public void DrawDefaultPictures()
        {
            foreach (var set in rectsToDraw)
            {
                var rects = new List<Rectangle>();

                foreach (var size in set)
                {
                    var addedRect = layouter.PutNextRectangle(size);
                    rects.Add(addedRect);
                }

                DrawRectangles(rects, "completePictures" + rectsToDraw.IndexOf(set));
            }
        }

        public List<Rectangle> NormolizeToCenterRects(List<Rectangle> rectsInZeroCoord)
        {
            return rectsInZeroCoord.Select(x => new Rectangle(
                x.X + (int)(bitmap.Width / 2), 
                x.Y + (int)(bitmap.Height / 2), 
                x.Width, 
                x.Height))
                .ToList();
        }

        public readonly List<List<Size>> rectsToDraw = new List<List<Size>>()
        {
            new List<Size>()
            {
                new Size(200, 150),
                new Size(100, 50),
                new Size(125, 70),
                new Size(80, 30),
                new Size(125, 70),
                new Size(100, 50),
                new Size(80, 30),
                new Size(80, 30),
                new Size(100, 50),
                new Size(125, 70),
                new Size(80, 30),
            },
            new List<Size>()
            {
                new Size(300, 150),
                new Size(200, 100),
                new Size(125, 70),
                new Size(175, 30),
                new Size(125, 70),
                new Size(125, 70),
                new Size(80, 30),
                new Size(175, 30),
                new Size(80, 30),
                new Size(125, 70),
                new Size(100, 50),
                new Size(175, 30),
                new Size(80, 30),
                new Size(100, 50),
                new Size(125, 70),
                new Size(175, 30),
                new Size(80, 30),
            },
            new List<Size>()
            {
                new Size(200, 150),
                new Size(100, 50),
                new Size(125, 70),
                new Size(80, 30),
                new Size(125, 70),
                new Size(100, 50),
                new Size(80, 30),
                new Size(80, 30),
                new Size(100, 50),
                new Size(125, 70),
                new Size(80, 30),
            },
            new List<Size>()
            {
                new Size(400, 200),
                new Size(300, 100),
                new Size(125, 70),
                new Size(200, 100),
                new Size(200, 100),
                new Size(175, 30),
                new Size(50, 25),
                new Size(50, 25),
                new Size(50, 25),
                new Size(300, 100),
                new Size(125, 70),
                new Size(125, 70),
                new Size(125, 70),
                new Size(125, 70),
                new Size(125, 70),
                new Size(125, 70),
                new Size(50, 25),
                new Size(80, 30),
                new Size(175, 30),
                new Size(80, 30),
                new Size(125, 70),
                new Size(125, 70),
                new Size(125, 70),
                new Size(100, 50),
                new Size(50, 25),
                new Size(50, 25),
                new Size(50, 25),
                new Size(175, 30),
                new Size(50, 25),
                new Size(80, 30),
                new Size(100, 50),
                new Size(125, 70),
                new Size(175, 30),
                new Size(80, 30),
            }
        };
    }
}
