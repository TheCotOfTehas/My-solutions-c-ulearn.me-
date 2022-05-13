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

        public Node(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            Value = key;
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

        public IEnumerator<TKey> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}