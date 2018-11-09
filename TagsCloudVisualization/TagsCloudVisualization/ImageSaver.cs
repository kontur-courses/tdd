using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ImageSaver
    {
        public static void SaveBitmapToFile(string fileName, Bitmap bitmap)
        {
            bitmap.Save(fileName + ".bmp");
        }
    }
}
