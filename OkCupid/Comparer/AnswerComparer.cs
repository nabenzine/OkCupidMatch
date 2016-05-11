using System.Collections.Generic;

namespace OkCupid.Comparer
{
    class AnswerComparer : IEqualityComparer<Answer>
    { 
        public bool Equals(Answer x, Answer y)
        {
            return  x.questionId == y.questionId;
        }

        public int GetHashCode(Answer obj)
        {
            return obj.questionId;
        }
    
    }
}
