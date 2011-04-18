using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace RottenTomatoes.NET {
    [DataContract]
    public class SearchLinks {
        public string Self { get; set; }
        public string Next { get; set; }
        public string SearchTemplate { get; set; }

        internal static SearchLinks Parse(JToken linksJson, string linkTemplate) {
            return new SearchLinks() {
                Self = (string)linksJson["self"] + "&apikey={0}",
                Next = (string)linksJson["next"] + "&apikey={0}",
                SearchTemplate = linkTemplate
            };
        }
    }
}
