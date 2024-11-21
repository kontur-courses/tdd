using System.Drawing;
using TagsCloudVisualization.CloudClasses;
using TagsCloudVisualization.CloudClasses.Interfaces;

namespace TagsCloudVisualization.Visualisation
{
    public class TagCloudImageGenerator : IDisposable
    {
        private readonly IVisualiser Visualiser;

        private bool isDisposed = false;

        public TagCloudImageGenerator(IVisualiser visualiser)
        {
            Visualiser = visualiser;
        }

        public Bitmap CreateNewBitmap(Size bitmapSize, ICloudLayouter layouter,
            Func<IEnumerable<Size>> configurationFunc)
        {
            var newBitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

            foreach (var size in configurationFunc())
            {
                Visualiser.DrawRectangle(newBitmap, layouter.PutNextRectangle(size));
            }

            return newBitmap;
        }

        public Bitmap CreateNewBitmap(Size bitmapSize, List<Rectangle> rectangles)
        {
            var newBitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);

            foreach (var rectangle in rectangles)
            {
                Visualiser.DrawRectangle(newBitmap, rectangle);
            }

            return newBitmap;
        }

        public void AddToCurrentImage(Bitmap bitmap, ICloudLayouter layouter,
            Func<IEnumerable<Size>> configurationFunc)
        {
            foreach (var size in configurationFunc())
            {
                Visualiser.DrawRectangle(bitmap, layouter.PutNextRectangle(size));
            }
        }

        public void AddToCurrentImage(Bitmap bitmap, List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                Visualiser.DrawRectangle(bitmap, rectangle);
            }
        }

        ~TagCloudImageGenerator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool fromMethod)
        {
            if (isDisposed)
                return;

            if (fromMethod)
            {
                Visualiser.Dispose();
            }

            isDisposed = true;
        }
    }
}
