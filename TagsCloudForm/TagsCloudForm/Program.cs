using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using TagsCloudVisualization;
using System.IO;
using System.Drawing.Imaging;

namespace TagsCloudForm
{
    class Program : Form
    {
        private const bool testMode = false;
        private const bool savePicture = false;

        CircularCloudLayouter layouter = new CircularCloudLayouter(new Point(500, 500));
        protected override void OnPaint(PaintEventArgs e)
        {
            int minSize = 20;
            int maxSize = 100;
            Random rnd = new Random();
            var graphics = e.Graphics;
            bool exitFlag = false;
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < 50; i++)
            {
                if (exitFlag)
                    break;
                var rect = layouter.PutNextRectangle(new Size(rnd.Next(minSize, maxSize), rnd.Next(minSize, maxSize)));
                graphics.FillRectangle(new SolidBrush(Color.LightGreen), rect);
                graphics.DrawRectangle(new Pen(Color.Black, 2), rect);
                rectangles.Add(rect);
                if (testMode)
                {
                    foreach (var rectangle in rectangles)
                        if (rect.IntersectsWith(rectangle) && rect != rectangle)
                        {
                            exitFlag = true;
                            WriteLog(rectangles);
                        }
                }
                Thread.Sleep(100);
            }
            if (savePicture)
                SavePicture(rectangles);

        }

        private static void WriteLog(List<Rectangle> rectangles)
        {
            string fileName = @"..\..\testLog.txt";
            if (File.Exists(fileName) != true)
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                {
                    foreach (var rect in rectangles)
                    {
                        sw.WriteLine(rect.ToString());
                    }
                    sw.WriteLine("");
                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(new FileStream(fileName, FileMode.Open, FileAccess.Write)))
                {
                    foreach (var rect in rectangles)
                    {
                        sw.WriteLine(rect.ToString());
                    }
                    sw.WriteLine("");
                }
            }
        }

        private void SavePicture(List<Rectangle> rectangles)
        {
            string pictureName = @"..\..\picture.png";
            var bitmap = new Bitmap(Convert.ToInt32(1024), Convert.ToInt32(1024), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rect in rectangles)
            {
                graphics.FillRectangle(new SolidBrush(Color.LightGreen), rect);
                graphics.DrawRectangle(new Pen(Color.Black, 2), rect);
            }
            bitmap.Save(pictureName, ImageFormat.Png);
        }

        public static void Main()
        {
            Application.Run(new Program { ClientSize = new Size(1000, 1000) });
        }
    }
}
