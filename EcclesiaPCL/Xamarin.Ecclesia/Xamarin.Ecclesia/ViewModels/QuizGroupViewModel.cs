using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;
using Xamarin.Ecclesia.XML;

namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizGroupViewModel:ParentViewModel
    {
        #region Constructor
        public QuizGroupViewModel():base()
        {
            Title = "Quizzes";
            BackgroundColor = AppSettings.PageBackgroundColor;
            LoadQuizzes();
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #endregion

        #region Methods
        void LoadQuizzes()
        {
            var quizData = XMLHelper.LoadXML("Data/Quizzes/Quizzes.xml");
            var quizElements = quizData.Descendants("QuizGroup").ToList();

            foreach (var element in quizElements)
            {
                AddChild(new QuizViewModel(element));
            }
        }

        #endregion
    }
}
