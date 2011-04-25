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
using System.Windows.Browser;

namespace RottenTomatoes.NET.SL {
    public class MovieSearchResult {
        public event EventHandler<EventArgs> GetNextPageCompleted;
        protected virtual void OnGetNextPageCompleted() {
            if (GetNextPageCompleted != null)
                GetNextPageCompleted(this, new EventArgs());
        }

        public event EventHandler<EventArgs> FindMoviesCompleted;
        protected virtual void OnFindMoviesCompleted() {
            if (FindMoviesCompleted != null)
                FindMoviesCompleted(this, new EventArgs());
        }

        private int currentPage;

        public int Total { get; set; }
        public int PageSize { get; set; }
        public List<Movie> Movies { get; set; }
        public SearchLinks SearchLinks { get; set; }

        public bool HasMorePages {
            get {
                return currentPage * PageSize < Total;
            }
        }

        internal MovieSearchResult() { }

        internal MovieSearchResult(string jsonString, int pageSize) {
            Parse(jsonString, pageSize);
        }

        private void Parse(string jsonString, int pageSize) {
            PageSize = pageSize;
            currentPage = 1;
            dynamic json = DynamicJsonObject.Parse(jsonString);
            Total = DynamicJsonObject.ParseIntFromDyn(json.total);
            Movies = new List<Movie>();
            foreach (var m in json.movies)
                Movies.Add(new Movie(m, true));
            SearchLinks = new SearchLinks(json.links, json.link_template);
        }

        internal void FindMoviesAsync(string query, int pageSize) {
            var uri = new Uri(string.Format(Constants.MovieSearchEndpoint, Constants.ApiKey, HttpUtility.UrlEncode(query), pageSize));
            WebClient wc = new WebClient();
            DownloadStringCompletedEventHandler f = null;
            f = (s, ea) => {
                wc.DownloadStringCompleted -= f;
                Parse(ea.Result, pageSize);
                OnFindMoviesCompleted();
            };
            wc.DownloadStringCompleted += f;
            wc.DownloadStringAsync(uri);
        }

        public void GetNextPageAsync() {
            if (HasMorePages) {
                var uri = new Uri(string.Format(SearchLinks.Next, Constants.ApiKey));
                WebClient wc = new WebClient();
                DownloadStringCompletedEventHandler f = null;
                f = (s, ea) => {
                    wc.DownloadStringCompleted -= f;
                    dynamic json = DynamicJsonObject.Parse(ea.Result);
                    if (Movies == null)
                        Movies = new List<Movie>();
                    foreach (var m in json.movies)
                        Movies.Add(new Movie(m, true));
                    SearchLinks.Next = new SearchLinks(json.links, json.link_template).Next;
                    ++currentPage;
                    OnGetNextPageCompleted();
                };
                wc.DownloadStringCompleted += f;
                wc.DownloadStringAsync(uri);
            }
            else {
                OnGetNextPageCompleted();
            }
        }
    }
}
