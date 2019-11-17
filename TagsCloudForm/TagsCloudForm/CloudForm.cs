using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using TagsCloudVisualization;

namespace TagsCloudForm
{
    class CloudForm : Form
    {
        private const bool savePicture = false;
        private readonly int  RectanglesNum;
        private Point CloudCenter;
        private readonly int MinRectSize;
        private readonly int MaxRectSize;
        private readonly CircularCloudLayouter Layouter;


        public CloudForm()
        {
            InitializeComponent();
            CloudCenter = new Point(
                (int)Math.Floor(Size.Width / (double)2),
                (int)Math.Floor(Size.Height / (double)2));
            Layouter = new CircularCloudLayouter(CloudCenter);
            RectanglesNum = 10;
        }

        public CloudForm(int rectanglesNum, int minRectSize, int maxRectSize)
        {
            InitializeComponent();
            CloudCenter = new Point(
                (int)Math.Floor(Size.Width / (double)2),
                (int)Math.Floor(Size.Height / (double)2));
            Layouter = new CircularCloudLayouter(CloudCenter);
            RectanglesNum = rectanglesNum;
            MinRectSize = minRectSize;
            MaxRectSize = maxRectSize;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Random rnd = new Random();
            var graphics = e.Graphics;
            var rectangles = new List<Rectangle>();
            for (int i = 0; i < RectanglesNum; i++)
            {
                var rect = Layouter.PutNextRectangle(new Size(rnd.Next(MinRectSize, MaxRectSize), rnd.Next(MinRectSize, MaxRectSize)));
                graphics.FillRectangle(new SolidBrush(Color.LightGreen), rect);
                graphics.DrawRectangle(new Pen(Color.Black, 2), rect);
                rectangles.Add(rect);
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CloudForm
            // 
            this.ClientSize = new System.Drawing.Size(584, 561);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "CloudForm";
            this.ResumeLayout(false);

        }
    }
}
