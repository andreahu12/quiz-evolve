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
		 * Contains constructor that creates a page with the instructions for how
		 * to play the quiz app.
		 * Also initiates values to pick up where user left off and gets the first question object
		 * and passes it down to the next page.
		 * */
	public class QuizInstructions : ContentPage {
		ParseObject obj;
		ParseUser user = ParseUser.CurrentUser;
		string quizname;
		long questionNum;

		/**
			 * Constructor for the Instructions page
			 * */
		public QuizInstructions () {
			NavigationPage.SetHasNavigationBar (this, false);

			BackgroundColor = Color.FromHex ("#ecf0f1");
			Title = "Instructions";

			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Instructions",
				TextColor = Color.FromHex("#4e5758"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Label instructions = new Label () {
				TextColor = Color.FromHex("#b455b6"),
				Text = string.Format(
					"{0} questions \n" +
					"30 seconds per question" +
					"\n\n" +
					"The faster you answer a question, the more points you get.\n\n" +
					"If you get a question wrong, you can ask a rep for partial credit.\n\n" +
					"If you quit, you can pick up where you left off later.\n\n", Quizes.QuizMenu.getTotalQuestions()),
			};

			Label readyLabel = new Label () {
				Text = "Are you ready?\n\n",
				TextColor = Color.Navy,
				Font = Font.SystemFontOfSize(NamedSize.Large),
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
			};

			Button startButton = new Button () {
				Text = "START",
				TextColor = Color.White,
				BackgroundColor = Color.FromHex("#3498db"),
			};

			Button toQuizMenu = new Button () {
				Text = "Quiz Menu",
			};

			SetValues ();

			startButton.Clicked += async (sender, e) => {
				try {
					obj = await GetFirstQuestionObject(questionNum);
				} catch (ParseException p) {
					Console.WriteLine(p.Message);
				}
				Quizes.QuestionPage.SetEnviron();
				await this.Navigation.PushAsync(new Countdown(obj));
			};

			toQuizMenu.Clicked += async (sender, e) => {
				await this.Navigation.PopAsync();
			};

			sl.Children.Add (pageTitle);
			sl.Children.Add (instructions);
			sl.Children.Add (readyLabel);
			sl.Children.Add (startButton);
			sl.Children.Add (toQuizMenu);

			Content = sl;
		}

		/**
			 * <summary>
			 * Retrieves the number of the question the user left off at.
			 * </summary>
			 * */

		private void SetValues() {
			quizname = QuizMenu.getQuizName();
			questionNum = Quizes.QuestionListPage.GetGoToQuestionNumber ();
//			if (quizname.Equals ("Trivia")) {
//				questionNum = (long)user ["CurrentTrivia"];
//			} else if (quizname.Equals ("Sales")) {
//				questionNum = (long)user ["CurrentSales"];
//			} else if (quizname.Equals ("People")) {
//				questionNum = (long)user ["CurrentPeople"];
//			}
		}

		/**
			 * <summary>
			 * Queries for the first question object when entering the quiz.
			 * </summary>
			 * @param long questionNum
			 * @return Task<ParseObject>
			 * */
		private async Task<ParseObject> GetFirstQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();
			var query = from question in ParseObject.GetQuery (questionDB)
					where question.Get<long>("Number") == (questionNum)
				select question;
			ParseObject obj = await query.FirstAsync();
			return obj;
		}
	}
}