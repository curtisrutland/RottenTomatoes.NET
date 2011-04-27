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

namespace RottenTomatoes.NET.SL {
    public class RottenTomatoe {
        public event EventHandler<ResultEventArgs<Movie>> GetMovieCompleted;
        protected virtual void OnGetMovieCompleted(Movie m) {
            if (GetMovieCompleted != null)
                GetMovieCompleted(this, new ResultEventArgs<Movie>(m));
        }

        public event EventHandler<ResultEventArgs<MovieSearchResult>> FindMoviesCompleted;
        protected virtual void OnFindMoviesCompleted(MovieSearchResult ms) {
            if (FindMoviesCompleted != null)
                FindMoviesCompleted(this, new ResultEventArgs<MovieSearchResult>(ms));
        }


        public void GetMovieAsync(int id) {
            Movie m = new Movie();
            EventHandler<EventArgs> f = null;
            f = (s, ea) => {
                m.GetMovieCompleted -= f;
                OnGetMovieCompleted(m);
            };
            m.GetMovieCompleted += f;
            m.GetMovieAsync(id);
        }

        public void FindMoviesAsync(string query, int pageSize) {
            MovieSearchResult ms = new MovieSearchResult();
            EventHandler<EventArgs> f = null;
            f = (s, ea) => {
                ms.FindMoviesCompleted -= f;
                OnFindMoviesCompleted(ms);
            };
            ms.FindMoviesCompleted += f;
            ms.FindMoviesAsync(query, pageSize);
        }
    }
}
