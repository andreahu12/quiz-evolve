using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.DataObjects;
using Parse;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.DataObjects;

namespace Xamarin.Ecclesia.Parse
{
    public class ParseData:IParseData
    {
        const string sharedPass = "1@sd%gR43";
        #region Accounts
        public UserAccount GetCurrentAccount()
        {
            if (ParseUser.CurrentUser != null)
            {
                return AccountFromParseUser(ParseUser.CurrentUser);
            }
            return null;
        }

        public async Task<UserAccount> SigInAccountAsync(string email)
        {
            var user = await ParseUser.LogInAsync(email,sharedPass);
            SaveLocal(email);
            return AccountFromParseUser(user);
        }
        
        public async Task<UserAccount> RegisterAccountAsync(string email, string firstName, string lastName)
        {
            bool isRegistered = false;

            var user = new ParseUser()
            {
                Username = email,
                Email = email,
                Password = sharedPass,
            };

            user["first_name"] = firstName;
            user["last_name"] = lastName;
            try
            {
                await user.SignUpAsync();
                SaveLocal(email);
            }
            catch (ParseException p)
            {
                //TODO: Log it
                isRegistered = true;
            }
            catch (Exception a)
            {
                //TODO: Log it
				var t = a.Message;
            }
            
            if (isRegistered)
                return await SigInAccountAsync(email);
            else
                return AccountFromParseUser(user);
        }

        UserAccount AccountFromParseUser(ParseUser parseUser)
        {
            var account = new UserAccount();
            account.FirstName = parseUser["first_name"].ToString();
            account.LastName = parseUser["last_name"].ToString();
            account.ID = parseUser.ObjectId;
            return account;
        }

        void SaveLocal(string email)
        {
            AppSettings.AccountEmail = email;
            //AppSettings.AccountID = socialId;
        }
        #endregion
        
        #region Quizzes
        public async Task<List<Quiz>> GetQuizzesAsync()
        {
            var query = ParseObject.GetQuery("Quiz").OrderBy("name");
            var objects = await query.FindAsync();

            var rv = new List<Quiz>();
            foreach (var t in objects)
            {
                rv.Add(QuizFromParseObject(t));
            }

            return rv;
        }

        public async Task<List<QuizQuestion>> GetQuestionsAsync(string quizName)
        {
            var query = ParseObject.GetQuery(quizName).OrderBy("Number");
            var objects = await query.FindAsync();

            var rv = new List<QuizQuestion>();
            foreach (var t in objects)
            {
                rv.Add(QuestionFromParseObject(t));
            }

            return rv;
        }


        Quiz QuizFromParseObject(ParseObject parseObject)
        {
            var quiz = new Quiz();
            quiz.ID = parseObject.ObjectId;
            quiz.Name = parseObject["name"].ToString();
            quiz.Description = parseObject["description"].ToString();
            return quiz;
        }

        QuizQuestion QuestionFromParseObject(ParseObject parseObject)
        {
            var question = new QuizQuestion();
            question.ID = parseObject.ObjectId;
            question.Question = parseObject["Question"].ToString();
            question.CorrectAnswerID = Convert.ToInt32(parseObject["SolutionNum"]);
            question.AnswerA = parseObject["A"].ToString();
            question.AnswerB = parseObject["B"].ToString();
            question.AnswerC = parseObject["C"].ToString();
            question.AnswerD = parseObject["D"].ToString();
            return question;
        }
        #endregion
    }
}