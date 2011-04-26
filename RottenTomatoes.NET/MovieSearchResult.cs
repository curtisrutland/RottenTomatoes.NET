using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using System.Net;
using System;
using System.Threading.Tasks;
using System.Web;

namespace RottenTomatoes.NET {
    [DataContract]
    public class MovieSearchResult : IEnumerable<Movie> {
        public event EventHandler<EventArgs> GetNextPageCompleted;
        protected virtual void OnGetNextPageCompleted() {
            if (GetNextPageCompleted != null)
                GetNextPageCompleted(this, new EventArgs());
        }

        private int currentPage;
        
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int PageSize { get; set; }
        [DataMember]
        private List<Movie> Movies { get; set; }
        [DataMember]
        public SearchLinks SearchLinks { get; set; }

        public bool HasMorePages {
            get {
                return currentPage * PageSize < Total;
            }
        }

        private MovieSearchResult(){}

        internal MovieSearchResult(string jsonString, int pageSize) {
            PageSize = pageSize;
            currentPage = 1;
            JObject json = JObject.Parse(jsonString);
            Total = json["total"].TryParseInt();
            Movies = json["movies"].Select(x => Movie.Parse(x, isAbbreviated: true)).ToList();
            SearchLinks = SearchLinks.Parse(json["links"], (string)json["link_template"]);
        }

        internal static MovieSearchResult Parse(string jsonString){
            MovieSearchResult res = new MovieSearchResult();
            JObject json = JObject.Parse(jsonString);
            res.Total = json["total"].TryParseInt();
            res.Movies = json["movies"].Select(x => Movie.Parse(x, isAbbreviated: true)).ToList();
            res.SearchLinks = SearchLinks.Parse(json["links"], (string)json["link_template"]);
            return res;
        }

        internal static MovieSearchResult FindMovies(string query, int pageSize) {
            string json = new WebClient().DownloadString(string.Format(Constants.MovieSearchEndpoint, Constants.ApiKey, HttpUtility.UrlEncode(query), pageSize));
            return new MovieSearchResult(json, pageSize);
        }

        public Movie this[int pos] {
            get { return Movies[pos]; }
        }

        public void GetNextPage() {
            if (HasMorePages) {
                string jsonString = new WebClient().DownloadString(string.Format(SearchLinks.Next, Constants.ApiKey));
                var next = Parse(jsonString);
                Movies.AddRange(next.Movies);
                SearchLinks.Next = next.SearchLinks.Next;
                ++currentPage;
            }
        }

        public void GetNextPageAsync() {
            Task.Factory.StartNew(() => {
                GetNextPage();
                OnGetNextPageCompleted();
            });
        }

        #region IEnumerable<Movie> Members

        public IEnumerator<Movie> GetEnumerator() {
            return Movies.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return Movies.GetEnumerator();
        }

        #endregion
    }
}
