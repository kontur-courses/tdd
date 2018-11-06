using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    public partial class TagsCloudForm : Form
    {
        private readonly Random random = new Random();
        private readonly Point centerPoint = new Point(250, 250);
        private readonly List<Color> colors = new List<Color> { Color.Black, Color.Blue, Color.Green, Color.Brown, Color.BurlyWood, Color.DarkOrange };


        public TagsCloudForm()
        {
            var baseWords = new List<string> { "Cloud", "Simple" };

            SetBaseSetting();
            AddButtons();
            UpdateDisplay(baseWords);
            InitializeComponent();

            Text = "Tags cloud";
        }

        private void SetBaseSetting()
        {
            BackColor = Color.DeepSkyBlue;
            Size = new Size(650, 600);
            MinimumSize = Size;
            MaximumSize = Size;
        }

        private void AddButtons()
        {
            var sizeButton = new Size(90, 50);

            var loadButton = new Button();
            loadButton.Size = sizeButton;
            loadButton.BackColor = Color.White;
            loadButton.Location = new Point(Size.Width - 150, Size.Height - 100);
            loadButton.Text = "Load";
            loadButton.Click += new EventHandler(LoadTags);

            var saveButton = new Button();
            saveButton.Size = sizeButton;
            saveButton.BackColor = Color.White;
            saveButton.Location = new Point(Size.Width - 250, Size.Height - 100);
            saveButton.Text = "Save";
            saveButton.Click += new EventHandler(SaveCloud);

            Controls.Add(loadButton);
            Controls.Add(saveButton);
        }

        private void AddWordLabels(IEnumerable<String> words)
        {
            var generatorCirclePoints = new EternityGeneratorCirclePoints(centerPoint);
            var cloudLayouter = new CircularCloudLayouter(generatorCirclePoints);
            var fontSize = 35;

            foreach (var word in words)
            {
                var wordLabel = new Label();
                wordLabel.Font = new Font("Arial", fontSize, FontStyle.Bold);
                wordLabel.Text = word;
                var size = TextRenderer.MeasureText(word, wordLabel.Font);
                var rectangleWords = cloudLayouter.PutNextRectangle(size);
                wordLabel.Location = rectangleWords.Location;
                wordLabel.Size = size;
                wordLabel.ForeColor = GetRandomColor();
                Controls.Add(wordLabel);
                fontSize = GetNextFontSize(fontSize);
            }
        }

        private Color GetRandomColor()
            => colors[random.Next(0, colors.Count)];


        private int GetNextFontSize(int fontSize)
            => Math.Max(10, fontSize * 19 / 20);

        private void UpdateDisplay(IEnumerable<String> words)
        {
            RemoveLabels();
            AddWordLabels(words);
        }

        private void RemoveLabels()
        {
            foreach (var control in Controls.OfType<Label>().ToList())
                Controls.Remove(control);
        }

        private void RemoveButtons()
        {
            foreach (var control in Controls.OfType<Button>().ToList())
                Controls.Remove(control);
        }

        private void LoadTags(Object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Data Files (*.txt)|*.txt";
            openFileDialog.DefaultExt = "txt";

            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                var lines = File.ReadAllLines(openFileDialog.FileName);
                UpdateDisplay(lines);
            }
            catch (Exception exception)
            {
                ShowError(exception.Message);
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void SaveCloud(Object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Data Files (*.jpg)|*.jpg";
            saveFileDialog.DefaultExt = "jpg";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = "Example.jpg";

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            using (Bitmap bmp = new Bitmap(Width, Height))
            {
                RemoveButtons();
                DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
                AddButtons();
                try
                {
                    bmp.Save(saveFileDialog.FileName, ImageFormat.Png);
                }
                catch (Exception exception)
                {
                    ShowError(exception.Message);
                }
            }

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
