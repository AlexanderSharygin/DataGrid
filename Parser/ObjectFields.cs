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
        private void ThrowIfTryToUseNonExistKey(string key)
        {
            if (_Keys.IndexOf(key) == -1)
            {
                throw new KeyNotFoundException("Попытка обратиться к несущесвтующему полю объекта");
            }
        }
        private void ThrowIfKeysAndValuesHaveVariusCounts()
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
                ThrowIfKeysAndValuesHaveVariusCounts();
                return _Values.Count;
            }
        }

        public MyList<string> Keys
        {
            get
            { return _Keys; }
            set
            { _Keys = value; }
        }
        public string this[string key]
        {
            get
            {
                ThrowIfTryToUseNonExistKey(key);
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
    public class AgregatedKeyList : IObjectFields<int>
    {
        MyList<string> _AgregatedKeys;
        public AgregatedKeyList()
        {
            _AgregatedKeys = new MyList<string>();
        }
        public AgregatedKeyList(MyList<JSONObject> JSONObjects)
        {
            _AgregatedKeys = new MyList<string>();
            for (int i = 0; i < JSONObjects.Count-1; i++)
            {
                AddKeysFromObject(JSONObjects[i]);
            }
        }
        private void ThrowIfTryAceessToKeyByIncorrectIndex(int itemIndex)
        {
            if ((itemIndex < 0) || (itemIndex > _AgregatedKeys.Count - 1))
            {
                throw new IndexOutOfRangeException("Попытка обратиться к объекту по недопустимому индексу (Индекс < 0 или Индекс > List.Count)");
            }
        }
        public int Count => _AgregatedKeys.Count;
        public MyList<string> GetKeys => _AgregatedKeys;
        public string this[int i]
        {
            get
            {
                ThrowIfTryAceessToKeyByIncorrectIndex(i);
                return _AgregatedKeys[i];
            }
        }
       public void Add(string p_key)
        {
            int index = _AgregatedKeys.IndexOf(p_key);
            if (index == -1)
            {
                _AgregatedKeys.Add(p_key);
            }
            else
            {
                _AgregatedKeys[index] = p_key;
               
            }
        }
        public void AddKeysFromObject(JSONObject p_JSONObject)
        {
            MyList<string> keys = p_JSONObject.Fields.Keys;
            for (int i = 0; i < keys.Count; i++)
            {
                Add(keys[i]);
            }
        }
    }
    
    interface IObjectFields<T>
    {
        string this[T x]
        { get; }
        
    }
}

