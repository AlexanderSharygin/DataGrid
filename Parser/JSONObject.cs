
using Parser.Extensions;
namespace Parser
{
    public class JSONObject
    {
        string FirstName
        { get; set; }
        string LastName
        { get; set; }
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
    }
}
