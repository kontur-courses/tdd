using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class CenterRectanglePlacer : IRectanglePlacer
    {
        //todo add check on reserved space -> delete check from layouter
        public Rectangle PlaceRectangle(Size size, Point startPoint, IReadOnlyCollection<Rectangle> reservedSpace)
        {
            return RectangleBuilder.CreateRectangle(size, startPoint); 
        }
    }
}