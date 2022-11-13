using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TagCloud2
{
    public partial class Form1 : Form
    {
        private bool _alreadyDone;
        public Form1()
        {
            InitializeComponent();
            this.Width = 1280;
            this.Height = 768;
            Paint += new PaintEventHandler(Form1_Paint!);
        }
        
        //https://stackoverflow.com/a/13103960
        private void Print(Bitmap BM, PaintEventArgs e)
        {
            if (_alreadyDone)
                return;
            _alreadyDone = true;

            using (Graphics graphicsObj = e.Graphics)
                graphicsObj.DrawImage(BM, 0, 0);
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var maxFontSize = 80;
            var minFontSize = 20;

            var cloud = new SpiralTagCloud(
                new SpiralTagCloudEngine(new Point(Size / 2)),
                new SpiralTagCloudBitmapDrawer(Size, "Consolas"),
                new DataParser(),
                minFontSize,
                maxFontSize
            );
            
            cloud.Parser.ParseFile("2.txt");
            cloud.CreateTagCloud();


            //cloud.Drawer.DrawRectangles(cloud.Engine.Rectangles.ToArray());
            cloud.Drawer.DrawRectanglesWithTags(
                cloud.Engine.Rectangles.ToArray(), 
                cloud.TagWithSize.ToArray());
            Print(cloud.Drawer.Bitmap, e);
        }
    }
}