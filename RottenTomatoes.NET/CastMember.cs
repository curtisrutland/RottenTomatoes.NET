using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class CastMember {
        [DataMember]
        string Name { get; set; }
        [DataMember]
        public List<string> Characters { get; set; }

        internal static CastMember Parse(JToken json) {
            return new CastMember() {
                Name = (string)json["name"],
                Characters = json["characters"] == null ? null : json["characters"].Select(x => (string)x).ToList()
            };
        }
    }
}
