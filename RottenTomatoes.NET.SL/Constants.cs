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
    public static class Constants {
        public const string ApiKey = "h6stqn7pb4t95eed8jspy4gw";
        public const string MovieSearchEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey={0}&q={1}&page_limit={2}";
        public const string MovieEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}.json?apikey={0}";
        public const string CastEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}/cast.json?apikey={0}";
        public const string ReviewsEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}/reviews.json?apikey={0}&page={2}&review_type={3}";
        public const string TopCritic = "top_critic";
        public const string Dvd = "dvd";
    }
}
