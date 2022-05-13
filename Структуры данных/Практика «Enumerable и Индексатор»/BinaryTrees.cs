using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public interface IPriorityQueue<TKey>
    {
        void Add(TKey key);
        bool Contains(TKey key);
    }

    public class Node<TKey> where TKey : IComparable
    {
        public TKey Value { get; }
        public Node<TKey> Left, Right, ParentNode;
        public int Size { get; set; }
        public Node(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Value = key;
            Size = 1;
        }

        internal IEnumerable<TKey> GetValues()
        {
            if (Left != null)
            {
                foreach (var value in Left.GetValues())
                    yield return value;
            }

            yield return Value;

            if (Right != null)
            {
                foreach (var value in Right.GetValues())
                    yield return value;
            }
        }
    }

    public class BinaryTree<TKey> : IPriorityQueue<TKey>, IEnumerable<TKey> where TKey : IComparable
    {
        Node<TKey> root = null;

        public void Add(TKey key)
        {
            var newNode = new Node<TKey>(key);
            if (root == null) root = newNode;
            else
            {
                var currentNode = root;
                while (true)
                    if (currentNode.Value.CompareTo(key) > 0)
                    {
                        if (currentNode.Left == null)
                        {
                            currentNode.Left = new Node<TKey>(key);
                            break;
                        }
                        currentNode = currentNode.Left;
                    }
                    else
                    {
                        if (currentNode.Right == null)
                        {
                            currentNode.Right = new Node<TKey>(key);
                            break;
                        }
                        currentNode = currentNode.Right;
                    }
            }
        }


        public bool Contains(TKey key)
        {
            var node = root;
            while (node != null)
            {
                if (node.Value.CompareTo(key) == 0) return true;

                if (node.Value.CompareTo(key) > 0) node = node.Left;
                else node = node.Right;
            }

            return false;
        }

        public TKey this[int index]
        {
            get
            {
                if (root == null || index < 0)
                    throw new IndexOutOfRangeException();

                if (index >= root.Size)
                    throw new IndexOutOfRangeException();

                var node = root;
                while (true)
                {
                    var nodeLeft = node.Left;
                    int sizeLeft;
                    if (nodeLeft == null) sizeLeft = 0;
                    else sizeLeft = nodeLeft.Size;

                    if (index == sizeLeft) return node.Value;

                    if (index < sizeLeft) node = node.Left;
                    else
                    {
                        node = node.Right;
                        index -= 1 + sizeLeft;//бошка кипеть. Типо уровень меньше индекс минус
                    }
                }
            }
        }

        public IEnumerator<TKey> GetEnumerator()
        {
            if (root == null) return null;

            return root.GetValues().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}