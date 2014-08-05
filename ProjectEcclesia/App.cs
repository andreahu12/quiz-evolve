using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

/**
 * This namespace contains the NavPage, SignUpPage, LoginPage, MainMenuPage.
 * It also has a class with helper methods to speed up the process of creating horizontal/vertical layouts.
 * */
namespace ProjectEcclesia
{
	/**
	 * Contains the NavPage for popping to root
	 * */
	public class App
	{
		public static NavigationPage NavPage;

		public static Page GetMainPage () {    
			NavPage = new NavigationPage ();
			NavPage.PushAsync (new LoginPage ());
			return NavPage;
		}
	}

	/**
	 * A SignUpPage to be pushed/popped modally from the SignUpPage.
	 * */
	public class SignUpPage : ContentPage {

		/**
		 * Constructor for SignUpPage
		 * */
		public SignUpPage() {
			BackgroundColor = Color.FromHex ("#2c3e50");
			NavigationPage.SetHasNavigationBar (this, false);

			StackLayout vl = HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Sign Up",
				TextColor = Color.FromHex("#b455b6"),
				Font = Font.SystemFontOfSize (NamedSize.Large),
			};

			Entry firstNameEntry = new Entry () {
				Placeholder = "First",
				TextColor = Color.FromHex("#4e5758"),
			};

			Entry lastNameEntry = new Entry () {
				Placeholder = "Last",
				TextColor = Color.FromHex("#4e5758"),
			};

			Entry emailEntry = new Entry () {
				Placeholder = "Email",
				TextColor = Color.FromHex("#4e5758"),
			};

			Entry passwordEntry = new Entry () {
				Placeholder = "Password",
				TextColor = Color.FromHex("#4e5758"),
				IsPassword = true,
			};

			Button createAccountButton = new Button () {
				Text = "Create Account",
				BackgroundColor = Color.FromHex("#3498db"),
				TextColor = Color.White,
			};

			Button backToLoginScreenButton = new Button () {
				Text = "Return to Login Screen",
				TextColor = Color.FromHex("#ecf0f1"),
			};
					
			createAccountButton.Clicked += async (sender, e) => {
				try {
					ParseUser user = new ParseUser() {
						Username = emailEntry.Text,
						Email = emailEntry.Text,
						Password = passwordEntry.Text,
					};

					user["Name"] = string.Format("{0} {1}", firstNameEntry.Text, lastNameEntry.Text);
					user["CurrentSales"] = 1;
					user["CurrentTrivia"] = 1;
					user["CurrentPeople"] = 1;
					user["SalesPoints"] = 0;
					user["TriviaPoints"] = 0;
					user["OverallPoints"] = 0;
					user["PeoplePoints"] = 0;

					await user.SignUpAsync();
					await this.Navigation.PopModalAsync();

				} catch (InvalidOperationException i) {
					var alert = DisplayAlert ("Invalid Operation", "Please check your entry fields.", "Continue", null);
					Console.WriteLine(alert + i.Message);
				} catch (ParseException p) {
					var alert = DisplayAlert("Account Taken", "There is an existing account with these credentials.", "Okay", null);
					Console.WriteLine (alert + p.Message);
				} catch (Exception a) {
					var alert = DisplayAlert("Error", "An error has occurred. Please try again.", "Continue", null);
					Console.WriteLine (alert + a.Message);
				}
			};


			backToLoginScreenButton.Clicked += async (sender, e) => {
				await this.Navigation.PopModalAsync();
			};

			vl.Children.Add (pageTitle);
			vl.Children.Add (firstNameEntry);
			vl.Children.Add (lastNameEntry);
			vl.Children.Add (emailEntry);
			vl.Children.Add (passwordEntry);
			vl.Children.Add (createAccountButton);
			vl.Children.Add (backToLoginScreenButton);

			Content = vl;
		}
	}

	/**
	 * A LoginPage for users to log in.
	 * If the user is already logged in, it pushes a new MainMenuPage.
	 * */
	public class LoginPage : ContentPage {

		ParseUser user;

		/**
		 * Constructor for a LoginPage
		 * */
		public LoginPage () {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);

			if (ParseUser.CurrentUser != null) {
				Console.WriteLine ("Current User Not Null " + ParseUser.CurrentUser.Username.ToString ());
				this.Navigation.PushAsync (new MainMenuPage ());
			} else {
				Console.WriteLine ("Current User Null");
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

				Button signUpButton = new Button () {
					Text = "Sign Up",
				};

				Image monkeyImage = new Image { Aspect = Aspect.AspectFit };
				monkeyImage.Source = ImageSource.FromUri (new Uri ("http://blog.xamarin.com/wp-content/uploads/2014/04/monkey.png"));

				loginButton.Clicked += async (sender, e) => {
					try {
						user = await ParseUser.LogInAsync (emailEntry.Text, passwordEntry.Text);
						await this.Navigation.PushAsync (new MainMenuPage ());

					} catch {
						DisplayAlert ("Invalid Login", "Your login information did not match our records. Please try again.", "Okay", null);
					
					} finally {
						passwordEntry.Text = "";
						emailEntry.Text = "";
					}
				};

				signUpButton.Clicked += async (sender, e) => {
					await this.Navigation.PushModalAsync(new SignUpPage());
				};

				sl.Children.Add (welcomeLabel);
				sl.Children.Add (emailEntry);
				sl.Children.Add (passwordEntry);
				sl.Children.Add (loginButton);
				sl.Children.Add (signUpButton);
				sl.Children.Add (monkeyImage);
				Content = sl;
			}
		}
	}

	/**
	 * A MainMenuPage that allows the user to go to QuizMenu, LeaderboardMenu, and Log Out.
	 * */
	public class MainMenuPage : ContentPage {

		/**
		 * Constructor for the Main Menu
		 * */
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

			Button logOutButton = new Button () {
				Text = "Log Out",
			};

			toLBMenu.Clicked += (sender, e) => {
				this.Navigation.PushAsync(new Leaderboards.LeaderboardOptionsPage());
			};

			toQuizMenu.Clicked += (sender, e) => {
				this.Navigation.PushAsync(new Quizes.QuizMenu());
			};

			logOutButton.Clicked += async (sender, e) => {
				ParseUser.LogOut();
				await ProjectEcclesia.App.NavPage.PopToRootAsync();
				await ProjectEcclesia.App.NavPage.PushAsync(new LoginPage());
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (toQuizMenu);
			sl.Children.Add (toLBMenu);
			sl.Children.Add (logOutButton);

			Content = sl;
		}
	}

	/**
	 * Contains methods to simplify creating vertical and horizontal stacklayouts.
	 * */
	public class HelperMethods {

		/**
		 * <summary>
		 * Returns a Vertical StackLayout with varying padding based on platform.
		 * </summary>
		 * @param none
		 * @return StackLayout
		 * */
		public static StackLayout createVertSL () {
			StackLayout sl = new StackLayout () {
				Orientation = StackOrientation.Vertical,
				Padding = new Thickness (20, Device.OnPlatform (40, 20, 20), 20, 20),
			};
			return sl;
		}

		/**
		 * <summary>
		 * Returns a Horizontal StackLayout with varying padding based on platform.
		 * </summary>
		 * @param none
		 * @return StackLayout
		 * */
		public static StackLayout createHorizSL() {
			StackLayout sl = new StackLayout () {
				Orientation = StackOrientation.Horizontal,
			};
			return sl;
		}
	}
}
