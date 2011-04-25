using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RottenTomatoes.NET {
    public static class RottenTomatoes {
        public static Movie GetMovie(int id) {
            return Movie.GetMovie(id);
        }

        public static MovieSearchResult FindMovies(string query, int pageSize) {
            return MovieSearchResult.Search(query, pageSize);
        }
    }
}
