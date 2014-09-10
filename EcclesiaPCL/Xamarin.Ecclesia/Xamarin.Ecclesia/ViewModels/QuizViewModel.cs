using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.DataObjects;
using Xamarin.Ecclesia.Parse;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.XML;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizViewModel:ParentViewModel
    {
        #region Constructor
        /// <summary>
        /// Quiz from XML data
        /// </summary>
        /// <param name="data"></param>
        public QuizViewModel(XElement data):base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = data.Attribute("Id").Value;
            Name = data.Attribute("Name").Value;
        }

        /// <summary>
        /// Quiz from parse data
        /// </summary>
        /// <param name="data"></param>
        public QuizViewModel(Quiz quiz)
            : base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = quiz.ID;
            Name = quiz.Name;
            Description = quiz.Description;
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        public string ID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        #endregion

        #region Methods
        public void LoadQuestionsFromXML()
        {
            if (Children != null && Children.Any())
                ClearChildren();
            var quizData = XMLHelper.LoadXML(string.Format("Data/Quizzes/{0}.xml",Name));
            var quizElements = quizData.Descendants("Question").ToList();

            foreach (var element in quizElements)
            {
                AddChild(new QuestionViewModel(element));
            }
        }

        public async void LoadQuestionsFromParse()
        {
            if (Children != null && Children.Any())
                ClearChildren();
            var quizData = await ParseHelper.ParseData.GetQuestionsAsync(Name);

            foreach (var question in quizData)
            {
                AddChild(new QuestionViewModel(question));
            }
        }

        public override void ClearChildren()
        {
            foreach (QuestionViewModel vm in Children)
                vm.ClearChildren();
            base.ClearChildren();
        }
        #endregion
    }
}
