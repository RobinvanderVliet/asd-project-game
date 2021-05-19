using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Creature.Pathfinder
{
    public class PriorityQueue<T> where T : IComparable<T>
    {
        private List<T> _elements;

        public PriorityQueue()
        {
            _elements = new List<T>();
        }

        public int Count
        {
            get { return _elements.Count; }
        }

        public bool Contains(T item)
        {
            foreach (var element in _elements)
            {
                if (element.Equals(item))
                    return true;
            }
            return false;
        }

        [ExcludeFromCodeCoverage]
        public T Peek()
        {
            T frontItem = _elements[0];
            return frontItem;
        }

        public void Enqueue(T item)
        {
            int i = _elements.Count;
            _elements.Add(item);
            while (i > 0 && _elements[(i - 1) / 2].CompareTo(item) > 0)
            {
                _elements[i] = _elements[(i - 1) / 2];
                i = (i - 1) / 2;
            }
            _elements[i] = item;
        }

        public T Dequeue()
        {
            T firstItem = _elements[0];
            T tempItem = _elements[_elements.Count - 1];
            _elements.RemoveAt(_elements.Count - 1);
            if (_elements.Count > 0)
            {
                int i = 0;
                while (i < _elements.Count / 2)
                {
                    int j = (2 * i) + 1;
                    if ((j < _elements.Count - 1) && (_elements[j].CompareTo(_elements[j + 1]) > 0)) 
                        ++j;
                    if (_elements[j].CompareTo(tempItem) >= 0)
                        break;
                    _elements[i] = _elements[j];
                    i = j;
                }
                _elements[i] = tempItem;
            }
            return firstItem;
        }
    }
}
