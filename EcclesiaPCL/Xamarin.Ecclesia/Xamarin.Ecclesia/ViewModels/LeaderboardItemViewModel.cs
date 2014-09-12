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
    public class LeaderboardItemViewModel:ParentViewModel
    {
        #region Constructor

        /// <summary>
        /// Creates vm from Parse object
        /// </summary>
        /// <param name="data"></param>
        public LeaderboardItemViewModel(LeaderboardEntry leaderboard)
            : base()
        {
            Title = "Leaderboard Item";
            BackgroundColor = AppSettings.PageBackgroundColor;
            _leaderboard = leaderboard;
        }
        #endregion

        #region Fields
        LeaderboardEntry _leaderboard;
        #endregion

        #region Properties
        public string ID
        {
            get
            { return _leaderboard.ID; }
        }
        public string Text
        {
            get
            { return _leaderboard.UserName; }
        }
        public string QuizName {
            get
            { return _leaderboard.QuizName; }
        }
        
        public string Score
        {
            get
            {
                return _leaderboard.Score.ToString();
            }
        }

        
        #endregion

        #region Methods
        
        #endregion
    }
}
