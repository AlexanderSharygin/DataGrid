
using Parser.Extensions;
using System;

namespace Parser
{
    // excessive abstraction
    public class JSONObject
    {
        public string _FirstName;
        public string FirstName        
        { get=>_FirstName; }
        public string _LastName;
        public string LastName
        { get => _LastName; }
        public ObjectFields _Fields;
        public JSONObject()
        {
            _Fields = new ObjectFields();
        }
        public ObjectFields Fields
        {
            get
            { return _Fields; }
        }
        public void MapObjectFields()
        {
            if (_Fields == null)
            {
                throw new NullReferenceException();
            }
            else
            {               
                if (_Fields.KeyIndexOf("FirstName") == -1)
                {
                    _FirstName = Constants.TextForUndefinedField;
                }
                else
                {
                    _FirstName = _Fields["FirstName"];
                }
                if (_Fields.KeyIndexOf("LastName") == -1)
                {
                    _LastName = Constants.TextForUndefinedField;
                }
                else
                {
                    _LastName = _Fields["LastName"];
                }

            }
        }
    }
}
