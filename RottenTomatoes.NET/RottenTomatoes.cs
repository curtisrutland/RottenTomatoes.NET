using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RottenTomatoes.NET {
    public static class RottenTomatoes {
        public static Movie GetById(int id) {
            return Movie.GetById(id);
        }

        public static MovieSearchResult Search(string query, int pageSize) {
            return MovieSearchResult.Search(query, pageSize);
        }
    }
}
