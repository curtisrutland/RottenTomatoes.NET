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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using RottenTomatoes.NET.SL;
using System.Windows.Browser;

namespace SilverTomatoes {
    public partial class Home : Page {
        const string SEARCHING = "Searching...";
        const string READY = "Search will start when you stop typing.";
        const string WAITING = "Begin typing to search for movies.";
        const string NORESULTS = "No movies were found.";

        private string loadingText {
            get { return ContentText.Text; }
            set { ContentText.Text = value; }
        }

        private DispatcherTimer timer;
        RottenTomatoe rt;

        public Home() {
            InitializeComponent();
            rt = new RottenTomatoe();
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            rt.FindMoviesCompleted += new EventHandler<ResultEventArgs<MovieSearchResult>>(rt_FindMoviesCompleted);
            timer.Tick += new EventHandler(timer_Tick);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (NavigationContext.QueryString.ContainsKey("c")) {
                int i;
                if (int.TryParse(NavigationContext.QueryString["c"], out i))
                    pageCountUpDown.Value = i;
            }
            if (NavigationContext.QueryString.ContainsKey("q")) {
                queryTextBox.Text = NavigationContext.QueryString["q"];
                DoSearch();
            }
        }

        private void DoSearch() {
            resultListBox.SelectionChanged -= resultListBox_SelectionChanged;
            loadingText = SEARCHING;
            rt.FindMoviesAsync(queryTextBox.Text, (int)pageCountUpDown.Value);
        }

        private void queryTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            loadingText = READY;
            if (timer.IsEnabled)
                timer.Stop();
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e) {
            timer.Stop();
            string url = string.Format("/Home/{0}/{1}", queryTextBox.Text, pageCountUpDown.Value);
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        void rt_FindMoviesCompleted(object sender, ResultEventArgs<MovieSearchResult> e) {
            if (e.Result.Total != 0)
                loadingText = WAITING;
            else
                loadingText = NORESULTS;
            resultListBox.ItemsSource = e.Result;
            resultListBox.SelectionChanged += resultListBox_SelectionChanged;
        }

        private void resultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int id = (resultListBox.SelectedItem as Movie).Id;
            string url = "/Detail/" + id;
            NavigationService.Navigate(new Uri(url, UriKind.Relative));
        }

        private void Page_GotFocus(object sender, RoutedEventArgs e) {
            queryTextBox.Focus();
        }
    }
}