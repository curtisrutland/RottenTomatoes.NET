using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace RottenTomatoes.NET {
    public static class Constants {
        private static string apiKey = null;
        internal static string ApiKey {
            get {
                if (apiKey == null)
                    apiKey = ConfigurationManager.AppSettings["apiKey"];
                return apiKey;
            }
        }
        public const string MovieSearchEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies.json?apikey={0}&q={1}&page_limit={2}";
        public const string MovieEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}.json?apikey={0}";
        public const string CastEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}/cast.json?apikey={0}";
        public const string ReviewsEndpoint = "http://api.rottentomatoes.com/api/public/v1.0/movies/{1}/reviews.json?apikey={0}&page={2}&review_type={3}";
        public const string TopCritic = "top_critic";
        public const string Dvd = "dvd";
    }
}
