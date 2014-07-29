using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Parse;

namespace Quizes {
	public class QuizMenu : ContentPage {
		static int totalQuestions;
		static string quizName;

		public QuizMenu () {
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();
			BackgroundColor = Color.FromHex ("#ecf0f1");
			Title = "Quizes";

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

			toSalesQuiz.Clicked += (sender, e) => {
				totalQuestions = 40;
				quizName = "Sales";
				this.Navigation.PushAsync(new QuizInstructions());
			};

			toTrivaQuiz.Clicked += (sender, e) => {
				totalQuestions = 88;
				quizName = "Trivia";
				this.Navigation.PushAsync(new QuizInstructions());
			};

			sl.Children.Add (toSalesQuiz);
			sl.Children.Add (toTrivaQuiz);

			Content = sl;
		}

		public static int getTotalQuestions() {
			return totalQuestions;
		}

		public static string getQuizName() {
			return quizName;
		}

		public static string GetQuestionList() {
			if (quizName.Equals ("Sales")) {
				return "SalesQuestions";
			} else if (quizName.Equals ("Trivia")) {
				return "TriviaQuestions";
			} else {
				return "";
			}
		}
	}

	public class QuizInstructions : ContentPage {
		ParseObject obj;
		ParseUser user = ParseUser.CurrentUser;
		string quizname;
		long questionNum;

		public QuizInstructions () {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			Title = "Instructions";

			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label instructions = new Label () {
				Text = string.Format(
					"{0} questions \n" +
					"30 seconds per question" +
					"\n\n" +
					"The faster you answer a question, the more points you get!\n\n" +
					"If you run out of time, the quiz will be paused and you will be given the option to quit." +
					"\n\nIf you quit, you can pick up where you left off later.\n\n\n", Quizes.QuizMenu.getTotalQuestions()),
				TextColor = Color.FromHex("#b455b6"),
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

			SetValues ();

			startButton.Clicked += async (sender, e) => {
				obj = await GetFirstQuestionObject(questionNum);

				Quizes.QuestionPage.SetEnviron();
				await this.Navigation.PushAsync(new Countdown(obj));
			};

			sl.Children.Add (instructions);
			sl.Children.Add (readyLabel);
			sl.Children.Add (startButton);

			Content = sl;
		}

		private void SetValues() {
			quizname = QuizMenu.getQuizName();
			if (quizname.Equals("Trivia")) {
				questionNum = (long) user["CurrentTrivia"];
			} else if (quizname.Equals("Sales")) {
				questionNum = (long) user["CurrentSales"];
			}
		}

		private async Task<ParseObject> GetFirstQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();
			var query = from question in ParseObject.GetQuery (questionDB)
					where question.Get<long>("Number") == (questionNum)
				select question;
			ParseObject obj = await query.FirstAsync();
			return obj;
		}
	}

	public class Countdown : ContentPage {

		Label countdownLabel = new Label ();
		int secondsLeft = 3;
		System.Timers.Timer timer;
		ParseObject obj;

		public Countdown (ParseObject obj) {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			this.obj = obj;
			NavigationPage.SetHasNavigationBar (this, false);
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			timer = new System.Timers.Timer () {
				Enabled = true,
				Interval = 1000,
			};

			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
				countdownLabel.Font = Font.SystemFontOfSize(72);
				countdownLabel.Text = secondsLeft.ToString ();
				countdownLabel.TextColor = Color.FromHex("#b455b6");
				countdownLabel.XAlign = TextAlignment.Center;
			});

