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
            Paint += new PaintEventHandler(Form1_Paint!);
        }
        
        //https://stackoverflow.com/a/13103960
        private void print(Bitmap BM, PaintEventArgs e)
        {
            if (_alreadyDone)
                return;
            _alreadyDone = true;
            Graphics graphicsObj = e.Graphics; //Get graphics from the event
            graphicsObj.DrawImage(BM, 0, 0); // or "e.Graphics.DrawImage(bitmap, 60, 10);"
            graphicsObj.Dispose();
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            this.Width = 1280;
            this.Height = 768;
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
            cloud.Drawer.DrawTags(
                cloud.Engine.Rectangles.ToArray(), 
                cloud.TagWithSize.ToArray());
            print(cloud.Drawer.Bitmap, e);
        }
    }
}