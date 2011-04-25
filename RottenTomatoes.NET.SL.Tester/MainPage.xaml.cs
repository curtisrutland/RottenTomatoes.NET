using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RottenTomatoes.NET.SL.Tester {
    public partial class MainPage : UserControl {
        public MainPage() {
            InitializeComponent();
            RottenTomatoes rt = new RottenTomatoes();
            rt.FindMoviesCompleted += new EventHandler<ResultEventArgs<MovieSearchResult>>(rt_FindMoviesCompleted);
            rt.FindMoviesAsync("cell", 10);
        }

        void rt_FindMoviesCompleted(object sender, ResultEventArgs<MovieSearchResult> e) {
            var m = e.Result.Movies.First();
            m.LoadFullCastCompleted += new EventHandler<EventArgs>(m_LoadFullCastCompleted);
            m.LoadReviewsCompleted += new EventHandler<EventArgs>(m_LoadReviewsCompleted);
            m.LoadUnabridgedCompleted += new EventHandler<EventArgs>(m_LoadUnabridgedCompleted);
            m.LoadUnabridgedAsync(); 
            m.LoadFullCastAsync();
            m.LoadTopCriticReviewsAsync();
            m.LoadDvdReviewsAsync();
        }

        void m_LoadUnabridgedCompleted(object sender, EventArgs e) {
            var m = (sender as Movie);
        }

        void m_LoadReviewsCompleted(object sender, EventArgs e) {
            var m = (sender as Movie);
        }

        void m_LoadFullCastCompleted(object sender, EventArgs e) {
            var m = (sender as Movie);
        }
    }
}
