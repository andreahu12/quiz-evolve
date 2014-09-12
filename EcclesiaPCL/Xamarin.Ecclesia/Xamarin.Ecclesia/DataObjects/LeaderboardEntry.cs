using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.DataObjects
{
    public class LeaderboardEntry
    {
        #region Constants
        
        #endregion

        #region Constructor
        public LeaderboardEntry()
        {
        }
        #endregion

        #region Properties
        public string ID { get; set; }
        public string QuizName { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        #endregion

        #region Methods
        
        #endregion
    }
}
