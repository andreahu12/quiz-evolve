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
    public class LeaderboardsViewModel:ParentViewModel
    {
        #region Constructor
        public LeaderboardsViewModel()
            : base()
        {
            Title = "Leaderboards";
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
        /// Loads quizzes data from parse.com database
        /// </summary>
        async void LoadQuizzesFromParse()
        {
            var quizzes = await ParseHelper.ParseData.GetQuizzesAsync();
            foreach (var quiz in quizzes)
            {
                AddChild(new LeaderboardViewModel(quiz));
            }
        }
        #endregion
    }
}
