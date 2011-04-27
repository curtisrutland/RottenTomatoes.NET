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
using System.Windows.Navigation;
using RottenTomatoes.NET.SL;
using System.Windows.Media.Imaging;

namespace SilverTomatoes.Views {
    public partial class Detail : Page {
        Movie movie;
        RottenTomatoe rt;

        public Detail() {
            InitializeComponent();
            rt = new RottenTomatoe();
            rt.GetMovieCompleted += new EventHandler<ResultEventArgs<Movie>>(rt_GetMovieCompleted);
        }

        void rt_GetMovieCompleted(object sender, ResultEventArgs<Movie> e) {
            movie = e.Result;
            titleTextBlock.Text = movie.Title;
            yearTextBlock.Text = movie.Year.ToString();
            posterImage.Source = new BitmapImage(new Uri(movie.Posters.Original, UriKind.Absolute));
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (NavigationContext.QueryString.ContainsKey("id"))
                rt.GetMovieAsync(int.Parse(NavigationContext.QueryString["id"]));
        }

    }
}
