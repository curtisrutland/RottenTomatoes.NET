
namespace RottenTomatoes.NET.SL {
    public class Ratings {
        public int CriticsScore { get; set; }
        public int AudienceScore { get; set; }

        internal Ratings (dynamic json) {
            AudienceScore = DynamicJsonObject.ParseIntFromDyn(json.audience_score);
            CriticsScore = DynamicJsonObject.ParseIntFromDyn(json.critics_score);
        }
    }
}
