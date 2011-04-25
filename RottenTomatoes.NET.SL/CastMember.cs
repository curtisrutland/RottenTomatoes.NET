using System.Collections.Generic;

namespace RottenTomatoes.NET.SL {
    public class CastMember {

        string Name { get; set; }

        public List<string> Characters { get; set; }

        internal CastMember(dynamic json) {
            Name = json.name;
            if (json.characters == null)
                Characters = null;
            else {
                Characters = new List<string>();
                foreach (var cast in json.characters)
                    Characters.Add(cast);
            }
        }
    }
}
