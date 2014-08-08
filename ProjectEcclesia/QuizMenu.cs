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
		 * Contains constructor and helper methods to create a menu displaying quiz options (sales + trivia).
		 * Also allows for retrieval of the total number of questions for that quiz and which quiz is being played.
		 * */
	public class QuizMenu : ContentPage {

		static int totalQuestions;
		static string quizName;

		/**
			 * Constructor for the Quiz Menu
			 * Can choose from Trivia and Sales
			 * */
		public QuizMenu () {
			NavigationPage.SetHasNavigationBar (this, false);
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();
			BackgroundColor = Color.FromHex ("#ecf0f1");
			Title = "Quizes";

			Label pageTitle = new Label () {
				Text = "Quiz Menu",
				TextColor = Color.FromHex("#4e5758"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Button toSalesQuiz = new Button () {
				Text = "Sales Playbook Quiz",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toTrivaQuiz = new Button () {
				Text = "Xamarin Trivia Quiz",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toPeopleQuiz = new Button () {
				Text = "People Quiz",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toMainMenu = new Button () {
				Text = "Main Menu",
			};

			toSalesQuiz.Clicked += async (sender, e) => {
				totalQuestions = await GetNumQuestions("SalesQuestions");
				quizName = "Sales";
//				await this.Navigation.PushAsync(new QuestionListPage());
				await this.Navigation.PushAsync(new QuizInstructions());
			};

			toTrivaQuiz.Clicked += async (sender, e) => {
				totalQuestions = await GetNumQuestions("TriviaQuestions");
				quizName = "Trivia";
//				await this.Navigation.PushAsync(new QuestionListPage());
				await this.Navigation.PushAsync(new QuizInstructions());
			};

			toPeopleQuiz.Clicked += async (sender, e) => {
				totalQuestions = await GetNumQuestions("PeopleQuestions");
				quizName = "People";
//				await this.Navigation.PushAsync(new QuestionListPage());
				await this.Navigation.PushAsync(new QuizInstructions());
			};

			toMainMenu.Clicked += async (sender, e) => {
				await this.Navigation.PopAsync();
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (toSalesQuiz);
			sl.Children.Add (toTrivaQuiz);
			sl.Children.Add (toPeopleQuiz);
			sl.Children.Add (toMainMenu);

			Content = new ScrollView () {
				Content = sl,
			};
		}
		/**
		 * <summary>
		 * Queries for the number of questions in the particular question database
		 * </summary>
		 * @param string parseDBName
		 * @return Task<int>
		 * */
		private async Task<int> GetNumQuestions(string parseDBName) {
			var query = from questions  in ParseObject.GetQuery (parseDBName)
						where (long) questions["Number"] > 0
			            select questions;
			var count = await query.CountAsync ();
			return count;
		}

		/**
		 * <summary>
		 * Retrieves the total number of questions for the question database.
		 * </summary>
		 * */
		public static int getTotalQuestions() {
			return totalQuestions;
		}

		/**
		 * <summary>
		 * Retrieves the name of the quiz.
		 * </summary>
		 * */

		public static string getQuizName() {
			return quizName;
		}

		/**
		 * <summary>
		 * Converts the quizName to the name of the question database.
		 * </summary>
		 * */

		public static string GetQuestionList() {
			if (quizName.Equals ("Sales")) {
				return "SalesQuestions";
			} else if (quizName.Equals ("Trivia")) {
				return "TriviaQuestions";
			} else if (quizName.Equals ("People")) {
				return "PeopleQuestions";
			} else {
				return "";
			}
		}
	}
}