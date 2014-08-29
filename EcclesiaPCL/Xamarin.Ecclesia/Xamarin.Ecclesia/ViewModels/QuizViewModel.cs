using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class QuizViewModel:ParentViewModel
    {
        #region Constructor
        public QuizViewModel(XElement data):base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = data.Attribute("Id").Value;
            Name = data.Attribute("Name").Value;
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        public string ID { get; private set; }
        public string Name { get; private set; }
        #endregion

        #region Methods
        void LoadQuestions()
        {
            
        }

        #endregion
    }
}
