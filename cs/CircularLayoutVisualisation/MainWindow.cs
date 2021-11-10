using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TagsCloudVisualization;

namespace CircularLayoutVisualisation
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            EnterButton.Click += Enter_Click;
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            if (int.TryParse(Count.Text, out var count)
                && double.TryParse(SizeScale.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out var scale))
            {
                var bitmap = Visualizer.GetRandomBitmap(count, scale);

                var imageForm = new Form
                {
                    ClientSize = bitmap.Size
                };
                imageForm.Paint += (_, e) => e.Graphics.DrawImage(bitmap, Point.Empty);
                imageForm.Show();
                imageForm.Invalidate();
            }
        }
    }
}
