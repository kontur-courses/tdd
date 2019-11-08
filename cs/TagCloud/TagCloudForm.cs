using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TagCloud
{
    public partial class TagCloudForm : Form
    {
        private Graphics graphics;
        private PictureBox pictureBox;
        private readonly Random random = new Random();
        private readonly CircularCloudLayouter layouter;

        private readonly Brush[] colors =
        {
            Brushes.Aqua, Brushes.Lime, Brushes.Blue, Brushes.Brown, Brushes.Chartreuse,
            Brushes.Chocolate, Brushes.Coral, Brushes.Crimson, Brushes.MediumSlateBlue,
            Brushes.Gold, Brushes.Green, Brushes.Fuchsia, Brushes.BlueViolet
        };

        private readonly string[] phrases =
            {"поставь 2 балла", "почему 1 балл", " поставь макс балл", "Наставник", "поставь баллы", "ну пожалуйста"};

        private TagCloudForm()
        {
            InitializeComponent();
            Paint += TagCloudForm_Paint;
            layouter = new CircularCloudLayouter(new Point(0, 0));
        }

        private void TagCloudForm_Paint(object sender, PaintEventArgs e)
        {
            SetPictureBox();
            SetGraphics(e);
            for (var i = 0; i < 150; i++)
                PaintRectangleOnCanvas();
            SaveImage();
            Close();
        }

        private void SetPictureBox()
        {
            pictureBox = new PictureBox
            {
                Size = new Size(1366, 768),
                Image = new Bitmap(1366, 768)
            };
        }

        private void SetGraphics(PaintEventArgs e)
        {
            graphics = Graphics.FromImage(pictureBox.Image);
            graphics.TranslateTransform(683, 384);
        }

        private void PaintRectangleOnCanvas()
        {
            var parameter = random.Next(35, 120);
            var rect = layouter.PutNextRectangle(
                new Size(parameter, (int) (parameter / 2.5)));
            graphics.FillRectangle(colors[random.Next(0, colors.Length)], rect);

            graphics.DrawString(phrases[random.Next(0, phrases.Length)], new Font("Arial", parameter / 8),
                Brushes.Black,
                rect);
        }

        private void SaveImage()
        {
            pictureBox.Image.Save("result.png", ImageFormat.Png);
        }

        public static void Main()
        {
            Application.Run(new TagCloudForm());
        }
    }
}