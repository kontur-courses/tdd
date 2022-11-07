using System.Drawing;

namespace TagsCloudVisualization.Structures
{
    public class QuadTree
    {
        private readonly Rectangle canvas;
        private readonly QuadTreeNode root;

        public QuadTree(Rectangle canvas)
        {
            this.canvas = canvas;
            root = new QuadTreeNode(this.canvas);
        }

        public void Insert(Rectangle item)
        {
            root.Insert(item);
        }

        public bool HasContent(Rectangle area)
        {
            return root.Query(area);
        }
    }
}