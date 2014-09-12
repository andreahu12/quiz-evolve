using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.DataObjects
{
    public class UserAccount
    {
        #region Fields
        List<QuestionProgress> _progresses { get; set; }
        List<LeaderboardEntry> _leaderboards { get; set; }
        object _lockObj = new object();

        #endregion

        #region Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
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

        public LeaderboardEntry GetLeaderboardForQuiz(string quizName)
        {
            if (_leaderboards == null)
            {
                _leaderboards = new List<LeaderboardEntry>();
            }
            var leaderboard = _leaderboards.FirstOrDefault(f => f.QuizName == quizName);
            if (leaderboard == null)
            {
                leaderboard = new LeaderboardEntry();
                leaderboard.QuizName = quizName;
                _leaderboards.Add(leaderboard);
            }
            return leaderboard;
        }

        public int GetQuizScore(string quizName)
        {
            if (_progresses == null)
                return 0;
            var leaderboard = GetLeaderboardForQuiz(quizName);
            leaderboard.Score = _progresses.Where(f => f.QuizName == quizName).Sum(s => s.Score);
            return leaderboard.Score;
        }

        public bool IsQuizCompleted(string quizName)
        {
            if (_progresses == null)
                return false;
            return !_progresses.Any(f => f.Answers == 0);
        }

        public async void UpdateProgress()
        {
            var rv = await ParseHelper.ParseData.GetProgressesAsync();
            if (_progresses == null)
                _progresses = rv;
            else
            {
                foreach (var progress in rv)
                {
                    var oldValue = _progresses.FirstOrDefault(f => f.QuestionID == progress.QuestionID);
                    if (oldValue == null)
                        _progresses.Add(progress);
                    else
                    {
                        oldValue.IsAnswered = progress.IsAnswered;
                        oldValue.TimeElapsed = progress.TimeElapsed;
                        oldValue.AnswerOn = progress.AnswerOn;
                        oldValue.Answers = progress.Answers;
                    }
                }
            }
        }

        public async void UpdateLeaderboards()
        {
            var rv = await ParseHelper.ParseData.GetMyLeaderboardsAsync();
            if (_leaderboards == null)
                _leaderboards = rv;
            else
            {
                foreach (var leaderboard in rv)
                {
                    var oldValue = _leaderboards.FirstOrDefault(f => f.QuizName == leaderboard.QuizName);
                    if (oldValue == null)
                        _leaderboards.Add(leaderboard);
                    else
                        oldValue.Score = leaderboard.Score;
                }
            }
        }
        #endregion
    }
}
