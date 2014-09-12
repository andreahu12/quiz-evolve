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
    public class LeaderboardViewModel:ParentViewModel
    {
        #region Constructor
        
        /// <summary>
        /// Quiz from parse data
        /// </summary>
        /// <param name="data"></param>
        public LeaderboardViewModel(Quiz quiz)
            : base()
        {
            Title = "Quiz";
            BackgroundColor = AppSettings.PageBackgroundColor;
            _quiz = quiz;
        }
        #endregion

        #region Fields
        Quiz _quiz;
        #endregion

        #region Properties
        public string ID {
            get
            { return _quiz.ID; }
        }
        public string Name {
            get { return _quiz.Name; }
        }
        public string Description {
            get { return _quiz.Description; }
        }

		public string Score
		{
			get
			{ 
                return AppSettings.CurrentAccount.GetQuizScore(Name).ToString ();
			}
		}

        #endregion

        #region Methods
        
        public async void LoadLeaderboardsFromParse()
        {
            if (Children != null && Children.Any())
                ClearChildren();
            var leaderboardsData = await ParseHelper.ParseData.GetQuizLeaderboardsAsync(Name);

            foreach (var leaderboard in leaderboardsData)
            {
                AddChild(new LeaderboardItemViewModel(leaderboard));
            }
        }

        #endregion
    }
}
