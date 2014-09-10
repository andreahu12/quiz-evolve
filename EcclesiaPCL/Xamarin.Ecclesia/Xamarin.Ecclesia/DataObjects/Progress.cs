using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Ecclesia.DataObjects
{
    public class QuestionProgress
    {
		const int MaxPossibleScore = 40;
		const int MaxTime = 30;

        public string ID { get; set; }
        public string QuestionID { get; set; }
        public int TimeElapsed { get; set; }
		public int WrongAnswers { get; set; }

        public int TimeRemaining
        {
            get
            { return MaxTime - TimeElapsed; }
        }

		public int Score
		{
			get
			{
				return MaxPossibleScore - TimeElapsed;
			}
		}
    }
}
