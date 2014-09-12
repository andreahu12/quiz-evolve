using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.DataObjects;

namespace Xamarin.Ecclesia.Parse
{
    public interface IParseData
    {
        #region Users
        UserAccount GetCurrentAccount();
        Task<UserAccount> RegisterAccountAsync(string email, string firstName, string lastName);
        Task<UserAccount> SigInAccountAsync(string email);
        #endregion

        #region Quizzes
        Task<List<Quiz>> GetQuizzesAsync();
        Task<List<QuizQuestion>> GetQuestionsAsync(string quizName);
        #endregion

        #region Scoring
        Task<List<QuestionProgress>> GetProgressesAsync();
        void SaveProgress(QuestionProgress progress);

        Task<List<LeaderboardEntry>> GetMyLeaderboardsAsync();
        Task<List<LeaderboardEntry>> GetQuizLeaderboardsAsync(string quizName);
        void SaveLeaderboard(LeaderboardEntry leaderboard);
        #endregion

        #region Logs
        void LogException(Exception ex);
        void LogMessage(string message, [CallerMemberName] string from = null);
        #endregion
    }
}
