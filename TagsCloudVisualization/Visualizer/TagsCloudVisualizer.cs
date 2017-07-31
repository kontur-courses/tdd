using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        private readonly SolidBrush brush;
        private readonly Color backgroundСolor;

        public TagsCloudVisualizer(SolidBrush brush, Color backgroundСolor)
        {
            this.brush = brush;
            this.backgroundСolor = backgroundСolor;
        }

        public Bitmap GetCloudImage(List<Tag> tags, int width, int height)
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
                frameRectangle.Location
            );
        }

    }
}