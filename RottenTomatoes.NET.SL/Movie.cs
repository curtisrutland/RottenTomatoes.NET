using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace RottenTomatoes.NET.SL {
    public class Movie {
        #region Events

        internal event EventHandler<EventArgs> GetMovieCompleted;
        protected virtual void OnGetMovieMovieCompleted() {
            if (GetMovieCompleted != null)
                GetMovieCompleted(this, new EventArgs());
        }

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

        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public List<string> Genres { get; set; }

        public string MPAARating { get; set; }

        public int Runtime { get; set; }

        public ReleaseDates ReleaseDates { get; set; }

        public Ratings Ratings { get; set; }

        public string Synopsis { get; set; }

        public Posters Posters { get; set; }

        public List<CastMember> AbridgedCast { get; set; }

        public List<CastMember> FullCast { get; set; }

        public List<string> AbridgedDirectors { get; set; }

        public MovieLinks Links { get; set; }

        public bool IsAbridged { get; set; }

        public List<Review> TopCriticReviews { get; set; }

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

        #region Constructors and Helpers

        internal Movie() { }

        internal Movie(dynamic json, bool isAbbrev = false) {
            Parse(json, isAbbrev);
        }

        internal void Parse(dynamic json, bool isAbbrev = false) {
            Id = DynamicJsonObject.ParseIntFromDyn(json.id);
            Title = json.title;
            Year = DynamicJsonObject.ParseIntFromDyn(json.year);
            if (isAbbrev) {
                Genres = null;
                AbridgedDirectors = null;
            }
            else {
                Genres = new List<string>();
                foreach (var g in json.genres)
                    Genres.Add(g);
                AbridgedDirectors = new List<string>();
                foreach (var ad in json.abridged_directors)
                    AbridgedDirectors.Add(ad.name);
            }
            MPAARating = json.mpaa_rating;
            Runtime = DynamicJsonObject.ParseIntFromDyn(json.runtime);
            ReleaseDates = new ReleaseDates(json.release_dates);
            Ratings = new Ratings(json.ratings);
            Synopsis = json.synopsis;
            Posters = new Posters(json.posters);
            AbridgedCast = new List<CastMember>();
            foreach (var ac in json.abridged_cast)
                AbridgedCast.Add(new CastMember(ac));
            Links = new MovieLinks(json.links);
            IsAbridged = isAbbrev;
        }

        internal void GetMovieAsync(int id) {
            WebClient wc = new WebClient();
            DownloadStringCompletedEventHandler f = null;
            f = (s, ea) => {
                wc.DownloadStringCompleted -= f;
                dynamic json = DynamicJsonObject.Parse(ea.Result);
                Parse(json);
                OnGetMovieMovieCompleted();
            };
            wc.DownloadStringCompleted += f;
            wc.DownloadStringAsync(new Uri(string.Format(Constants.MovieEndpoint, Constants.ApiKey, id)));
        }

        #endregion

        #region Public Methods

        public void LoadUnabridgedAsync() {
            EventHandler<EventArgs> f = null;
            f = (s, ea) => {
                GetMovieCompleted -= f;
                OnLoadUnabridgedCompleted();
            };
            GetMovieCompleted += f;
            GetMovieAsync(this.Id);
        }

        public void LoadFullCastAsync() {
            WebClient wc = new WebClient();
            DownloadStringCompletedEventHandler f = null;
            f = (s, ea) => {
                wc.DownloadStringCompleted -= f;
                dynamic json = DynamicJsonObject.Parse(ea.Result);
                FullCast = new List<CastMember>();
                foreach (var cast in json.cast)
                    FullCast.Add(new CastMember(cast));
                OnLoadFullCastCompleted();
            };
            wc.DownloadStringCompleted += f;
            wc.DownloadStringAsync(new Uri(string.Format(Constants.CastEndpoint, Constants.ApiKey, Id)));
        }

        public void LoadTopCriticReviewsAsync() {
            if (HasMoreTopCriticReviews) {
                WebClient wc = new WebClient();
                DownloadStringCompletedEventHandler f = null;
                f = (s, ea) => {
                    wc.DownloadStringCompleted -= f;
                    dynamic json = DynamicJsonObject.Parse(ea.Result);
                    topCriticReviewTotal = DynamicJsonObject.ParseIntFromDyn(json.total);
                    if (TopCriticReviews == null)
                        TopCriticReviews = new List<Review>();
                    foreach (var rev in json.reviews)
                        TopCriticReviews.Add(new Review(rev));
                    OnLoadReviewsCompleted();
                };
                wc.DownloadStringCompleted += f;
                wc.DownloadStringAsync(new Uri(nextTopCriticReviewLink));
            }
            else
                OnLoadReviewsCompleted();
        }

        public void LoadDvdReviewsAsync() {
            if (HasMoreDvdReviews) {
                WebClient wc = new WebClient();
                DownloadStringCompletedEventHandler f = null;
                f = (s, ea) => {
                    wc.DownloadStringCompleted -= f;
                    dynamic json = DynamicJsonObject.Parse(ea.Result);
                    dvdReviewTotal = DynamicJsonObject.ParseIntFromDyn(json.total);
                    if (DvdReviews == null)
                        DvdReviews = new List<Review>();
                    foreach (var rev in json.reviews)
                        DvdReviews.Add(new Review(rev));
                    OnLoadReviewsCompleted();
                };
                wc.DownloadStringCompleted += f;
                wc.DownloadStringAsync(new Uri(nextDvdReviewLink));
            }
            else
                OnLoadReviewsCompleted();
        }

        #endregion
    }
}
