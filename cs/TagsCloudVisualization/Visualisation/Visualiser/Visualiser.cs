using System.Drawing;

namespace TagsCloudVisualization.Visualisation
{
    public class Visualiser : IVisualiser
    {
        private readonly Pen pen;

        private bool isDisposed = false;

        public Visualiser(Color penColor, int penWidth)
        {
            pen = new Pen(penColor, penWidth);
        }

        public void DrawRectangle(Bitmap bitmap, Rectangle rectangle)
        {
            if (bitmap == null)
                throw new ArgumentNullException($"bitmap field cannot be null {bitmap}");

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawRectangle(pen, rectangle);
            }
        }

        ~Visualiser()
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
                pen.Dispose();
            }

            isDisposed = true;
        }
    }
}
