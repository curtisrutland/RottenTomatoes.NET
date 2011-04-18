using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class Review {
        [DataMember]
        public string Critic { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public string OriginalScore { get; set; }
        [DataMember]
        public string Publication { get; set; }
        [DataMember]
        public string Quote { get; set; }
        [DataMember]
        public string ReviewLink { get; set; }

        internal static Review Parse(JToken json) {
            return new Review() {
                Critic = (string)json["critic"],
                Date = json["date"] == null ? DateTime.MinValue : DateTime.Parse((string)json["date"]),
                OriginalScore = (string)json["original_score"],
                Publication = (string)json["publication"],
                Quote = (string)json["quote"],
                ReviewLink = (string)(json["links"]["review"])
            };
        }
    }
}
