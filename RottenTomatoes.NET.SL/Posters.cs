
namespace RottenTomatoes.NET.SL {
    public class Posters {

        const string DEFAULT_POSTER_URL = "http://images.rottentomatoescdn.com/images/redesign/poster_default.gif";
        const string CORRECT_DEFAULT = "Images/poster_default.png";

        public string Thumbnail { get; set; }

        public string Profile { get; set; }

        public string Detailed { get; set; }

        public string Original { get; set; }

        internal Posters(dynamic json) {
            Thumbnail = GetPosterName(json.thumbnail);
            Profile = GetPosterName(json.profile);
            Detailed = GetPosterName(json.detailed);
            Original = GetPosterName(json.original);
        }

        private string GetPosterName(string p) {
            if (p.ToLower() == DEFAULT_POSTER_URL.ToLower()) {
                return CORRECT_DEFAULT;
            }
            else return p;
        }
    }
}
