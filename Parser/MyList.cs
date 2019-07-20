using System;
using System.Collections.Generic;
using System.Collections;


namespace Parser.Extensions
{
    class MyEnumerator<T> : IEnumerator<T>
    {
        

        private T[] _EnumedMass;
        int _elementIndex;
        public MyEnumerator(T[] items)
        {
            _EnumedMass = items;
            _elementIndex = -1;
        }

        public T Current
        {
            get
            {
                return _EnumedMass[_elementIndex];
            }
        }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
         
        }

        public bool MoveNext()
        {
            _elementIndex++;
            return _elementIndex < _EnumedMass.Length;
        }
       
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
    public class MyList<T> : IList<T>
    {
        private int _defaultMinLength = 4;
        private T[] _List;
        private int _RealLength;
        static readonly T[] _emptyArray = new T[0];

        private void ThrowIfAceessToObjectByIncorrectIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > _RealLength-1))
            {
                throw new IndexOutOfRangeException("Попытка обратиться к объекту по недопустимому индексу (Индекс < 0 или Индекс > List.Count)");
            }
        }
        private void ThrowIfInvalidInsertIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > _List.Length))
            {
                throw new IndexOutOfRangeException("Попытка добавить/получить элемент в список типа MyList с недопустимым индексом");
            }
        }
        public MyList()
        {
            _RealLength = 0;
            _List = _emptyArray;
        }
        public MyList(int capacity)
        {
            _RealLength = 0;
            if (capacity < 0)
            { throw new IndexOutOfRangeException("Попытка создать лист отрицательного размера"); }            

            if (capacity == 0)
            { _List = _emptyArray; }
            else
            { _List = new T[capacity]; }
        }
        public T this[int elementIndex]
        {
            get
            {
            ThrowIfAceessToObjectByIncorrectIndex(elementIndex);               
            return _List[elementIndex];
            }
            set
            {
                ThrowIfAceessToObjectByIncorrectIndex(elementIndex);
                _List[elementIndex] = value;
            }               
        }
        private void IncreaseLength(int minRequiredLength)
        {
            if (_List.Length < minRequiredLength)
            {
                var newLength=0;
                var oldLength = _List.Length;
                if (_List.Length == 0)
                {
                    newLength = _defaultMinLength; }
                else
                {
                    newLength = _List.Length * 2;
                }
                T[] temp = new T[newLength];
                for (int i = 0; i < oldLength; i++)
                {
                    temp[i] = _List[i];
                }
                _List = temp;
             }
        }
        public int Count => _RealLength;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(T item)
        {
            Insert(_RealLength, item);
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
            for (int i = 0; i < _RealLength; i++)
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

        public void TrimExcessObjects()
        {
            T[] temp = new T[_RealLength];
            for (int i = 0; i <_RealLength; i++)
            {
                temp[i] = _List[i];
            }
            _List = temp;
        }
        public void Insert(int index, T item)
        {
            ThrowIfInvalidInsertIndex(index);
            if (_RealLength == _List.Length)
            {
                IncreaseLength(_List.Length + 1);
            }
                        
            for (var i = _List.Length - 1; i > index; i--)
            {
                _List[i] = _List[i - 1];
            }
            _List[index] = item;
            _RealLength++;
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
