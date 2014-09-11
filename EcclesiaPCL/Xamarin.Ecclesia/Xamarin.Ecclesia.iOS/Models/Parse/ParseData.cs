using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.DataObjects;
using Parse;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Settings;
using System.Runtime.CompilerServices;

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
            try
            {
                var user = await ParseUser.LogInAsync(email, sharedPass);
                SaveLocal(email);
                return AccountFromParseUser(user);
            }
            catch (Exception ex)
            {
                ParseHelper.ParseData.LogException(ex);
                SaveLocal("");
                return null;
            }
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
            SaveLocal(email);
            try
            {
                await user.SignUpAsync();
            }
            catch (ParseException p)
            {
                ParseHelper.ParseData.LogException(p);
                var t = p.Message;

                isRegistered = true;
            }
            catch (Exception a)
            {
                ParseHelper.ParseData.LogException(a);
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
                var q = QuestionFromParseObject(t);
                q.QuizName = quizName;
                rv.Add(q);
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
            try
            {
                question.AnswerC = parseObject["C"].ToString();
                question.AnswerD = parseObject["D"].ToString();
            }
            catch { }
            question.Number = Convert.ToInt32(parseObject["Number"]);
            return question;
        }
        #endregion

        #region Progresses
        public async Task<List<QuestionProgress>> GetProgressesAsync()
        {
            var query = ParseObject.GetQuery("QuestionProgress").WhereEqualTo("user_id", AppSettings.CurrentAccount.ID);
            var objects = await query.FindAsync();

            var rv = new List<QuestionProgress>();
            foreach (var t in objects)
            {
                rv.Add(ProgressFromParseObject(t));
            }

            return rv;
        }

        public void SaveProgress(QuestionProgress progress)
        {
            var parseQuestion = new ParseObject("QuestionProgress");
            parseQuestion.ObjectId=progress.ID;
            parseQuestion["user_id"] = AppSettings.CurrentAccount.ID;
            parseQuestion["quiz_name"] = progress.QuizName;
            parseQuestion["question_id"] = progress.QuestionID;
            parseQuestion["answer_on"] = progress.AnswerOn;
            parseQuestion["answers"] = progress.Answers;
            parseQuestion["is_answered"] = progress.IsAnswered;
            parseQuestion["time_elapsed"] = progress.TimeElapsed;
            parseQuestion.SaveAsync();
        }

        QuestionProgress ProgressFromParseObject(ParseObject parseObject)
        {
            var progress = new QuestionProgress();
            progress.ID = parseObject.ObjectId;
            progress.QuestionID = parseObject["question_id"].ToString();
            progress.QuizName = parseObject["quiz_name"].ToString();
            progress.AnswerOn = Convert.ToInt32(parseObject["answer_on"]);
            progress.Answers = Convert.ToInt32(parseObject["answers"]);
            progress.IsAnswered = Convert.ToBoolean(parseObject["is_answered"]);
            progress.TimeElapsed = Convert.ToInt32(parseObject["time_elapsed"]);
            return progress;
        }
        #endregion

        #region Logs
        public void LogException(Exception ex)
        {
            var parseEx = new ParseObject("ExceptionLog");
            parseEx["message"] = ex.Message;
            parseEx["trace"] = ex.StackTrace;
            parseEx["user"] = string.IsNullOrEmpty(AppSettings.AccountEmail) ? "uncknown" : AppSettings.AccountEmail;
            parseEx.SaveAsync();
        }

        public void LogMessage(string message, [CallerMemberName] string from=null)
        {
            var parseEx = new ParseObject("MessageLog");
            parseEx["message"] = message;
            parseEx["from"] = from;
            parseEx["user"] = string.IsNullOrEmpty(AppSettings.AccountEmail) ? "uncknown" : AppSettings.AccountEmail;
            parseEx.SaveAsync();
        }
        #endregion
    }
}