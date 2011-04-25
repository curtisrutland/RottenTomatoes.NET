using System;
using System.Dynamic;

namespace RottenTomatoes.NET.SL {
    public class Review {

        public string Critic { get; set; }

        public DateTime Date { get; set; }

        public string OriginalScore { get; set; }

        public string Publication { get; set; }

        public string Quote { get; set; }

        public string ReviewLink { get; set; }

        internal Review(dynamic json) {
            Critic = json.critic;
            Date = json.date == null ? DateTime.MinValue : DateTime.Parse(json.date);
            OriginalScore = json.original_score ?? string.Empty;
            Publication = json.publication;
            Quote = json.quote;
            ReviewLink = json.links.review;
        }
    }
}
