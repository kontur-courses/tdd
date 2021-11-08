using System;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class SortedSingleLinkedList<TValue>
    {
        private Node<TValue> root;
        private readonly Func<TValue, TValue, bool> sep;

        public SortedSingleLinkedList(Func<TValue, TValue, bool> separator)
        {
            sep = separator;
        }

        public void Add(TValue value)
        {
            var node = new Node<TValue>(value);
            if (root is null)
            {
                root = node;
                return;
            }

            Node<TValue> previousNode = null;
            var currentNode = root;
            while (currentNode != null && !sep(currentNode.Value, node.Value))
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            if (previousNode is null)
            {
                node.Next = root;
                root = node;
                return;
            }

            previousNode.Next = node;
            node.Next = currentNode;
        }

        public void Remove(TValue value)
        {
            Node<TValue> previousNode = null;
            var currentNode = root;
            while (currentNode != null && !currentNode.Value.Equals(value))
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            if (currentNode is null)
                throw new ArgumentException("No such argument");

            if (previousNode is null)
            {
                root = currentNode.Next;
                return;
            }

            previousNode.Next = currentNode.Next;
        }

        public IEnumerable<TValue> ToEnumerable()
        {
            var node = root;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }
    }

    public class Node<TValue>
    {
        public TValue Value;
        public Node<TValue> Next;

        public Node(TValue value, Node<TValue> next = null)
        {
            Value = value;
            Next = next;
        }
    }
}