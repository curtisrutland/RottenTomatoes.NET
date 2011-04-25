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
    public class SearchLinks {
        public string Self { get; set; }
        public string Next { get; set; }
        public string SearchTemplate { get; set; }

        internal SearchLinks(dynamic linksJson, string linkTemplate) {
            Self = linksJson.self + "&apikey={0}";
            Next = linksJson.next + "&apikey={0}";
            SearchTemplate = linkTemplate;
        }
    }
}
