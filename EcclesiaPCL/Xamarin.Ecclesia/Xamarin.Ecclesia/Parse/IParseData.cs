using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.DataObjects;
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
    }
}
