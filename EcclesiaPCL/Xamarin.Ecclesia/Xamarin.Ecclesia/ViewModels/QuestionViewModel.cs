using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.XML;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class QuestionViewModel:ParentViewModel
    {
        #region Constructor
        public QuestionViewModel(XElement data)
            : base()
        {
            Title = "Question";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = data.Attribute("Id").Value;
            Text = data.Attribute("Text").Value;
            CorrectAnswerID = int.Parse(data.Attribute("CorrectAnswerId").Value);
            var quizElements = data.Descendants("Answer").ToList();
            LoadAnswers(quizElements);
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        public string ID { get; private set; }
        public string Text { get; private set; }

        public int CorrectAnswerID { get; private set; }

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
                return "My score is " + UserSettings.Score.ToString();
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
        #endregion

        #region Methods
        void LoadAnswers(List<XElement> quizElements)
        {
            
            foreach (var element in quizElements)
            {
                AddChildRandomly(new AnswerViewModel(element));
            }
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