			timer.Elapsed += OnTimedEvent;
			sl.Children.Add(countdownLabel);
			Content = sl;
		}

		private void OnTimedEvent (object o, System.EventArgs e) {
			Console.WriteLine ("OnTimedEvent called");
			if (secondsLeft == 1) {
				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					countdownLabel.Text = "GO";
				});
				secondsLeft--;

			} else if (secondsLeft == 0) {
				timer.Enabled = false;
				timer = null;
				countdownLabel.IsVisible = false;

				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					this.Navigation.PushAsync (new QuestionPage (obj));
				});

			} else {
				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					countdownLabel.Text = secondsLeft.ToString ();
				});
				secondsLeft--;
			}
		}
	}

	public class QuestionPage : ContentPage {
		public static long triviaNum = 1;
		public static long salesNum = 1;
		public static long questionNum;

		public static long points;
		public static long triviaPoints = 0;
		public static long salesPoints = 0;
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

			if (questionNum >= Quizes.QuizMenu.getTotalQuestions ()) {

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

				Image photo = new Image();

				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					SetCounters(countdownBar);

					Label questionLabel = new Label () {
						Text = string.Format ("\n{0}. {1}\n\n", questionNum, question),
						//TextColor = Color.FromHex("#95a5a6"),
						TextColor = Color.Black,
						XAlign = TextAlignment.Start,
					};

					StackLayout rowA = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowB = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowC = ProjectEcclesia.HelperMethods.createHorizSL();
					StackLayout rowD = ProjectEcclesia.HelperMethods.createHorizSL();

					Button optionAButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = "  A  ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(NamedSize.Large),
					};

					Button optionBButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = "  B  ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(NamedSize.Large),
					};
						
					Button optionCButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = "  C  ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(NamedSize.Large),
					};

					Button optionDButton = new Button () {
						BackgroundColor = Color.FromHex("#2c3e50"),
						Text = "  D  ",
						TextColor = Color.FromHex ("#ecf0f1"),
						Font = Font.SystemFontOfSize(NamedSize.Large),
					};

					Label labelA = new Label() {
						Text = optionA,
						TextColor = Color.FromHex("#2c3e50"),
					};

					Label labelB = new Label() {
						Text = optionB,
						TextColor = Color.FromHex("#2c3e50"),
					};

					Label labelC = new Label() {
						Text = optionC,
						TextColor = Color.FromHex("#2c3e50"),
					};

					Label labelD = new Label() {
						Text = optionD,
						TextColor = Color.FromHex("#2c3e50"),
					};

					rowA.Children.Add(optionAButton);
					rowB.Children.Add(optionBButton);
					rowC.Children.Add(optionCButton);
					rowD.Children.Add(optionDButton);

					rowA.Children.Add(labelA);
					rowB.Children.Add(labelB);
					rowC.Children.Add(labelC);
					rowD.Children.Add(labelD);

					optionAButton.Clicked += async (sender, e) => {
						int optionChosen = 1;
						await GetNextQuestionObject(questionNum);
						if (isCorrect (questionNum, optionChosen)) {
							optionDButton.Text = " O ";
							optionAButton.BackgroundColor = Color.Green;
							labelA.TextColor = Color.Green;
						} else {
							optionDButton.Text = " X ";
							optionAButton.BackgroundColor = Color.Red;
							labelA.TextColor = Color.Red;
						}
						ToNextQuestion (optionChosen);
						IsEnabled = false;
					};

					optionBButton.Clicked += async (sender, e) => {
						int optionChosen = 2;
						await GetNextQuestionObject(questionNum);
						if (isCorrect (questionNum, optionChosen)) {
							optionDButton.Text = " O ";
							optionBButton.BackgroundColor = Color.Green;
							labelB.TextColor = Color.Green;
						} else {
							optionDButton.Text = " X ";
							optionBButton.BackgroundColor = Color.Red;
							labelB.TextColor = Color.Red;
						}
						ToNextQuestion (optionChosen);
						IsEnabled = false;
					};

					optionCButton.Clicked += async (sender, e) => {
						int optionChosen = 3;
						await GetNextQuestionObject(questionNum);
						if (isCorrect (questionNum, optionChosen)) {
							optionDButton.Text = " O ";
							optionCButton.BackgroundColor = Color.Green;
							labelC.TextColor = Color.Green;
						} else {
							optionDButton.Text = " X ";
							optionCButton.BackgroundColor = Color.Red;
							labelC.TextColor = Color.Red;
						}
						ToNextQuestion (optionChosen);
						IsEnabled = false;
					};

					optionDButton.Clicked += async (sender, e) => {
						int optionChosen = 4;
						await GetNextQuestionObject(questionNum);
						if (isCorrect (questionNum, optionChosen)) {
							optionDButton.Text = " O ";
							optionDButton.BackgroundColor = Color.Green;
							labelD.TextColor = Color.Green;
						} else {
							optionDButton.Text = " X ";
							optionDButton.BackgroundColor = Color.Red;
							labelD.TextColor = Color.Red;
						}
						ToNextQuestion (optionChosen);
						IsEnabled = false;
					};

					if (HasImage (currentObj)) {
						photo = new Image { Aspect = Aspect.AspectFit };
						photo.Source = ImageSource.FromUri (new Uri ( (string) currentObj["URI"]));
					}

					timer.Elapsed += untilTimeIsUp;

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

		public static void DetermineQuiz() {
			string quizName = QuizMenu.getQuizName ();

			if (quizName.Equals("Trivia")) {
				questionNum = triviaNum;
				points = triviaPoints;
				questionList = "TriviaQuestions";
			} else if (quizName.Equals ("Sales")) {
				questionNum = salesNum;
				points = salesPoints;
				questionList = "SalesQuestions";
			}
		}

		private void SetCounters(Label countdownBar) {
			string markers = "";
			for (int i = 0; i < secondsLeft; i ++) {
				markers = markers + "o";
			}

			Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
				countdownBar.Text = markers;
			});
		}

		private void ToNextQuestion (int optionChosen) {
			string quizName = QuizMenu.getQuizName ();
			SaveEnviron ();
			if (isCorrect (questionNum, optionChosen)) {
				if (quizName.Equals("Trivia")) {
					triviaPoints += secondsLeft;
					points = triviaPoints;
				} else if (quizName.Equals ("Sales")) {
					salesPoints += secondsLeft;
					points = salesPoints;
				}
				totalPoints = salesPoints + triviaPoints;
			}

			if (quizName.Equals("Trivia")) {
				triviaNum++;
				questionNum = triviaNum;
			} else if (quizName.Equals ("Sales")) {
				salesNum++;
				questionNum = salesNum;
			}

			timer.Stop ();
			timer = null;
			this.Navigation.PushAsync (new QuestionPage (obj));
		}

		private bool isCorrect (long questionNum, double optionChosen) {
			double solution = (double) currentObj ["SolutionNum"];
			Console.WriteLine ("Solution " + solution.ToString ());
			Console.WriteLine (solution == optionChosen);
			Console.WriteLine ("Number " + obj ["Number"]);
			return (solution == optionChosen);	
		}

		private async Task<ParseObject> GetNextQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();
			var query = from question in ParseObject.GetQuery (questionDB)
					where question.Get<long>("Number") == (questionNum + 1)
				select question;
			ParseObject obj = await query.FirstAsync();
			this.obj = obj;
			return obj;
		}

		private string generateQuestion (long questionNum) {
			string question = (string) currentObj ["Question"];
			return question;
		}

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

		private string generateA (long questionNum) {
			string a = (string) currentObj ["A"];
			return a;
		}

		private string generateB (long questionNum) {
			string b = (string) currentObj ["B"];
			return b;
		}

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

		public static void SaveEnviron() {
			var user = ParseUser.CurrentUser;
			user ["OverallPoints"] = totalPoints;
			user ["SalesPoints"] = salesPoints;
			user ["TriviaPoints"] = triviaPoints;
			user ["CurrentSales"] = salesNum;
			user ["CurrentTrivia"] = triviaNum;
			user.SaveAsync ();
		}

		public static void SetEnviron() {
			var user = ParseUser.CurrentUser;
			Quizes.QuestionPage.totalPoints =  (long) user ["OverallPoints"];
			Quizes.QuestionPage.salesPoints = (long) user ["SalesPoints"];
			Quizes.QuestionPage.triviaPoints = (long) user ["TriviaPoints"];
			Quizes.QuestionPage.salesNum = (long) user ["CurrentSales"];
			Quizes.QuestionPage.triviaNum = (long) user ["CurrentTrivia"];
		}

		private bool HasImage (ParseObject currentObj) {
			try {
				return (currentObj ["URI"] != null);
			} catch (KeyNotFoundException e) {
				Console.WriteLine (e.Message);
				return false;
			}
		}
	}

	public class ResultSummary : ContentPage {
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
			Content = sl;
		}
	}

	public class TimesUpPage : ContentPage {
		ParseObject obj;
		public TimesUpPage(ParseObject obj) {

			BackgroundColor = Color.FromHex("#2c3e50");
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();

			Label pageTitle = new Label () {
				Text = "Quiz Paused",
				TextColor = Color.White,
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
				Console.WriteLine("Continue > questionNum " + QuestionPage.questionNum);
				if (QuestionPage.questionNum <= QuizMenu.getTotalQuestions()) {
					Device.BeginInvokeOnMainThread (async () => {
						obj = await GetNextQuestionObject(QuestionPage.questionNum);
						await this.Navigation.PopModalAsync();
						await ProjectEcclesia.App.NavPage.PushAsync(new QuestionPage(obj));
						//await this.Navigation.PopModalAsync();
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
//				ProjectEcclesia.App.NavPage.PopToRootAsync();
				await this.Navigation.PopModalAsync();
				await ProjectEcclesia.App.NavPage.PopToRootAsync();
				await ProjectEcclesia.App.NavPage.PushAsync(new ProjectEcclesia.MainMenuPage());
			};

			logOutButton.Clicked += async (sender, e) => {
				QuestionPage.SaveEnviron();
				ParseUser.LogOut();
//				ProjectEcclesia.App.NavPage.PopToRootAsync();
//				this.Navigation.PopModalAsync();
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

		private async Task<ParseObject> GetNextQuestionObject (long questionNum) {
			string questionDB = QuizMenu.GetQuestionList ();

			string quizName = QuizMenu.getQuizName ();

			if (quizName.Equals("Trivia")) {
				QuestionPage.triviaNum++;
				QuestionPage.questionNum = QuestionPage.triviaNum;
			} else if (quizName.Equals ("Sales")) {
				QuestionPage.salesNum++;
				QuestionPage.questionNum = QuestionPage.salesNum;
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


