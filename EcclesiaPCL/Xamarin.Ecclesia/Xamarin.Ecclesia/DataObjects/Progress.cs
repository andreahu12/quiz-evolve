using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.DataObjects
{
    public class QuestionProgress
    {
        #region Constants
        const int MaxPossibleScore = 40;
		const int MaxTime = 30;
        #endregion

        #region Constructor
        public QuestionProgress(QuizQuestion question)
        {
            QuestionID = question.ID;
            QuizName = question.QuizName;
        }

        public QuestionProgress()
        {
        }
        #endregion

        #region Properties
        public string ID { get; set; }
        public string QuestionID { get; set; }
        public int TimeElapsed { get; set; }
		public int Answers { get; set; }
        public int AnswerOn { get; set; }
        public string QuizName { get; set; }
        public int TimeRemaining
        {
            get
            {
                var rv = MaxTime - TimeElapsed;
                if (rv < 0)
                    return 0;
                return rv;
            }
        }

        public bool IsAnswered { get; set; } 

		public bool IsLocked
		{
			get
			{
				return Answers > 0;
			}
		}

		public int Score
		{
			get
			{
                if (!IsAnswered || TimeRemaining < 1)
                    return 0;
				return MaxPossibleScore - TimeElapsed;
			}
        }
        #endregion

        #region Methods
        public void SetAnswer(bool isAnswered)
        {
            IsAnswered = isAnswered;
            Answers++;
            AnswerOn = TimeElapsed;

            ParseHelper.ParseData.SaveProgress(this);
        }
        #endregion
    }
}
