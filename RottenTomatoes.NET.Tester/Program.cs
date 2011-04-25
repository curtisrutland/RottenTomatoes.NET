using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RottenTomatoes.NET.Tester {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Searching.");
            var res = RottenTomatoes.FindMovies("cell", 10);
            var mov = res.Movies.First();
            mov.LoadUnabridged();
            mov.LoadTopCriticReviews();
            mov.LoadDvdReviews();
            mov.LoadFullCast();
            Console.ReadKey();
        }
    }
}
