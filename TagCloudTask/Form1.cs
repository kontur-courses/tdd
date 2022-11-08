using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TagCloud
{
    public partial class Form1 : Form
    {
        private bool alreadyDone = false;
        public Form1()
        {
            InitializeComponent();
            //tagCloudBuilder = new TagCloudBuilder(this.Width, this.Height);
            Paint += new PaintEventHandler(Form1_Paint!);
            
        }

        //https://stackoverflow.com/a/13103960
        private void print(Bitmap BM, PaintEventArgs e)
        {
            if (alreadyDone)
                return;
            alreadyDone = true;
            Graphics graphicsObj = e.Graphics; //Get graphics from the event
            graphicsObj.DrawImage(BM, 0, 0); // or "e.Graphics.DrawImage(bitmap, 60, 10);"
            graphicsObj.Dispose();
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // this.Width = 1280;
            // this.Height = 768;
            // var tagCloudBuilder = new TagCloudBuilder(this.Size, 0);
            // /*for (int i = 0; i < 2000; i++)
            // {
            //     tagCloudBuilder.IterationOfDrawSpiral();
            // }*/
            // var r = new Random();
            // for (var i = 0; i < 800; i++)
            // {
            //     int w = (int)(50- (i/20) - r.Next(0,2));
            //     int h = (int)(25 - (i/40) - r.Next(0,2));
            //     tagCloudBuilder.DrawRectangle(new Rectangle(0, 0, w, h));
            // }
            //
            // Bitmap bitamp = tagCloudBuilder.Bitmap;
            // print(bitamp, e);
        }
    }
}
