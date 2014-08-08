using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Parse;
using Quizes;

namespace Quizes
{
	/**
	 * Contains constructor to display results to the user.
	 * */
	public class ResultSummary : ContentPage {

		/**
		 * Constructor for result page.
		 * */
		public ResultSummary() {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Result Summary",
				TextColor = Color.FromHex("#4e5758"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Label resultLabel = new Label () {
				Text = string.Format("Your Final Score: {0} \nYour Overall Score: {1}", 
					Quizes.QuestionPage.points, Quizes.QuestionPage.totalPoints),
				TextColor = Color.FromHex("#b455b6"),
			};

			Button toRootButton = new Button () {
				Text = "Main Menu",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button logOutButton = new Button () {
				Text = "Log Out",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			toRootButton.Clicked += (sender, e) => {
				Device.BeginInvokeOnMainThread(async () => {
					Quizes.QuestionPage.SaveEnviron();
					await ProjectEcclesia.App.NavPage.PopToRootAsync();
					await ProjectEcclesia.App.NavPage.PushAsync(new ProjectEcclesia.MainMenuPage());
				});
			};

			logOutButton.Clicked += async (sender, e) => {
				Quizes.QuestionPage.SaveEnviron();
				ParseUser.LogOut();
				await ProjectEcclesia.App.NavPage.PopToRootAsync();
				await ProjectEcclesia.App.NavPage.PushAsync(new ProjectEcclesia.LoginPage());
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (resultLabel);
			sl.Children.Add (toRootButton);
			sl.Children.Add (logOutButton);

			Content = new ScrollView () {
				Content = sl,
			};
		}
	}
}

