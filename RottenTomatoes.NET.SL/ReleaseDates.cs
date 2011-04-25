using System;

namespace RottenTomatoes.NET.SL {
    public class ReleaseDates {
        
        public DateTime? Theater { get; set; }
        
        public DateTime? Dvd { get; set; }

        internal ReleaseDates(dynamic json) {
            string t = json.theater;
            string d = json.dvd;
            if(t == null)
                Theater = null;
            else
                Theater = DateTime.Parse(t);
            if(d == null)
                Dvd = null;
            else
                Dvd = DateTime.Parse(d);
        }
    }
}
