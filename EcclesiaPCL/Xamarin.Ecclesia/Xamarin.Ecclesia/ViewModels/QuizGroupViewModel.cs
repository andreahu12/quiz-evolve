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
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizGroupViewModel:ParentViewModel
    {
        #region Constructor
        public QuizGroupViewModel():base()
        {
            Title = "Quizzes";
            BackgroundColor = AppSettings.PageBackgroundColor;
            LoadQuizzesFromParse();
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #endregion

        #region Methods
        /// <summary>
        /// Loads quizzes data from local xml file
        /// </summary>
        void LoadQuizzesFromXML()
        {
            var quizData = XMLHelper.LoadXML("Data/Quizzes/Quizzes.xml");
            var quizElements = quizData.Descendants("QuizGroup").ToList();

            foreach (var element in quizElements)
            {
                AddChild(new QuizViewModel(element));
            }
        }

        /// <summary>
        /// Loads quizzes data from parse.com database
        /// </summary>
        async void LoadQuizzesFromParse()
        {
            var quizzes = await ParseHelper.ParseData.GetQuizzesAsync();
            foreach (var quiz in quizzes)
            {
                AddChild(new QuizViewModel(quiz));
            }
        }
        #endregion
    }
}
