using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace ProjectEcclesia
{
	public class App
	{
		public static NavigationPage NavPage;

		public static Page GetMainPage () {    
			NavPage = new NavigationPage ();
			NavPage.PushAsync (new LoginPage ());
//			NavPage.PushAsync (new CheckLoggedInPage ());
			return NavPage;
		}
	}

	public class CheckLoggedInPage : ContentPage {
		public CheckLoggedInPage() {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);

			if (ParseUser.CurrentUser != null) {
				Console.WriteLine ("Current User Not Null" + ParseUser.CurrentUser.ToString ());
				this.Navigation.PushAsync (new MainMenuPage ());
			} else {
				Console.WriteLine ("Current User Null");
				this.Navigation.PushAsync (new LoginPage ());
			}
		}
	}

	public class LoginPage : ContentPage {
//		public static ParseUser user;
		ParseUser user;
		public LoginPage () {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);
			if (ParseUser.CurrentUser != null) {
				Console.WriteLine ("Current User Not Null " + ParseUser.CurrentUser.Username.ToString ());
				this.Navigation.PushAsync (new MainMenuPage ());
			} else {
				Console.WriteLine ("Current User Null");
//				this.Navigation.PushAsync (new LoginPage ());
//			}
				NavigationPage.SetHasNavigationBar (this, false);
				StackLayout sl = HelperMethods.createVertSL ();

				Label welcomeLabel = new Label () {
					Text = "Login",
					TextColor = Color.FromHex("#4e5758"),
					Font = Font.SystemFontOfSize (NamedSize.Large),
				};

				Entry emailEntry = new Entry () {
					Placeholder = "Email",
					TextColor = Color.FromHex("#3498db"),
				};

				Entry passwordEntry = new Entry () {
					Placeholder = "Password",
					IsPassword = true,
					TextColor = Color.FromHex("#3498db"),
				};

				Button loginButton = new Button () {
					BackgroundColor = Color.FromHex("#3498db"),
					TextColor = Color.White,
					Text = "Submit",
				};

				Image monkeyImage = new Image { Aspect = Aspect.AspectFit };
				monkeyImage.Source = ImageSource.FromUri (new Uri ("http://blog.xamarin.com/wp-content/uploads/2014/04/monkey.png"));

				loginButton.Clicked += async (sender, e) => {
					try {
						user = await ParseUser.LogInAsync (emailEntry.Text, passwordEntry.Text);
						await this.Navigation.PushAsync (new MainMenuPage ());

					} catch {
						var alert = DisplayAlert ("Invalid Login", "Your login information did not match our records. Please try again.", "Okay", null);
						Console.WriteLine (alert);
					} finally {
						passwordEntry.Text = "";
						emailEntry.Text = "";
					}
				};

				sl.Children.Add (welcomeLabel);
				sl.Children.Add (emailEntry);
				sl.Children.Add (passwordEntry);
				sl.Children.Add (loginButton);
				sl.Children.Add (monkeyImage);
				Content = sl;
			}
		}

		private void SetPoints() {
			Quizes.QuestionPage.salesPoints = (int) user ["SalesPoints"];
		}
	}


	public class MainMenuPage : ContentPage {
		public MainMenuPage() {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);

			StackLayout sl = HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Main Menu",
				TextColor = Color.FromHex("#4e5758"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Button toLBMenu = new Button () {
				Text = "Leaderboards",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toQuizMenu = new Button () {
				Text = "Quizes",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			toLBMenu.Clicked += (sender, e) => {
				this.Navigation.PushAsync(new Leaderboards.LeaderboardOptionsPage());
			};

			toQuizMenu.Clicked += (sender, e) => {
				this.Navigation.PushAsync(new Quizes.QuizMenu());
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (toQuizMenu);
			sl.Children.Add (toLBMenu);

			Content = sl;
		}
	}

	public class HelperMethods {
		public static StackLayout createVertSL () {
			StackLayout sl = new StackLayout () {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness (20, Device.OnPlatform (40, 20, 20), 20, 20),
			};
			return sl;
		}

		public static StackLayout createHorizSL() {
			StackLayout sl = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
			};
			return sl;
		}
	}
}
