using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        private readonly SolidBrush brush;
        private readonly StringFormat drawFormat;

        public TagsCloudVisualizer(SolidBrush brush)
        {
            this.brush = brush;
            drawFormat = new StringFormat();
        }

        public Bitmap GetImageCloud(List<Tag> tags, int width, int height, Color backgroundСolor)
        {
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(backgroundСolor);
                foreach (var tag in tags)
                {
                    DrawPhrase(graphics, tag.Phrase, tag.Rectangle, tag.Font);
                }
            }
            return bitmap;
        }

        private void DrawPhrase(Graphics graphics, string phrase, Rectangle frameRectangle, Font font)
        {
            graphics.DrawString(
                phrase,
                font,
                brush,
                frameRectangle,
                drawFormat
            );
        }

    }
}