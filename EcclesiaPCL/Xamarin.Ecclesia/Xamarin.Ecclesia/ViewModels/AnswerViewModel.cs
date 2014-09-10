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
        
        public AnswerViewModel(int id, string answer)
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
        public int ID { get; private set; }
        public string Text { get; private set; }
               
        #endregion

        #region Methods
        public void CheckAnswer()
        {
            if (_isChecked)
                return;
            ((QuestionViewModel)Parent).CheckAnswer(ID);
             
            _isChecked = true;
            
        }

        
        #endregion
    }
}
