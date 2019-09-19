using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Trash
{
    public class AgregatedKeyList : IEnumerable
    {
        List<string> _AgregatedKeys;

        public AgregatedKeyList()
        {
            _AgregatedKeys = new List<string>();
        }
        public AgregatedKeyList(List<Dictionary<string, string>> JSONObjects)
        {
            _AgregatedKeys = new List<string>();
            for (int i = 0; i < JSONObjects.Count; i++)
            {
                foreach (var item in JSONObjects[i].Keys)
                {
                    Add(item);
                }

            }
        }

        public int Count => _AgregatedKeys.Count;
        public List<string> GetKeys => _AgregatedKeys;
        public string this[int i]
        {
            get
            {
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

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_AgregatedKeys).GetEnumerator();
        }

    }

    public class ObjectFields
    {
        private List<string> _Keys = new List<string>();
        private List<string> _Values = new List<string>();
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

                return _Values.Count;
            }
        }

        public List<string> Keys
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
        public int ValueIndexOf(string key)
        {
            int itemFirstIndex = 0;
            for (int i = 0; i < _Values.Count; i++)
            {
                string temp = _Values[i];
                if (key.Equals(temp))
                {
                    return itemFirstIndex;
                }
                itemFirstIndex++;
            }
            return -1;
        }
        public int KeyIndexOf(string key)
        {
            int itemFirstIndex = 0;
            for (int i = 0; i < _Keys.Count; i++)
            {
                string temp = _Keys[i];
                if (key.Equals(temp))
                {
                    return itemFirstIndex;
                }
                itemFirstIndex++;
            }
            return -1;
        }
    }
}
