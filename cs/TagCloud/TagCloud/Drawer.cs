using System.Drawing;

namespace TagCloud
{
    public class Drawer
    {
        private readonly Bitmap bitmap;
        
        public Drawer(Size imgSize)
        {
            bitmap = new Bitmap(imgSize.Width, imgSize.Height);
        }

        public void DrawWord(string word, Font font)
        {
        }

        public void SaveImg()
        {
        }
    }
}