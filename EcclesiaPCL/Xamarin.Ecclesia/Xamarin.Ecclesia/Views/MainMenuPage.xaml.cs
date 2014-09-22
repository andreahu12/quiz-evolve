using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.ViewModels;
using Xamarin.Forms;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.Views
{
    public partial class MainMenuPage:BaseView
    {
        public MainMenuPage()
        {
            //Initialize UI
            InitializeComponent();
            //Set viewmodel
            BindingContext = ViewModelProvider.GetViewModel<MainMenuViewModel>();

        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public override void AttachNavigationEvents()
        {
			if (AppSettings.CurrentAccount == null) {
				var vm = GetViewModel<MainMenuViewModel> ();
				vm.LoadApp ();
			} else
				IsBusy = false;
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
        }
        #endregion

        #region Controls Handlers
        async void OnQuizzesClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new QuizzesPage());
        }
        async void OnHelpClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new InstructionsPage());
        }
        async void OnLeaderboardsClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new LeaderboardsPage());
        }
        async void OnMyInfoClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MyInfoPage());
        }



        #endregion
    }
}
