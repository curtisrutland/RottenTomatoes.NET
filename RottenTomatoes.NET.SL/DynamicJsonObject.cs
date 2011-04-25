using System.Dynamic;
using System.Json;
using System.Linq;
using System;

namespace RottenTomatoes.NET.SL {
    public class DynamicJsonObject : DynamicObject {
        public static int ParseIntFromDyn(dynamic d) {
            if (d is string) {
                int i;
                if (int.TryParse(d, out i))
                    return i;
                else return -1;
            }
            else if (d is int)
                return d;
            else throw new ArgumentException("Was neither string nor int!", "d");
        }

        public JsonObject jsonObject;

        public static DynamicJsonObject Parse(string jsonString){
            return new DynamicJsonObject(jsonString);
        }

        private DynamicJsonObject(string jsonString) {
            jsonObject = (JsonObject)JsonObject.Parse(jsonString);
        }

        private DynamicJsonObject(JsonObject o) {
            jsonObject = o;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (!jsonObject.ContainsKey(binder.Name))
                jsonObject.Add(binder.Name, (dynamic)value);
            else
                jsonObject[binder.Name] = (dynamic)value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (!jsonObject.ContainsKey(binder.Name)) {
                result = null;
                return true;
            }
            else {
                var member = jsonObject[binder.Name];
                result = GetJsonValue(member);
                return true;
            }
        }

        private object GetJsonValue(JsonValue member) {
            object result = null;
            if (member.JsonType == JsonType.Array) {
                var array = member as JsonArray;
                if (array.Any()) {
                    if (array.First().JsonType == JsonType.Object || array.First().JsonType == JsonType.Array)
                        result = array.Select(x => new DynamicJsonObject(x as JsonObject)).ToArray();
                    else
                        result = array.Select(x => GetJsonValue(x)).ToArray();
                }
                else
                    result = member;
            }
            else if (member.JsonType == JsonType.Object) {
                return new DynamicJsonObject(member as JsonObject);
            }
            else if (member.JsonType == JsonType.Boolean)
                return (bool)member;
            else if (member.JsonType == JsonType.Number) {
                string s = member.ToString();
                int i;
                if (int.TryParse(s, out i))
                    return i;
                else return double.Parse(s);
            }
            else
                return (string)member;

            return result;
        }
    }
}
