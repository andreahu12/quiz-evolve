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
		 * Contains constructor to create a question page.
		 * Leads to various different pages depending on what the user does here.
		 * */

	public class QuestionPage : ContentPage {
		public static long triviaNum = 1;
		public static long salesNum = 1;
		public static long peopleNum = 1;
		public static long questionNum;
		public static long points;
		public static long triviaPoints = 0;
		public static long salesPoints = 0;
		public static long peoplePoints = 0;
		public static long totalPoints = 0;
		int secondsLeft = 30;
		Timer timer;
		Label countdownBar = new Label () {
			TextColor = Color.FromHex("#b455b6"),
			Font = Font.SystemFontOfSize(NamedSize.Small),
		};
		ParseObject obj;
		public static string questionList;
		ParseObject currentObj;






		public QuestionPage (ParseObject obj) {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			this.currentObj = obj;
			NavigationPage.SetHasNavigationBar (this, false);
			DetermineQuiz ();
			Console.WriteLine ("questionNum " + Quizes.QuestionPage.questionNum);

			if (questionNum > Quizes.QuizMenu.getTotalQuestions ()) {

				StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

				Button toResultsButton = new Button () {
					Text = "View Results",
					TextColor = Color.White,
					BackgroundColor = Color.FromHex("#3498db"),
				};

				toResultsButton.Clicked += (sender, e) => {
					QuestionPage.SaveEnviron();
					this.Navigation.PushAsync (new ResultSummary ());
				};

				sl.Children.Add (toResultsButton);
				Content = sl;

			} else {
				StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

				timer = new Timer () {
					Interval = 1000,
					Enabled = true,
				};

				string question = generateQuestion (questionNum);

				string optionA = generateA (questionNum);
				string optionB = generateB (questionNum);
				string optionC = generateC (questionNum);
				string optionD = generateD (questionNum);

				Image picA = new Image (), picB = new Image (), picC = new Image (), picD = new Image ();

				Image photo = new Image();

				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					SetCounters(countdownBar);

					Label questionLabel = new Label () {
//						Text = string.Format ("\n{0}. {1}\n\n", questionNum, question),
						Text = question,
						TextColor = Color.Black,
						XAlign = TextAlignment.Start,
					};

					StackLayout rowA = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowB = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowC = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowD = ProjectEcclesia.HelperMethods.createHorizSL();

					Button optionAButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = " A ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(50),
					};

					Button optionBButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = " B ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(50),
					};

					Button optionCButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = " C ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(50),
					};

					Button optionDButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = " D ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(50),
					};

					Button exitButton = new Button() {
						Text = "Exit Quiz",
						TextColor = Color.Gray,
					};

					Label labelA = new Label() {
						Text = optionA,
						TextColor = Color.Black,
					};

					var dim = 40;

					try {
						picA = new Image() {
							Aspect = Aspect.AspectFit,
							Source = new Uri((string) currentObj["aURI"]),
							WidthRequest = dim,
							HeightRequest = dim,
						};
					} catch (UriFormatException u) {
						Console.WriteLine(u.Message);
					} catch (KeyNotFoundException k) {
						Console.WriteLine(k.Message);
					}

					Label labelB = new Label() {
						Text = optionB,
						TextColor = Color.Black,
					};

					try {
						picB = new Image() {
							Aspect = Aspect.AspectFit,
							Source = new Uri((string) currentObj["bURI"]),
							HeightRequest = dim,
							WidthRequest = dim,
						};
					} catch (UriFormatException u) {
						Console.WriteLine(u.Message);
					} catch (KeyNotFoundException k) {
						Console.WriteLine(k.Message);
					}

					Label labelC = new Label() {
						Text = optionC,
						TextColor = Color.Black,
					};

					try {
						picC = new Image() {
							Aspect = Aspect.AspectFit,
							Source = new Uri((string) currentObj["cURI"]),
							HeightRequest = dim,
							WidthRequest = dim,
						};
					} catch (UriFormatException u) {
						Console.WriteLine(u.Message);
					} catch (KeyNotFoundException k) {
						Console.WriteLine(k.Message);
					}

					Label labelD = new Label() {
						Text = optionD,
						TextColor = Color.Black,
					};

					try {
						picD = new Image() {
							Aspect = Aspect.AspectFit,
							Source = new Uri((string) currentObj["dURI"]),
							HeightRequest = dim,
							WidthRequest = dim,
						};
					} catch (UriFormatException u) {
						Console.WriteLine(u.Message);
					} catch (KeyNotFoundException k) {
						Console.WriteLine(k.Message);
					}

					rowA.Children.Add(optionAButton);
					rowB.Children.Add(optionBButton);
					rowC.Children.Add(optionCButton);
					rowD.Children.Add(optionDButton);

					if (picA.Source != null) {
						rowA.Children.Add(picA);
					}
					rowA.Children.Add(labelA);

					if (picB.Source != null) {
						rowB.Children.Add(picB);
					}
					rowB.Children.Add(labelB);

					if (picC.Source != null) {
						rowC.Children.Add(picC);
					}
					rowC.Children.Add(labelC);

					if (picD.Source != null) {
						rowD.Children.Add(picD);
					}
					rowD.Children.Add(labelD);

					optionAButton.Clicked += async (sender, e) => {
						int optionChosen = 1;
						if (questionNum < QuizMenu.getTotalQuestions()) {
							await GetNextQuestionObject(questionNum);
						}
						await CodeWordCheck(optionAButton, labelA, questionNum, optionChosen);
						if (isCorrect(questionNum, optionChosen)) {
							ToNextQuestion (optionChosen);
						}
						IsEnabled = false;
					};

					optionBButton.Clicked += async (sender, e) => {
						int optionChosen = 2;
						if (questionNum < QuizMenu.getTotalQuestions()) {
							await GetNextQuestionObject(questionNum);
						}
						await CodeWordCheck(optionBButton, labelB, questionNum, optionChosen);
						if (isCorrect(questionNum, optionChosen)) {
							ToNextQuestion (optionChosen);
						}
						IsEnabled = false;
					};

					optionCButton.Clicked += async (sender, e) => {
						int optionChosen = 3;
						if (questionNum < QuizMenu.getTotalQuestions()) {
							await GetNextQuestionObject(questionNum);
						}
						await CodeWordCheck(optionCButton, labelC, questionNum, optionChosen);
						if (isCorrect(questionNum, optionChosen)) {
							ToNextQuestion (optionChosen);
						}
						IsEnabled = false;
					};

					optionDButton.Clicked += async (sender, e) => {
						int optionChosen = 4;
						if (questionNum < QuizMenu.getTotalQuestions()) {
							await GetNextQuestionObject(questionNum);
						}
						await CodeWordCheck(optionDButton, labelD, questionNum, optionChosen);
						if (isCorrect(questionNum, optionChosen)) {
							ToNextQuestion (optionChosen);
						}
						IsEnabled = false;
					};

					exitButton.Clicked += async (sender, e) => {
						await GetNextQuestionObject(questionNum);
						SaveEnviron();
						timer.Stop();
						await ProjectEcclesia.App.NavPage.Navigation.PushAsync(new ProjectEcclesia.MainMenuPage());
					};

					if (HasImage (currentObj)) {
						photo = new Image { Aspect = Aspect.AspectFit };
						photo.Source = ImageSource.FromUri (new Uri ( (string) currentObj["URI"]));
					}

					timer.Elapsed += untilTimeIsUp;

					sl.Children.Add (exitButton);
					sl.Children.Add (countdownBar);

					if (HasImage(obj))
						sl.Children.Add(photo);

					sl.Children.Add (questionLabel);

					sl.Children.Add (rowA);
					sl.Children.Add (rowB);

					if (!optionC.Equals(""))
						sl.Children.Add (rowC);
					if (!optionD.Equals(""))
						sl.Children.Add (rowD);

					Content = sl;
				});
			}
		}

		/**
			 * <summary>
			 * Marks the option chosen as right or wrong and pushes the
			 * EnterCodewordPage if they choose to ask a rep.
			 * </summary>
			 * */
		private async Task CodeWordCheck (Button currentButton, Label letterLabel, long questionNum, int optionChosen) {
			if (isCorrect (questionNum, optionChosen)) {
				timer.Stop ();
				currentButton.BackgroundColor = Color.Green;
				letterLabel.TextColor = Color.Green;
				//secondsLeft = points earned for this question
				await DisplayAlert("Correct!", string.Format("+{0}", secondsLeft), "Continue", null);
			} else {
				timer.Stop ();
				currentButton.BackgroundColor = Color.Red;
				letterLabel.TextColor = Color.Red;
				bool alert = await DisplayAlert("Incorrect!", "Would you like to ask a rep for the solution (and codeword) so you can try again for partial credit?" , "Yes", "No");
				Console.WriteLine ("bool alert: " + alert);
				if (alert) {
					secondsLeft = secondsLeft / 2;
					string quizName = QuizMenu.getQuizName ();
					if (quizName.Equals ("Trivia")) {
						triviaPoints += secondsLeft;
						points = triviaPoints;
					} else if (quizName.Equals ("Sales")) {
						salesPoints += secondsLeft;
						points = salesPoints;
					} else if (quizName.Equals ("People")) {
						peoplePoints += secondsLeft;
						points = peoplePoints;
					}
					totalPoints = salesPoints + triviaPoints + peoplePoints;
					await this.Navigation.PushModalAsync(new EnterCodewordPage(currentObj));
				} else {
					ToNextQuestion (optionChosen);
				}
			}
		}

		/**
			 * <summary>
			 * Keeps track of questionNum, points, and the name of the question database.
			 * </summary>
			 * */
		public static void DetermineQuiz() {
			string quizName = QuizMenu.getQuizName ();

			if (quizName.Equals ("Trivia")) {
				questionNum = triviaNum;
				points = triviaPoints;
				questionList = "TriviaQuestions";
			} else if (quizName.Equals ("Sales")) {
				questionNum = salesNum;
				points = salesPoints;
				questionList = "SalesQuestions";
			} else if (quizName.Equals ("People")) {
				questionNum = peopleNum;
				points = peoplePoints;
				questionList = "PeopleQuestions";
			}
		}

		/**
			 * <summary>
			 * Sets counters counting down from 30 at the top of question page.
			 * </summary>
			 * */

		private void SetCounters(Label countdownBar) {
			string markers = "";
			for (int i = 0; i < secondsLeft; i ++) {
				markers = markers + "o";
			}

			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
				countdownBar.Text = markers;
			});
		}

		/**
			 * <summary>
			 * Saves progress, calculates points, stops timer, and pushes next QuestionPage.
			 * </summary>
			 * */
		private void ToNextQuestion (int optionChosen) {
			string quizName = QuizMenu.getQuizName ();
			SaveEnviron ();
			if (isCorrect (questionNum, optionChosen)) {
				if (quizName.Equals ("Trivia")) {
					triviaPoints += secondsLeft;
					points = triviaPoints;
				} else if (quizName.Equals ("Sales")) {
					salesPoints += secondsLeft;
					points = salesPoints;
				} else if (quizName.Equals ("People")) {
					peoplePoints += secondsLeft;
					points = peoplePoints;
				}
				totalPoints = salesPoints + triviaPoints + peoplePoints;
			}

			if (quizName.Equals ("Trivia")) {
				triviaNum++;
				questionNum = triviaNum;
			} else if (quizName.Equals ("Sales")) {
				salesNum++;
				questionNum = salesNum;
			} else if (quizName.Equals ("People")) {
				peopleNum++;
				questionNum = peopleNum;
			}

			timer.Stop ();
			timer = null; 
			this.Navigation.PushAsync (new QuestionPage (obj));
		}

		/**
			 * <summary>
			 * Checks if the user got the question right.
			 * </summary>
			 * */
		private bool isCorrect (long questionNum, double optionChosen) {
			double solution = (double) currentObj ["SolutionNum"];
			Console.WriteLine ("Solution " + solution.ToString ());
			Console.WriteLine (solution == optionChosen);
			return (solution == optionChosen);	
		}

		/**
			 * <summary>
			 * Queries for the next question object from database to pass on to next QuestionPage
			 * </summary>
			 * */
		private async Task<ParseObject> GetNextQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();
			var query = from question in ParseObject.GetQuery (questionDB)
					where question.Get<long>("Number") == (questionNum + 1)
				select question;
			ParseObject obj = await query.FirstAsync();
			this.obj = obj;
			return obj;
		}

		/**
			 * <summary>
			 * Gets question from current question object.
			 * </summary>
			 * */
		private string generateQuestion (long questionNum) {
			string question = (string) currentObj["Number"].ToString() + ". " + (string) currentObj ["Question"];
			return question;
		}

		/**
			 * <summary>
			 * Decrements number of counters until time is up on the QuestionPage.
			 * </summary>
			 * */

		private void untilTimeIsUp (object o, System.EventArgs e) {
			Console.WriteLine (secondsLeft);
			secondsLeft--;

			SetCounters (countdownBar);

			if (secondsLeft == 0.0) {
				questionNum++;
				timer.Enabled = false;

				Xamarin.Forms.Device.BeginInvokeOnMainThread (()  => {
					this.Navigation.PushModalAsync (new TimesUpPage (obj));
				});
			}
		}

		/**
			 * <summary>
			 * Generates option A from current question object.
			 * </summary>
			 * */

		private string generateA (long questionNum) {
			string a = (string) currentObj ["A"];
			return a;
		}

		/**
			 * <summary>
			 * Generates option B from current question object.
			 * </summary>
			 * */

		private string generateB (long questionNum) {
			string b = (string) currentObj ["B"];
			return b;
		}

		/**
			 * <summary>
			 * Generates option C from current question object.
			 * </summary>
			 * */

		private string generateC (long questionNum) {
			string c = "defaultString";
			try {
				c = (string) currentObj ["C"];
			} catch (KeyNotFoundException e) {
				Console.WriteLine ("No option C" + e);
			} finally {
				if (c.Equals("defaultString")) {
					c = "";
				}
			}
			return c;
		}

		/**
			 * <summary>
			 * Generates option D from current question object.
			 * </summary>
			 * */

		private string generateD (long questionNum) {
			string d = "defaultString";
			try {
				d = (string) currentObj ["D"];
			} catch (KeyNotFoundException e) {
				Console.WriteLine ("No option D" + e);
			} finally {
				if (d.Equals("defaultString")) {
					d = "";
				}
			}
			return d;
		}

		/**
			 * <summary>
			 * Saves totalPoints, salesPoints, triviaPoints, and the current Sales and Trivia question
			 * the user is on to Parse.
			 * </summary>
			 * */

		public static void SaveEnviron() {
			var user = ParseUser.CurrentUser;
			user ["OverallPoints"] = totalPoints;
			user ["SalesPoints"] = salesPoints;
			user ["PeoplePoints"] = peoplePoints;
			user ["TriviaPoints"] = triviaPoints;
			user ["CurrentSales"] = salesNum;
			user ["CurrentTrivia"] = triviaNum;
			user ["CurrentPeople"] = peopleNum;
			user.SaveAsync ();
		}

		/**
			 * <summary>
			 * Gets totalPoints, salesPoints, triviaPoints, and the current Sales and Trivia question
			 * the user is on from Parse.
			 * </summary>
			 * */

		public static void SetEnviron() {
			var user = ParseUser.CurrentUser;
			Quizes.QuestionPage.totalPoints =  (long) user ["OverallPoints"];
			Quizes.QuestionPage.salesPoints = (long) user ["SalesPoints"];
			Quizes.QuestionPage.triviaPoints = (long) user ["TriviaPoints"];
			Quizes.QuestionPage.peoplePoints = (long)user ["PeoplePoints"];
			Quizes.QuestionPage.salesNum = (long) user ["CurrentSales"];
			Quizes.QuestionPage.triviaNum = (long) user ["CurrentTrivia"];
			Quizes.QuestionPage.triviaNum = (long)user ["CurrentPeople"];
		}

		/**
			 * <summary>
			 * Checks if the current question object has an image and returns the URI for it if it does.
			 * </summary>
			 * */

		private bool HasImage (ParseObject currentObj) {
			try {
				return (currentObj ["URI"] != null);
			} catch (KeyNotFoundException e) {
				Console.WriteLine (e.Message);
				return false;
			}
		}
	}
}
