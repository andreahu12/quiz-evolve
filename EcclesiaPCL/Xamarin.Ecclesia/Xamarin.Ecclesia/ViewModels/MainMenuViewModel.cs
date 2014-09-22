using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;
using Xamarin.Ecclesia.Views;
using Xamarin.Ecclesia.Parse;


namespace Xamarin.Ecclesia.ViewModels
{
    public class MainMenuViewModel : EcclesiaViewModel
    {
        #region Constructor
        public MainMenuViewModel()
        {
            Title = "Main Menu";
            BackgroundColor = AppSettings.PageBackgroundColor;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        public override bool IsBusy {
			get {
				return base.IsBusy;
			}
			set {
				base.IsBusy = value;
				Title = (value)?"Please wait...":"Main Menu";
				NotifyPropertyChanged ("IsMenuVisible");
			}
		}

		public bool IsMenuVisible
		{
			get { return !IsBusy;}
		}
        #endregion

        #region Methods
		public async void LoadApp()
		{

			IsBusy = true;
			//#if DEBUG
			//            await Navigation.PushAsync(new LoginPage());
			//            return;
			//#endif
			var email = AppSettings.AccountEmail;
			//var id = AppSettings.AccountID;
			if (string.IsNullOrEmpty(email))
			{
				await App.RootPage.Navigation.PushModalAsync(new LoginPage());
			}
			else
			{
				try
				{
					await ParseHelper.ParseData.SigInAccountAsync(email);
					AppSettings.CurrentAccount=ParseHelper.ParseData.GetCurrentAccount();
					if (AppSettings.CurrentAccount != null)
						IsBusy=false;
				}
				catch
				{
					AppSettings.AccountEmail="";
					//AppSettings.AccountID = "";
				}
				if (string.IsNullOrEmpty(AppSettings.AccountEmail))
				{
					await App.RootPage.Navigation.PushModalAsync(new LoginPage());
				}
			}
			IsBusy = false;
		}
        #endregion
    }
}
