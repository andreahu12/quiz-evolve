using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Utils;
using Xamarin.Ecclesia.ViewModels;
using Xamarin.Forms;

namespace Xamarin.Ecclesia.Views
{
    public partial class LeaderboardPage : BaseView
    {
        public LeaderboardPage()
        {
            //Initialize UI
            InitializeComponent();
            
        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public override void AttachNavigationEvents()
        {
            CommonActions.ActiveLeaderboard.LoadLeaderboardsFromParse();
            BindingContext = CommonActions.ActiveLeaderboard;
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
            BindingContext = null;
        }
        #endregion

        #region Controls Handlers
               
        
        #endregion
    }
}
