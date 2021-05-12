using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Pathfinder
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> elements = new List<T>();

        public int Count
        {
            get { return elements.Count; }
        }

        public bool Contains(T item)
        {
            foreach (var element in elements)
            {
                if (element.Equals(item))
                    return true;
            }
            return false;
        }

        //public T Peek()
        //{
        //    T frontItem = elements[0];
        //    return frontItem;
        //}

        public void Enqueue(T item)
        {
            int i = elements.Count;
            elements.Add(item);
            while (i > 0 && elements[(i - 1) / 2].CompareTo(item) > 0)
            {
                elements[i] = elements[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            elements[i] = item;
        }

        public T Dequeue()
        {
            T firstItem = elements[0];
            T tempItem = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            if (elements.Count > 0)
            {
                int i = 0;
                while (i < elements.Count / 2)
                {
                    int j = (2 * i) + 1;
                    if ((j < elements.Count - 1) && (elements[j].CompareTo(elements[j + 1]) > 0)) ++j;
                    if (elements[j].CompareTo(tempItem) >= 0)
                        break;
                    elements[i] = elements[j];
                    i = j;
                }
                elements[i] = tempItem;
            }
            return firstItem;
        }

        //public void Enqueue(T item)
        //{
        //    elements.Add(item);
        //}

        //public T Dequeue()
        //{
        //    T bestItem = elements[0];
        //    elements.RemoveAt(0);
        //    return bestItem;
        //}
    }
}
