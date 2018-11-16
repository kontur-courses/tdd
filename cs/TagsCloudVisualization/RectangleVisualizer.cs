using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class RectangleVisualizer<Layouter, Visualizer> 
        where Layouter : ICloudLayouter, new()
        where Visualizer : ICloudVisualizer, new()
    {
        public IEnumerable<Size> Rectangles { get; }

        public RectangleVisualizer(IEnumerable<Size> rectangles)
		{
            Rectangles = rectangles;
        }

        public void CreateCloudImage(string path)
        {
            
        }
    }
}
