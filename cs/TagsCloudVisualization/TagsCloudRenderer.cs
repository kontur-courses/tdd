using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsCloudRenderer
    {
        public void RenderIntoFile(string filePath, TagsCloud tagsCloud, Size pictureSize)
        {
            var btm = new Bitmap(pictureSize.Width, pictureSize.Height);
            var obj = Graphics.FromImage(btm);
            foreach (var r in tagsCloud.AddedRectangles)
            {
                obj.DrawRectangle(new Pen(Color.Brown), r);
            }

            btm.Save(filePath);
        }
    }
}