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
    public class AnswerViewModel:ParentViewModel
    {
        #region Constructor
        public AnswerViewModel(XElement data):base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = data.Attribute("Id").Value;
            Text = data.Attribute("Text").Value;
        }

        public AnswerViewModel(string id, string answer)
            : base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            ID = id;
            Text = answer;
        }
        #endregion

        #region Fields
        bool _isChecked ;
        #endregion

        #region Properties
        public string ID { get; private set; }
        public string Text { get; private set; }
               
        #endregion

        #region Methods
        public void CheckAnswer()
        {
            if (_isChecked)
                return;
            /*if (ID == ((QuestionViewModel)Parent).ID)
                UserSettings.Score+=10;
            else
                UserSettings.Score -= 2;
            _isChecked = true;
             */
        }

        
        #endregion
    }
}
