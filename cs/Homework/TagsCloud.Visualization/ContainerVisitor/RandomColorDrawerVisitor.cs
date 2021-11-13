using System;
using System.Drawing;
using System.Linq;
using TagsCloud.Visualization.LayoutContainer;

namespace TagsCloud.Visualization.ContainerVisitor
{
    public class RandomColorDrawerVisitor : IContainerVisitor
    {
        private readonly Random random = new();

        public void Visit(Graphics graphics, RectanglesContainer cont)
        {
            using var pen = new Pen(GetRandomColor());
            graphics.DrawRectangles(pen, cont.Items.ToArray());
        }

        public void Visit(Graphics graphics, WordsContainer container)
        {
            foreach (var word in container.Items)
            {
                var drawFormat = new StringFormat
                {
                    Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center
                };
                using var brush = new SolidBrush(GetRandomColor());
                graphics.DrawString(word.Word.Content,
                    word.Font, brush, word.Border, drawFormat);
            }
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}