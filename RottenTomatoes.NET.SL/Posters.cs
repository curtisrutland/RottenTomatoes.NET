
namespace RottenTomatoes.NET.SL {
    public class Posters {

        public string Thumbnail { get; set; }

        public string Profile { get; set; }

        public string Detailed { get; set; }

        public string Original { get; set; }

        internal Posters(dynamic json) {
            Thumbnail = json.thumbnail;
            Profile = json.profile;
            Detailed = json.detailed;
            Original = json.original;
        }
    }
}
