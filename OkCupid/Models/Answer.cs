using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OkCupid
{
    class Answer
    {
        public int questionId { get; set; }
        public int answer { get; set; }
        public List<int> acceptableAnswers { get; set; }
        public int importance { get; set; }

    }
}
