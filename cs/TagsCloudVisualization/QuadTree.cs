using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class QuadTree<T>
    {
        private Rectangle _bounds; // overall bounds we are indexing.
        private Quadrant _root;
        private IDictionary<T, Quadrant> _table;
        private IDictionary<T, Rectangle> _insertedBounds;
        
        public Rectangle Bounds
        {
            get { return this._bounds; }
            set { this._bounds = value; ReIndex(); }
        }

        public IEnumerable<T> Nodes
        {
            get { return this._insertedBounds.Keys; }
        }

        public QuadTree(Rectangle initialBounds)
        {
            if (initialBounds.Width == 0 || initialBounds.Height == 0)
            {
                throw new ArgumentException("Bounds must be non zero");
            }
            
            _bounds = initialBounds;
        }
 
        public void Insert(T node, Rectangle bounds)
        {
            if (bounds.Width == 0 || bounds.Height == 0)
            {
                throw new ArgumentException("Bounds must be non zero");
            }

            // Increase bounds if rect bounds is outside current quad tree bounds
            if (!this._bounds.Contains(bounds))
            {
                var outerRect = Rectangle.Union(this._bounds, bounds);
                
                var newBoundsPos = new Point(
                    outerRect.X - outerRect.Width,
                    outerRect.Y - outerRect.Height);
                
                var newBondsSize = new Size(
                    outerRect.X + outerRect.Width * 3,
                    outerRect.Y + outerRect.Height * 3);

                this._bounds = new Rectangle(newBoundsPos, newBondsSize);
                
                ReIndex();
            }
            
            if (_root == null)
            {
                _root = new Quadrant(null, this._bounds);
            }
 
            Quadrant parent = _root.Insert(node, bounds);
 
            if (_table == null)
            {
                _table = new Dictionary<T, Quadrant>();
            }

            if (_insertedBounds == null)
            {
                _insertedBounds = new Dictionary<T, Rectangle>();
            }

            _insertedBounds[node] = bounds;
            _table[node] = parent;
        }

        public IEnumerable<T> GetNodesInside(Rectangle bounds)
        {
            foreach (QuadNode n in GetNodes(bounds))
            {
                yield return n.Node;
            }
        }
 
        public bool HasNodesInside(Rectangle bounds)
        {
            if (_root == null)
            {
                return false;                
            }
            
            return _root.HasIntersectedNodes(bounds);
        }
 
        IEnumerable<QuadNode> GetNodes(Rectangle bounds)
        {
            List<QuadNode> result = new List<QuadNode>();
            
            if (_root != null)
            {
                _root.GetOverlappedNodes(result, bounds);
            }
            
            return result;
        }
 
        public bool Remove(T node)
        {
            if (_table != null)
            {
                Quadrant parent = null;
                if (_table.TryGetValue(node, out parent))
                {
                    parent.RemoveNode(node);
                    _table.Remove(node);
                    _insertedBounds.Remove(node);
                    return true;
                }
            }
            
            return false;
        }
 
        void ReIndex()
        {
            _root = null;

            if (_insertedBounds != null)
            {
                var insertedBounds = _insertedBounds.ToList();
                foreach (var keyValueBounds in insertedBounds)
                {
                    Insert(keyValueBounds.Key, keyValueBounds.Value);
                }
            }
        }

        private class TableValue
        {
            public Quadrant Parent;
            public Rectangle Bounds;
            public T Node;
        }
 
        internal class QuadNode
        {
            private Rectangle _bounds;
            private QuadNode _next; // linked in a circular list.
            private T _node; // the actual visual object being stored here.
 
            public QuadNode(T node, Rectangle bounds)
            {
                this._node = node;
                this._bounds = bounds;
            }
 
            public T Node
            {
                get { return this._node; }
                set { this._node = value; }
            }
 
            public Rectangle Bounds
            {
                get { return this._bounds; }
            }
 
            public QuadNode Next
            {
                get { return this._next; }
                set { this._next = value; }
            }
        }
 
        internal class Quadrant
        {
            private readonly Quadrant _parent;
            private readonly Rectangle _bounds; // quadrant bounds.
 
            QuadNode nodes; // nodes that overlap the sub quadrant boundaries.
 
            // The quadrant is subdivided when nodes are inserted that are 
            // completely contained within those subdivisions.
            Quadrant topLeft;
            Quadrant topRight;
            Quadrant bottomLeft;
            Quadrant bottomRight;
 
            public Quadrant(Quadrant parent, Rectangle bounds)
            {
                this._parent = parent;

                if (bounds.Width == 0 || bounds.Height == 0)
                {
                    throw new ArgumentException("Bounds must be non zero");
                }
                
                this._bounds = bounds;
            }
 
            internal Quadrant Parent
            {
                get { return this._parent; }
            }
 
            internal Rectangle Bounds
            {
                get { return this._bounds; }
            }
 
            internal Quadrant Insert(T node, Rectangle bounds)
            {
                if (bounds.Width == 0 || bounds.Height == 0)
                {
                    throw new ArgumentException("Bounds must be non zero");
                }
 
                Quadrant toInsert = this;
                while (true)
                {
                    int w = toInsert._bounds.Width / 2;
                    if (w < 1)
                    {
                        w = 1;
                    }
                    
                    int h = toInsert._bounds.Height / 2;
                    if (h < 1)
                    {
                        h = 1;
                    }
 
                    Rectangle topLeft = new Rectangle(toInsert._bounds.X, toInsert._bounds.Y, w, h);
                    Rectangle topRight = new Rectangle(toInsert._bounds.X + w, toInsert._bounds.Y, w, h);
                    Rectangle bottomLeft = new Rectangle(toInsert._bounds.X, toInsert._bounds.Y + h, w, h);
                    Rectangle bottomRight = new Rectangle(toInsert._bounds.X + w, toInsert._bounds.Y + h, w, h);
 
                    Quadrant child = null;
 
                    // See if any child quadrants completely contain this node.
                    if (topLeft.Contains(bounds))
                    {
                        if (toInsert.topLeft == null)
                        {
                            toInsert.topLeft = new Quadrant(toInsert, topLeft);
                        }
                        child = toInsert.topLeft;
                    }
                    else if (topRight.Contains(bounds))
                    {
                        if (toInsert.topRight == null)
                        {
                            toInsert.topRight = new Quadrant(toInsert, topRight);
                        }
                        child = toInsert.topRight;
                    }
                    else if (bottomLeft.Contains(bounds))
                    {
                        if (toInsert.bottomLeft == null)
                        {
                            toInsert.bottomLeft = new Quadrant(toInsert, bottomLeft);
                        }
                        child = toInsert.bottomLeft;
                    }
                    else if (bottomRight.Contains(bounds))
                    {
                        if (toInsert.bottomRight == null)
                        {
                            toInsert.bottomRight = new Quadrant(toInsert, bottomRight);
                        }
                        child = toInsert.bottomRight;
                    }
 
                    if (child != null)
                    {
                        toInsert = child;
                    }
                    else
                    {
                        QuadNode n = new QuadNode(node, bounds);
                        if (toInsert.nodes == null)
                        {
                            n.Next = n;
                        }
                        else
                        {
                            // link up in circular link list.
                            QuadNode x = toInsert.nodes;
                            n.Next = x.Next;
                            x.Next = n;
                        }
                        toInsert.nodes = n;
                        return toInsert;
                    }
                }
            }
 
            internal void GetOverlappedNodes(List<QuadNode> nodes, Rectangle bounds)
            {
                int w = this._bounds.Width / 2;
                int h = this._bounds.Height / 2;
 
                // assumption that the Rect struct is almost as fast as doing the operations
                // manually since Rect is a value type.
 
                Rectangle topLeft = new Rectangle(this._bounds.X, this._bounds.Y, w, h);
                Rectangle topRight = new Rectangle(this._bounds.X + w, this._bounds.Y, w, h);
                Rectangle bottomLeft = new Rectangle(this._bounds.X, this._bounds.Y + h, w, h);
                Rectangle bottomRight = new Rectangle(this._bounds.X + w, this._bounds.Y + h, w, h);
 
                // See if any child quadrants completely contain this node.
                if (topLeft.IntersectsWith(bounds) && this.topLeft != null)
                {
                    this.topLeft.GetOverlappedNodes(nodes, bounds);
                }
 
                if (topRight.IntersectsWith(bounds) && this.topRight != null)
                {
                    this.topRight.GetOverlappedNodes(nodes, bounds);
                }
 
                if (bottomLeft.IntersectsWith(bounds) && this.bottomLeft != null)
                {
                    this.bottomLeft.GetOverlappedNodes(nodes, bounds);
                }
 
                if (bottomRight.IntersectsWith(bounds) && this.bottomRight != null)
                {
                    this.bottomRight.GetOverlappedNodes(nodes, bounds);
                }
 
                GetOverlappedNodes(this.nodes, nodes, bounds);
            }
 
            static void GetOverlappedNodes(QuadNode last, List<QuadNode> nodes, Rectangle bounds)
            {
                if (last != null)
                {
                    QuadNode n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (n.Bounds.IntersectsWith(bounds))
                        {
                            nodes.Add(n);
                        }
                    } while (n != last);
                }
            }
 
            internal bool HasIntersectedNodes(Rectangle bounds)
            {
                int w = this._bounds.Width / 2;
                int h = this._bounds.Height / 2;
 
                // assumption that the Rect struct is almost as fast as doing the operations
                // manually since Rect is a value type.
 
                Rectangle topLeft = new Rectangle(this._bounds.X, this._bounds.Y, w, h);
                Rectangle topRight = new Rectangle(this._bounds.X + w, this._bounds.Y, w, h);
                Rectangle bottomLeft = new Rectangle(this._bounds.X, this._bounds.Y + h, w, h);
                Rectangle bottomRight = new Rectangle(this._bounds.X + w, this._bounds.Y + h, w, h);
 
                bool found = false;
 
                // See if any child quadrants completely contain this node.
                if (topLeft.IntersectsWith(bounds) && this.topLeft != null)
                {
                    found = this.topLeft.HasIntersectedNodes(bounds);
                }
 
                if (!found && topRight.IntersectsWith(bounds) && this.topRight != null)
                {
                    found = this.topRight.HasIntersectedNodes(bounds);
                }
 
                if (!found && bottomLeft.IntersectsWith(bounds) && this.bottomLeft != null)
                {
                    found = this.bottomLeft.HasIntersectedNodes(bounds);
                }
 
                if (!found && bottomRight.IntersectsWith(bounds) && this.bottomRight != null)
                {
                    found = this.bottomRight.HasIntersectedNodes(bounds);
                }
                if (!found)
                {
                    found = HasIntersectedNodes(this.nodes, bounds);
                }
                return found;
            }
 
            static bool HasIntersectedNodes(QuadNode last, Rectangle bounds)
            {
                if (last != null)
                {
                    QuadNode n = last;
                    do
                    {
                        n = n.Next; // first node.
                        if (n.Bounds.IntersectsWith(bounds))
                        {
                            return true;
                        }
                    } while (n != last);
                }
                return false;
            }
 
            internal bool RemoveNode(T node)
            {
                bool rc = false;
                if (this.nodes != null)
                {
                    QuadNode p = this.nodes;
                    while (!p.Next.Node.Equals(node) && p.Next != this.nodes)
                    {
                        p = p.Next;
                    }
                    if (p.Next.Node.Equals(node))
                    {
                        rc = true;
                        QuadNode n = p.Next;
                        if (p == n)
                        {
                            // list goes to empty
                            this.nodes = null;
                        }
                        else
                        {
                            if (this.nodes == n) this.nodes = p;
                            p.Next = n.Next;
                        }
                    }
                }
                return rc;
            }
        }
    }
}
