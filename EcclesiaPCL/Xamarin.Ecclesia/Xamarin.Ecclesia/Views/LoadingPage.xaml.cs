using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Auth;
using Xamarin.Ecclesia.Parse;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.ViewModels;
using Xamarin.Forms;

namespace Xamarin.Ecclesia.Views
{
    public partial class LoadingPage:BaseView
    {
        public LoadingPage()
        {
            //Initialize UI
            InitializeComponent();
           //Set viewmodel

        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public async override void AttachNavigationEvents()
        {
//#if DEBUG
//            await Navigation.PushAsync(new LoginPage());
//            return;
//#endif
            var email = AppSettings.AccountEmail;
            //var id = AppSettings.AccountID;
            if (string.IsNullOrEmpty(email))
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                try
                {
                    await ParseHelper.ParseData.SigInAccountAsync(email);
                    AppSettings.CurrentAccount=ParseHelper.ParseData.GetCurrentAccount();
                    if (AppSettings.CurrentAccount != null)
                        await Navigation.PushAsync(new MainMenuPage());
                }
                catch
                {
                    AppSettings.AccountEmail="";
                    //AppSettings.AccountID = "";
                }
                if (string.IsNullOrEmpty(AppSettings.AccountEmail))
                {
                    await Navigation.PushAsync(new LoginPage());
                }
            }
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
        }
        #endregion

        #region Controls Handlers
        async void OnFBClicked(object sender, EventArgs args)
        {
            if (await AuthHelper.OAuthCommunicator.AuthWithFacebookAsync())
                await Navigation.PushAsync(new MainMenuPage());
        }
        async void OnLdInClicked(object sender, EventArgs args)
        {
            if (await AuthHelper.OAuthCommunicator.AuthWithLinkedInAsync())
                await Navigation.PushAsync(new MainMenuPage());
        }
        #endregion
    }
}
