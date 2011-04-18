using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class MovieLinks {
        public string Alternate { get; set; }
        public string Cast { get; set; }
        public string Reviews { get; set; }

        internal static MovieLinks Parse(JToken json) {
            return new MovieLinks() {
                Alternate = (string)json["alternate"],
                Cast = (string)json["cast"],
                Reviews = (string)json["reviews"]
            };
        }
    }
}
