using System;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class ReleaseDates {
        [DataMember]
        public DateTime? Theater { get; set; }
        [DataMember]
        public DateTime? Dvd { get; set; }

        internal static ReleaseDates Parse(JToken json) {
            string t = (string)json["theater"];
            string d = (string)json["dvd"];
            return new ReleaseDates() {
                Theater = t == null ? default(DateTime?) : DateTime.Parse(t),
                Dvd = d == null ? default(DateTime?) : DateTime.Parse(d),
            };
        }
    }
}
