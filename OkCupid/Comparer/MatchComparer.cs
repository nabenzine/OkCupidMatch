using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkCupid.Comparer
{
    class MatchComparer : Comparer<Match>
    {
        public override int Compare(Match x, Match y)
        {
            // needs null checks
            var referenceEquals = ReferenceEquals(x, y);
            if (referenceEquals)
            {
                return 0;
            }
            //return Comparer<double>.Default.Compare(x.score, y.score);
            return y.score.CompareTo(x.score);
        }
    }
}
