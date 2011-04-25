
namespace RottenTomatoes.NET.SL {
    public class MovieLinks {
        public string Alternate { get; set; }
        public string Cast { get; set; }
        public string Reviews { get; set; }

        internal MovieLinks(dynamic json) {
            Alternate = json.alternate;
            Cast = json.cast;
            Reviews = json.reviews;
        }
    }
}
