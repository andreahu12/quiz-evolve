using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Ecclesia.ViewModels;


namespace Xamarin.Ecclesia.Utils
{
    /// <summary>
    /// Helper to allow navigation from viewmodels and othe common things
    /// </summary>
    public static  class CommonActions
    {
        #region events
        #endregion

        #region Fields
        static QuizViewModel _activeQuiz;
        static QuestionViewModel _activeQuestion;
        #endregion

        #region Properties
        public static QuizViewModel ActiveQuiz 
        {
            get
            {
                return _activeQuiz;
            }
            set
            {
                if (_activeQuiz != value)
                {
                    if (_activeQuiz != null)
                    {
                        _activeQuiz.ClearChildren();
                    }
                    _activeQuiz = value;
                }
            }
        }

        public static QuestionViewModel ActiveQuestion
        {
            get
            {
                return _activeQuestion;
            }
            set
            {
                if (_activeQuestion != value)
                {
                    if (_activeQuestion != null)
                    {
                        _activeQuestion.ClearChildren();
                    }
                    _activeQuestion = value;
                }
            }
        }
        #endregion
                
       
    }
}
