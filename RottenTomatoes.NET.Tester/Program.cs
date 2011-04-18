﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RottenTomatoes.NET.Tester {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Searching.");
            var res = RottenTomatoes.Search("cell", 10);
            var mov = res.Movies.First();
            mov.LoadTopCriticReviews();
            mov.LoadDvdReviews();
            mov.LoadDvdReviews();
            Console.ReadKey();
        }
    }
}