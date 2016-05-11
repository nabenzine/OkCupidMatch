using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkCupid
{
    class Match
    {
        public int profileId { get; set; }
        public double score { get; set; }

        public Match(int profileId, double score)
        {
            this.profileId = profileId;
            this.score = score;
        }
    }
}
