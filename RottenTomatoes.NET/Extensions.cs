using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    public static class Extensions {
        public static int TryParseInt(this JToken jt) {
            if (jt == null)
                return -1;
            if (jt.Type == JTokenType.Integer)
                return (int)jt;
            else {
                int temp;
                if (int.TryParse((string)jt, out temp))
                    return temp;
                else
                    return -1;
            }
        }
    }
}
