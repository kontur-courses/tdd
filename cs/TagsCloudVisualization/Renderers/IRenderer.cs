using System.Drawing;

namespace TagsCloudVisualization.Renderers;

interface IRenderer
{
    void AddRectangle(Rectangle rectangle);
    void AddRectangles(Rectangle[] rectangles);
    void SaveImage(string filename);
    void Clear();
}