using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class Ratings {
        [DataMember]
        public int CriticsScore { get; set; }
        [DataMember]
        public int AudienceScore { get; set; }

        internal static Ratings Parse(JToken json) {
            int c = json["critics_score"].TryParseInt();
            int a = json["audience_score"].TryParseInt();
            return new Ratings() { AudienceScore = a, CriticsScore = c };
        }
    }
}
