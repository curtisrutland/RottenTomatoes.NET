using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Web;
using System.Threading.Tasks;

namespace RottenTomatoes.NET {
    [DataContract]
    public class Movie {
        #region Events
        public event EventHandler<EventArgs> LoadFullCastCompleted;
        protected virtual void OnLoadFullCastCompleted() {
            if (LoadFullCastCompleted != null)
                LoadFullCastCompleted(this, new EventArgs());
        }

        public event EventHandler<EventArgs> LoadUnabridgedCompleted;
        protected virtual void OnLoadUnabridgedCompleted() {
            if (LoadUnabridgedCompleted != null)
                LoadUnabridgedCompleted(this, new EventArgs());
        }

        public event EventHandler<EventArgs> LoadReviewsCompleted;
        protected virtual void OnLoadReviewsCompleted() {
            if (LoadReviewsCompleted != null)
                LoadReviewsCompleted(this, new EventArgs());
        }
        #endregion

        #region Private Fields
        private string nextTopCriticReviewLink { get { return string.Format(Constants.ReviewsEndpoint, Constants.ApiKey, Id, topCriticReviewPage + 1, Constants.TopCritic); } }
        private int topCriticReviewTotal;
        private int topCriticReviewPage = 0;

        private string nextDvdReviewLink { get { return string.Format(Constants.ReviewsEndpoint, Constants.ApiKey, Id, dvdReviewPage + 1, Constants.Dvd); } }
        private int dvdReviewTotal;
        private int dvdReviewPage = 0;
        #endregion

        #region Properties
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public List<string> Genres { get; set; }
        [DataMember]
        public string MPAARating { get; set; }
        [DataMember]
        public int Runtime { get; set; }
        [DataMember]
        public ReleaseDates ReleaseDates { get; set; }
        [DataMember]
        public Ratings Ratings { get; set; }
        [DataMember]
        public string Synopsis { get; set; }
        [DataMember]
        public Posters Posters { get; set; }
        [DataMember]
        public List<CastMember> AbridgedCast { get; set; }
        [DataMember]
        public List<CastMember> FullCast { get; set; }
        [DataMember]
        public List<string> AbridgedDirectors { get; set; }
        [DataMember]
        public MovieLinks Links { get; set; }
        [DataMember]
        public bool IsAbridged { get; set; }
        [DataMember]
        public List<Review> TopCriticReviews { get; set; }
        [DataMember]
        public List<Review> DvdReviews { get; set; }

        public bool HasMoreTopCriticReviews {
            get {
                if (topCriticReviewPage == 0)
                    return true;
                if (topCriticReviewPage * 20 < topCriticReviewTotal)
                    return true;
                return false;
            }
        }

        public bool HasMoreDvdReviews {
            get {
                if (dvdReviewPage == 0)
                    return true;
                if (dvdReviewPage * 20 < dvdReviewTotal)
                    return true;
                return false;
            }
        }
        #endregion

        #region Static Methods
        internal static Movie GetById(int id) {
            string json = new WebClient().DownloadString(string.Format(Constants.MovieEndpoint, Constants.ApiKey, id));
            return Movie.Parse(json);
        }

        internal static Movie Parse(string jsonString) {
            JObject json = JObject.Parse(jsonString);
            return Movie.Parse(json);
        }

        internal static Movie Parse(JToken json, bool isAbbreviated = false) {
            Movie mov = new Movie();
            mov.Id = json["id"].TryParseInt();
            mov.Title = (string)json["title"];
            mov.Year = json["year"].TryParseInt();
            mov.Genres = isAbbreviated ? null : json["genres"].Select(x => (string)x).ToList();
            mov.MPAARating = (string)json["mpaa_rating"];
            mov.Runtime = json["runtime"].TryParseInt();
            mov.ReleaseDates = ReleaseDates.Parse(json["release_dates"]);
            mov.Ratings = Ratings.Parse(json["ratings"]);
            mov.Synopsis = (string)json["synopsis"];
            mov.Posters = Posters.Parse(json["posters"]);
            mov.AbridgedCast = json["abridged_cast"].Select(x => CastMember.Parse(x)).ToList();
            mov.AbridgedDirectors = isAbbreviated ? null : json["abridged_directors"].Select(x => (string)x["name"]).ToList();
            mov.Links = MovieLinks.Parse(json["links"]);
            mov.IsAbridged = isAbbreviated;
            return mov;
        }
        #endregion

        #region Public Methods
        public void LoadUnabridged() {
            var unabridged = Movie.GetById(Id);
            IsAbridged = false;
            AbridgedDirectors = unabridged.AbridgedDirectors;
            Genres = unabridged.Genres;
            MPAARating = unabridged.MPAARating;
        }

        public void LoadUnabridgedAsync() {
            Task.Factory.StartNew(() => {
                LoadUnabridged();
                OnLoadUnabridgedCompleted();
            });
        }

        public void LoadFullCast() {
            WebClient wc = new WebClient();
            string jsonStr = wc.DownloadString(string.Format(Constants.CastEndpoint, Constants.ApiKey, Id));
            JObject json = JObject.Parse(jsonStr);
            FullCast = json["cast"].Select(x => CastMember.Parse(x)).ToList();
        }

        public void LoadFullCastAsync() {
            Task.Factory.StartNew(() => {
                LoadFullCast();
                OnLoadFullCastCompleted();
            });
        }

        public void LoadTopCriticReviews() {
            if (HasMoreTopCriticReviews) {
                var jsonStr = new WebClient().DownloadString(nextTopCriticReviewLink);
                JObject json = JObject.Parse(jsonStr);
                topCriticReviewTotal = json["total"].TryParseInt();
                if (TopCriticReviews == null)
                    TopCriticReviews = new List<Review>();
                TopCriticReviews.AddRange(json["reviews"].Select(x => Review.Parse(x)).ToList());
                ++topCriticReviewPage;
            }
        }

        public void LoadTopCriticReviewsAsync() {
            Task.Factory.StartNew(() => {
                LoadTopCriticReviews();
                OnLoadReviewsCompleted();
            });
        }

        public void LoadDvdReviews() {
            if (HasMoreTopCriticReviews) {
                var jsonStr = new WebClient().DownloadString(nextDvdReviewLink);
                JObject json = JObject.Parse(jsonStr);
                dvdReviewTotal = json["total"].TryParseInt();
                if (DvdReviews == null)
                    DvdReviews = new List<Review>();
                DvdReviews.AddRange(json["reviews"].Select(x => Review.Parse(x)).ToList());
                ++dvdReviewPage;
            }
        }

        public void LoadDvdReviewsAsync() {
            Task.Factory.StartNew(() => {
                LoadTopCriticReviews();
                OnLoadReviewsCompleted();
            });
        }

        #endregion
    }
}
