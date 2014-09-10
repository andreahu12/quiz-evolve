using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Ecclesia.DataObjects
{
    public class UserAccount
    {
        #region Fields
        List<QuestionProgress> _progresses { get; set; }
        #endregion

        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string ID { get; set; }
        #endregion

        #region Methods

        /// <summary>
        /// Returns progress for question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public QuestionProgress GetProgressForQuestion(QuizQuestion question)
        {
            if (_progresses == null)
            {
                _progresses = new List<QuestionProgress>();
            }
            var progress = _progresses.FirstOrDefault(f => f.QuestionID == question.ID);
            if (progress == null)
            {
                progress = new QuestionProgress(question);
                _progresses.Add(progress);
            }
            return progress;
        }

        public int GetQuizScore(string quizName)
        {
            if (_progresses == null)
                return 0;
            return _progresses.Where(f => f.QuizName == quizName).Sum(s => s.Score);
        }
        #endregion
    }
}
