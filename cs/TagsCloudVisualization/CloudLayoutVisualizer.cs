using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudLayoutVisualizer
    {
        private static int imageBorders = 20;
        
        public static void SaveAsImage(IEnumerable<Rectangle> rects, string filePath)
        {
            var canvasSize = GetCanvasSize(rects);
            var bitmap = new Bitmap(canvasSize.Width, canvasSize.Height);
            var canvas = Graphics.FromImage(bitmap);

            foreach (var rect in rects)
            {
                canvas.DrawRectangle(
                    new Pen(Color.Black, 1),
                    rect.Pos.X + imageBorders,
                    rect.Pos.Y + imageBorders,
                    rect.Size.Width,
                    rect.Size.Height
                );
            }

            bitmap.Save(filePath.ToString());
        }

        private static Size GetCanvasSize(IEnumerable<Rectangle> rects)
        {
            Rectangle topRect = rects.First();
            Rectangle leftRect = rects.First();
            Rectangle bottomRect = rects.First();
            Rectangle rightRect = rects.First();
            
            foreach (var rect in rects)
            {
                if (rect.Pos.Y < topRect.Pos.Y)
                {
                    topRect = rect;
                }
                
                if (rect.Pos.X < leftRect.Pos.X)
                {
                    leftRect = rect;
                }

                if (rect.bottmRightPoint.Y > bottomRect.bottmRightPoint.Y)
                {
                    bottomRect = rect;
                }

                if (rect.bottmRightPoint.X > rightRect.bottmRightPoint.X)
                {
                    rightRect = rect;
                }
            }

            var w = rightRect.bottmRightPoint.X - leftRect.Pos.X + imageBorders * 2;
            var h = bottomRect.bottmRightPoint.Y - topRect.Pos.Y + imageBorders * 2;
            
            return new Size(w, h);
        }
    }
}