
using Parser.Extensions;
namespace Parser
{
    // excessive abstraction
    public class JSONObject
    {
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
