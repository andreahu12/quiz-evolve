using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Parse;
using Quizes;

namespace Quizes {
	/**
		 * Contains the constructor for a modally pushed page that gives the user 
		 * options to continue, exit, or log out when they run out of time.
		 * */
	public class TimesUpPage : ContentPage {
		ParseObject obj;

		/**
			 * Constructor for then time is up.
			 * */
		public TimesUpPage(ParseObject obj) {

			BackgroundColor = Color.FromHex("#2c3e50");
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Quiz Paused",
				TextColor = Color.FromHex("#b455b6"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Button continueButton = new Button () {
				Text = "Continue to Next Question",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button mainButton = new Button () {
				Text = "Main Menu",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button logOutButton = new Button () {
				Text = "Log Out",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			continueButton.Clicked += (sender, e) => {
				Quizes.QuestionPage.SaveEnviron();
				Console.WriteLine("Continue > questionNum " + QuestionPage.questionNum);
				if (QuestionPage.questionNum <= QuizMenu.getTotalQuestions()) {
					Device.BeginInvokeOnMainThread (async () => {
						obj = await GetNextQuestionObject(QuestionPage.questionNum);
						await this.Navigation.PopModalAsync();
						await ProjectEcclesia.App.NavPage.PushAsync(new QuestionPage(obj));
					});

				} else {
					Device.BeginInvokeOnMainThread (() => {
						ProjectEcclesia.App.NavPage.PushAsync(new ResultSummary());
						this.Navigation.PopModalAsync();
					});
				}
			};

			mainButton.Clicked += async (sender, e) => {
				QuestionPage.SaveEnviron();
				await this.Navigation.PopModalAsync();
				await ProjectEcclesia.App.NavPage.PopToRootAsync();
				await ProjectEcclesia.App.NavPage.PushAsync(new ProjectEcclesia.MainMenuPage());
			};

			logOutButton.Clicked += async (sender, e) => {
				QuestionPage.SaveEnviron();
				ParseUser.LogOut();
				await this.Navigation.PopModalAsync();
				await ProjectEcclesia.App.NavPage.PopToRootAsync();
				await ProjectEcclesia.App.NavPage.PushAsync(new ProjectEcclesia.LoginPage());
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (continueButton);
			sl.Children.Add (mainButton);
			sl.Children.Add (logOutButton);
			Content = sl;
		}

		/**
			 * <summary>
			 * Queries for next question object if user decides to continue.
			 * </summary>
			 * */
		private async Task<ParseObject> GetNextQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();

			string quizName = QuizMenu.getQuizName ();

			if (quizName.Equals ("Trivia")) {
				QuestionPage.triviaNum++;
				QuestionPage.questionNum = QuestionPage.triviaNum;
			} else if (quizName.Equals ("Sales")) {
				QuestionPage.salesNum++;
				QuestionPage.questionNum = QuestionPage.salesNum;
			} else if (quizName.Equals ("People")) {
				QuestionPage.peopleNum++;
				QuestionPage.questionNum = QuestionPage.peopleNum;
			}

			var query = from question in ParseObject.GetQuery (questionDB)
					where question.Get<long>("Number") == (questionNum)
				select question;
			ParseObject obj = await query.FirstAsync();
			this.obj = obj;
			return obj;
		}
	}
}
