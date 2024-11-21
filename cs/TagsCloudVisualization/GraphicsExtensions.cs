using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class GraphicsExtensions
    {
        public static void DrawGrid(this Graphics graphics, int cellsCount = 100, int cellSize = 10)
        {
            Pen p = new (Color.DarkGray);

            for (int y = 0; y < cellsCount; ++y)
            {
                graphics.DrawLine(p, 0, y * cellSize, cellsCount * cellSize, y * cellSize);
            }

            for (int x = 0; x < cellsCount; ++x)
            {
                graphics.DrawLine(p, x * cellSize, 0, x * cellSize, cellsCount * cellSize);
            }
        }
    }
}
