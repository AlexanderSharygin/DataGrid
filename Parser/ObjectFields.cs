using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Extensions
{
    public class ObjectFields : IObjectFields<string>
    {
        private MyList<string> _Keys;
        private MyList<string> _Values;
        public ObjectFields()
        {
            _Keys = new MyList<string>();
            _Values = new MyList<string>();
        }
        private void ThrowIfUseNonExistKey(string key)
        {
            if (_Keys.IndexOf(key) == -1)
            {
                throw new KeyNotFoundException("Попытка обратиться к несущесвтующему полю объекта");
            }
        }
        private void ThrowIfKeysValueVariusCount()
        {
            if (_Keys.Count != _Values.Count)
            {
                throw new Exception("Error. Keys.Count!=Values.Count");
            }
        }
        public int Count
        {
            get
            {
                ThrowIfKeysValueVariusCount();
                return _Values.Count;
            }
        }

        public MyList<string> Keys
        {
            get
            {
                return _Keys;
            }
        }

        public string this[string key]
        {
            get
            {
                ThrowIfUseNonExistKey(key);
                return _Values[_Keys.IndexOf(key)];
            }
        }
        public void Add(string p_key, string p_value)
        {
            int index = _Keys.IndexOf(p_key);
            if (index == -1)
            {
                _Keys.Add(p_key);
                _Values.Add(p_value);
            }
            else
            {
                _Values[index] = p_value;
            }
        }
    }
    public class KeyList : IObjectFields<int>
    {
        MyList<string> _AllKeys;
        public KeyList()
        {
            _AllKeys = new MyList<string>();
        }
        public int Count => _AllKeys.Count;
        public MyList<string> GetKeys => _AllKeys;

        public KeyList(MyList<JSONObject> JSONObjects)
        {
            _AllKeys = new MyList<string>();
            for (int i = 0; i < JSONObjects.Count-1; i++)
            {
                FillKeyList(JSONObjects[i].Fields.Keys);
            }
        }
        public void FillKeyList(MyList<string> p_Keys)
        {
            for (int i = 0; i < p_Keys.Count; i++)
            {
               Add(p_Keys[i]);
            }
        }
        private void ThrowIfAceessToKeyByIncorrectIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > _AllKeys.Count - 1))
            {
                throw new IndexOutOfRangeException("Попытка обратиться к объекту по недопустимому индексу (Индекс < 0 или Индекс > List.Count)");
            }
        }
        public string this[int i]
        {
            get
            {
                ThrowIfAceessToKeyByIncorrectIndex(i);
                return _AllKeys[i];
            }
        }
        public void Add(string p_key)
        {
            int index = _AllKeys.IndexOf(p_key);
            if (index == -1)
            {
                _AllKeys.Add(p_key);
            }
            else
            {
                _AllKeys[index] = p_key;
               
            }
        }
    }
    
    interface IObjectFields<T>
    {
        string this[T x]
        { get; }
        
    }
}

