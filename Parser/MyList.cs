using System;
using System.Collections.Generic;
using System.Collections;


namespace Parser.Extensions
{
    class MyEnumerator<T> : IEnumerator<T>
    {
        

        private T[] _EnumedMass;
        int _ElementIndex;
        public MyEnumerator(T[] items)
        {
            _EnumedMass = items;
            _ElementIndex = -1;
        }

        public T Current
        {
            get
            {
                return _EnumedMass[_ElementIndex];
            }
        }
        object IEnumerator.Current => Current;

        public void Dispose()
        {
         
        }

        public bool MoveNext()
        {
            _ElementIndex++;
            return _ElementIndex < _EnumedMass.Length;
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
                      
        public MyList()
        {
            _RealLength = 0;
            _List = _emptyArray;
        }
        public MyList(int capacity)
        {
            _RealLength = 0;
            if (capacity < 0)
            { throw new RankException("Невозможно создать список отрицательного размера"); }            
            if (capacity == 0)
            { _List = _emptyArray; }
            else
            { _List = new T[capacity]; }
        }
        public T this[int elementIndex]
        {
            get
            {
                TrimExcessObjects();
                return _List[elementIndex];
            }
            set
            {
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

        private void TrimExcessObjects()
        {
            T[] temp = new T[_RealLength];
            for (int i = 0; i <_RealLength; i++)
            {
                temp[i] = _List[i];
            }
            _List = temp;
        }
    
        public bool AddIsUniqueItem(T item)
        {
          
            bool isNotUnique = false;
            for (int i = 0; i < _List.Length; i++)
            {
                if (_List[i] != null)
                {
                    isNotUnique = _List[i].Equals(item);
                    if (isNotUnique)
                    { break; }
                }
            }    
            if (isNotUnique == true)
            {
                return false;
            }
            else
            {
                Insert(_RealLength, item);
                return true;
            }            
        }
        public void Insert(int index, T item)
        {
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
            if (_RealLength >= Constants.MinLengthMyListThenTrimExcessObjects)
            {
                TrimExcessObjects();
            }
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (_RealLength >= Constants.MinLengthMyListThenTrimExcessObjects)
            {
                TrimExcessObjects();
            }
            T [] tempList = new T[_RealLength-1];                    
            for (int i = 0; i < _RealLength -1; i++)
            {
                if (i < index)
                {
                    tempList[i] = _List[i];
                }
                else
                {
                    tempList[i] = _List[i + 1];
                }
            }
            _RealLength--;
            _List = tempList;
           
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
