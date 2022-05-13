using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        LinkedList<T> doublyLinkedList;
        private int limit;
        public int Limit
        {
            get { return limit; }
            private set { limit = value; }
        }

        public LimitedSizeStack(int limit)
        {
            doublyLinkedList = new LinkedList<T>();
            this.limit = limit;
        }

        public void Push(T item)
        {
            if (doublyLinkedList.Count < limit)
            {
                doublyLinkedList.AddFirst(item);
            }
            else
            {
                doublyLinkedList.AddFirst(item);
                doublyLinkedList.RemoveLast();
            }
        }

        public T Pop()
        {
            if (doublyLinkedList.Count == 0) throw new InvalidOperationException();
            var d = doublyLinkedList.First;
            doublyLinkedList.RemoveFirst();
            return d.Value;
        }

        public int Count { get { return doublyLinkedList.Count; } }
    }
}