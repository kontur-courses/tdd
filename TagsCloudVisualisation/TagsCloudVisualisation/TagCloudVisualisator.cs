using System.Drawing.Imaging;
using TagsCloudVisualization;

namespace TagsCloudVisualisation;

public partial class TagCloudVisualisator : Form
{
    private Graphics _graphics;
    
    private Color _backgroundColor;
    private Color _penColor;
    
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new TagCloudVisualisator());
    }
    
    private TagCloudVisualisator()
    {
        InitializeComponent();
        SetupApplication(); 
    }

    private void SetupApplication()
    {
        SetupWindow();
        SetupColors();
    }

    private void SetupWindow()
    {
        Text = "Tag Cloud Application";
        TopMost = true;
        WindowState = FormWindowState.Maximized;
    }

    private void SetupColors()
    {
        _backgroundColor = Color.FromArgb(47, 47, 42);
        _penColor = Color.White;
    }
    
    private void Render(object sender, PaintEventArgs e)
    {
        _graphics = e.Graphics;
        _graphics.Clear(_backgroundColor);
        
        var rnd = new Random();
        var pen = new Pen(_penColor);
        var cloudTag = new CircularCloudLayouter(new Point(Width / 2, Height / 2));

        for (int i = 0; i < 1000; i++)
        {
            var width = rnd.Next(1, 50);
            var height = rnd.Next(1, 50);
            var rectangle = cloudTag.PutNextRectangle(new Size(width, height));
            _graphics.DrawRectangle(pen, rectangle);    
        }
        
        ScreenShot.TakeScreenShot();
    }


}