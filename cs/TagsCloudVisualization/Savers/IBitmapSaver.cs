using System.Drawing;

namespace TagsCloudVisualization.Savers
{
    public interface IBitmapSaver
    {
        public void Save(Bitmap bitmap, string name);
    }
}