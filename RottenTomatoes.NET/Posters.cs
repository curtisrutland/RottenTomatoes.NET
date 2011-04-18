using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class Posters {
        [DataMember]
        public string Thumbnail { get; set; }
        [DataMember]
        public string Profile { get; set; }
        [DataMember]
        public string Detailed { get; set; }
        [DataMember]
        public string Original { get; set; }

        internal static Posters Parse(JToken json) {
            return new Posters() {
                Thumbnail = (string)json["thumbnail"],
                Profile = (string)json["profile"],
                Detailed = (string)json["detailed"],
                Original = (string)json["original"]
            };
        }
    }
}
