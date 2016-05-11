using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkCupid
{
    class Profile
    {
        public int id { get; set; }
        public List<Answer> answers { get; set; }
        public List<Match> matches = new List<Match>();

        public List<Answer> getAnswers()
        {
            return answers;
        }

        public List<Match> getListMatches()
        {
            return matches;
        }

        public void AddMatch(Match match)
        {
            matches.Add(match);
        }
    }
}
