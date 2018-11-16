using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class RectangleVisualizer
    {
        public IEnumerable<Size> Rectangles { get; }
        public ICloudLayouter Layouter { get; }
        public ICloudVisualizer Visualizer { get; }

        public RectangleVisualizer(IEnumerable<Size> rectangles,
            ICloudLayouter layouter, ICloudVisualizer visualizer)
        {
            Rectangles = rectangles;
            Layouter = layouter;
            Visualizer = visualizer;
        }

        public void CreateCloudImage(string path)
        {
            Rectangles.Select(Layouter.PutNextRectangle);
            Visualizer.CreateImage(Layouter.Rectangles, path);
        }
    }
}
