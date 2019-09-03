
using Parser.Extensions;
using System;

namespace Parser
{
    // excessive abstraction
    public class JSONObject
    {
       
       
        public ObjectFields _Fields = new ObjectFields();
      
        public ObjectFields Fields
        {
            get
            { return _Fields; }
        }
       
        }
    }

