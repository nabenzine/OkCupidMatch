using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkCupid
{
    class Result
    {
        public int profileId { get; set; }
        public List<Match> matches { get; set; }

        public Result(int profileId, List<Match> matches)
        {
            this.profileId = profileId;
            this.matches = matches;
        }
    }
}
