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
using System.Windows.Threading;
using RT = RottenTomatoes.NET.SL;

namespace SilverTomatoes {
    public partial class MainPage : UserControl {
        private RT.RottenTomatoes rt;
        private DispatcherTimer timer;
        private bool _loading;
        private bool loading {
            get {
                return _loading;
            }
            set {
                _loading = value;
                loadingBusyIndicator.IsBusy = value;
            }
        }

        public MainPage() {
            InitializeComponent();
            timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            rt = new RT.RottenTomatoes();
            rt.FindMoviesCompleted += new EventHandler<RT.ResultEventArgs<RT.MovieSearchResult>>(rt_FindMoviesCompleted);
            timer.Tick += new EventHandler(timer_Tick);
        }

        private void queryTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            if (timer.IsEnabled) {
                timer.Stop();
            }
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e) {
            loading = true;
            rt.FindMoviesAsync(queryTextBox.Text, (int)pageSizeUpDown.Value);
            timer.Stop();
        }

        void rt_FindMoviesCompleted(object sender, RT.ResultEventArgs<RT.MovieSearchResult> e) {
            resultDataGrid.ItemsSource = e.Result;
            loading = false;
        }
    }
}
