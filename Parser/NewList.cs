using System;

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;


namespace Parser.Extensions
{
    class MyEnumerator<T> : IEnumerator<T>
    {
        

        T[] EnumMass;
        int index;
        public MyEnumerator(T[] items)
        {
            EnumMass = items;
            index = -1;
        }

        public T Current
        {
            get
            {
                return EnumMass[index];
            }
        }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
         
        }

        public bool MoveNext()
        {
            index++;
            return index < EnumMass.Length;
        }
       
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
    public class NewList<T> : IList<T>
    {
        private T[] _List;
        static readonly T[] _emptyArray = new T[0];

        private void ThrowIfInvalidInsertIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > _List.Length))
            {
                throw new IndexOutOfRangeException("Попытка добавить/получить элемент в список типа MyList с недопустимым индексом");
            }
        }
        public NewList()
        {
            _List = _emptyArray;
        }
        public NewList(int capacity)
        {
            if (capacity == 0)
                _List = _emptyArray;
            else
                _List = new T[capacity];
        }
        public T this[int elementIndex]
        {
            get
            { 
            ThrowIfInvalidInsertIndex(elementIndex); 
            return _List[elementIndex];
            }
            set
            {
                ThrowIfInvalidInsertIndex(elementIndex);
                _List[elementIndex] = value;
            }               
        }

        public int Count => _List.Length;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            Insert(_List.Length, item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
   
        
         public IEnumerator<T> GetEnumerator()
         {
            var Enum = new MyEnumerator<T>(_List);
             return Enum;
         }

        public int IndexOf(T item)
        {
            int itemFirstIndex = 0;
            for (int i = 0; i < _List.Length; i++)
            {
                T temp = _List[i];
                if (item.Equals(temp))
                {
                    return itemFirstIndex;
                }
                itemFirstIndex++;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            ThrowIfInvalidInsertIndex(index);
            T[] temp = new T[_List.Length+1];
            for (int i = 0; i < _List.Length; i++)
            {
                temp[i] = _List[i];
            }
            _List = temp;
            for (var i = _List.Length - 1; i > index; i--)
            {
                _List[i] = _List[i - 1];
            }
            _List[index] = item;
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
