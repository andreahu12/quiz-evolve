using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.DataObjects;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.XML;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class QuestionViewModel:ParentViewModel
    {
        #region Constructor

        /// <summary>
        /// Creates vm from Parse object
        /// </summary>
        /// <param name="data"></param>
        public QuestionViewModel(QuizQuestion question)
            : base()
        {
            Title = "Question";
            BackgroundColor = AppSettings.PageBackgroundColor;
            _question = question;
            LoadAnswers(question);

            _progress = AppSettings.CurrentAccount.GetProgressForQuestion(question);

        }
        #endregion

        #region Fields
        QuestionProgress _progress;
        QuizQuestion _question;
        bool _stopTimer = false;
        #endregion

        #region Properties
        public string ID
        {
            get
            { return _question.ID; }
        }
        public string Text
        {
            get
            { return _question.Question; }
        }
        public string QuizName {
            get 
            { return _question.QuizName; }
        }
        public int CorrectAnswerID
        {
            get
            { return _question.CorrectAnswerID; }
        }

        public string Name
        {
            get
            {
                return "Question #" + (Index + 1).ToString();
            }
        }

        public string Score
        {
            get
            {
                return "My score is " +AppSettings.CurrentAccount.GetQuizScore(QuizName);
            }
        }

        public int Index
        {
            get
            {
                if (Parent == null)
                    return 0;
                return ((QuizViewModel)Parent).Children.IndexOf(this);
            }
        }

        public bool HasNextQuestion
        {
            get
            {
                if (Parent == null)
                    return false;
                return Index < ((QuizViewModel)Parent).Children.Count-1;
            }
        }

        public QuestionViewModel NextQuestion
        {
            get
            {
                if (Parent == null)
                    return null;
                return ((QuizViewModel)Parent).Children[Index + 1] as QuestionViewModel;
            }
        }

        public string TimeRemaining
        {
            get
            {
                return _progress.TimeRemaining.ToString();
            }
        }
        #endregion

        #region Methods
        
        public void StartTimer()
        {
            _stopTimer = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), TimerCallback);
        }

        public void StopTimer()
        {
            _stopTimer = true;
        }

        bool TimerCallback()
        {
            if (_stopTimer)
                return false;
            _progress.TimeElapsed++;
            NotifyPropertyChanged("TimeRemaining");
            return true;
        }

        public bool CheckAnswer(int answerId)
        {
            var isAnswered= CorrectAnswerID == answerId;
            _stopTimer = true;
            _progress.SetAnswer(isAnswered);
            return isAnswered;
        }

        void LoadAnswers(QuizQuestion question)
        {

            AddChildRandomly(new AnswerViewModel(1,question.AnswerA));
            AddChildRandomly(new AnswerViewModel(2, question.AnswerB));
            AddChildRandomly(new AnswerViewModel(3, question.AnswerC));
            AddChildRandomly(new AnswerViewModel(4, question.AnswerD));
        }

        public override void ClearChildren()
        {
            foreach (AnswerViewModel vm in Children)
                vm.ClearChildren();
            base.ClearChildren();
        }

        #endregion
    }
}
