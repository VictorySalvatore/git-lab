using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehavPatterns
{
    public interface Iterator<T> // Обобщенный
    {
        bool HasNext();
        T Next(); // Следующий элемент
    }
    public interface Aggregate<T> // Интерфейс агрегата (коллекции)
    {
        Iterator<T> CreateIterator();
    }
    public class ArrayIterator<T> : Iterator<T> // Конкретный итератор для массива
    {
        private readonly T[] _items;
        private int _pos;
        public ArrayIterator(T[] items)
        {
            _items = items;
            _pos = 0;
        }
        public bool HasNext() => _pos < _items.Length;
        public T Next()
        {
            if (HasNext())
                return _items[_pos++];
            throw new IndexOutOfRangeException("No more elements in the collection.");
        }
    }
    public class ArrayCollection<T> : Aggregate<T> // Конкретный агрегат
    {
        private readonly T[] _items;
        public ArrayCollection(T[] items) { _items = items; }
        public Iterator<T> CreateIterator() => new ArrayIterator<T>(_items);
    }
}
