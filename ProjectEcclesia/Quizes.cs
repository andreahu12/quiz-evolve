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

			toSalesQuiz.Clicked += async (sender, e) => {
				totalQuestions = await GetNumQuestions("SalesQuestions");
				quizName = "Sales";
				await this.Navigation.PushAsync(new QuizInstructions());
			};

			toTrivaQuiz.Clicked += async (sender, e) => {
				totalQuestions = await GetNumQuestions("TriviaQuestions");
				quizName = "Trivia";
				await this.Navigation.PushAsync(new QuizInstructions());
			};

			sl.Children.Add (toSalesQuiz);
			sl.Children.Add (toTrivaQuiz);
			Content = sl;
		}

		private async Task<int> GetNumQuestions(string parseDBName) {
			var query = from questions  in ParseObject.GetQuery (parseDBName)
						where (long) questions["Number"] > 0
			            select questions;
			var count = await query.CountAsync ();
			return count;
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

				Image photo = new Image();


				Xamarin.Forms.Device.BeginInvokeOnMainThread (() => {
					SetCounters(countdownBar);

					Label questionLabel = new Label () {
						Text = string.Format ("\n{0}. {1}\n\n", questionNum, question),
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

					Label labelB = new Label() {
						Text = optionB,
						TextColor = Color.Black,
					};

					Label labelC = new Label() {
						Text = optionC,
						TextColor = Color.Black,
					};

					Label labelD = new Label() {
						Text = optionD,
						TextColor = Color.Black,
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
//						await GetNextQuestionObject(questionNum);
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
//						await GetNextQuestionObject(questionNum);
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
//						await GetNextQuestionObject(questionNum);
						await CodeWordCheck(optionDButton, labelD, questionNum, optionChosen);
						if (isCorrect(questionNum, optionChosen)) {
							ToNextQuestion (optionChosen);
						}
						IsEnabled = false;
					};

					exitButton.Clicked += async (sender, e) => {
						await GetNextQuestionObject(questionNum);
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

		private async Task CodeWordCheck (Button currentButton, Label letterLabel, long questionNum, int optionChosen) {
			if (isCorrect (questionNum, optionChosen)) {
				timer.Stop ();
				currentButton.BackgroundColor = Color.Green;
				letterLabel.TextColor = Color.Green;
				await DisplayAlert("Correct!", string.Format("+{0}", secondsLeft), "Continue", null);
			} else {
				timer.Stop ();
				currentButton.BackgroundColor = Color.Red;
				letterLabel.TextColor = Color.Red;
				bool alert = await DisplayAlert("Incorrect!", "Would you like to ask a rep for the solution (and codeword) so you can try again for potential partial credit?" , "Yes", "No");
				Console.WriteLine ("bool alert: " + alert);
				if (alert) {
					secondsLeft = secondsLeft / 2;
//					string question = generateQuestion (questionNum);
					await this.Navigation.PushModalAsync(new EnterCodewordPage(currentObj));
				} else {
					ToNextQuestion (optionChosen);
				}
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
//			Console.WriteLine ("Number " + obj ["Number"]);
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

	public class EnterCodewordPage : ContentPage {

		public EnterCodewordPage(ParseObject currentObject) {
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();
			BackgroundColor = Color.FromHex ("#2c3e50");
			Label instructionsLabel = new Label () {
				Text = "Please ask a representative for the correct answer and the codeword.\n",
				TextColor = Color.FromHex("#b455b6"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			string questionNum = currentObject ["Number"].ToString();
			string question = currentObject ["Question"].ToString ();
			string a = "[A] " + currentObject ["A"].ToString ();
			string b = "[B] " + currentObject ["B"].ToString ();
			string c, d, questionLabelText;
			try {
				c = "[C] " + currentObject ["C"].ToString ();
				d = "[D] " + currentObject ["D"].ToString ();
				questionLabelText = string.Format ("{0}. {1}\n{2}\n{3}\n{4}\n{5}\n", questionNum, question, a, b, c, d);
			} catch (KeyNotFoundException k) {
				Console.WriteLine (k.Message);
				questionLabelText = string.Format ("{0}. {1}\n{2}\n{3}\n", questionNum, question, a, b);
			}

			Label questionLabel = new Label () {
				Font = Font.SystemFontOfSize(NamedSize.Small),
				TextColor = Color.White,
				Text = questionLabelText,
			};

			Entry codewordEntry = new Entry () {
				Placeholder = "Code Word",
				IsPassword = true,
			};

			Button submitButton = new Button () {
				Text = "Submit",
				BackgroundColor = Color.FromHex("#3498db"),
				TextColor = Color.White,
			};

			Button exitButton= new Button () {
				Text = "Exit Quiz",
				TextColor = Color.Silver,
			};

			submitButton.Clicked += async (sender, e) => {
				string correctCodeWord = SetCorrectCodeWord();
				if (correctCodeWord.Equals(codewordEntry.Text)) {
					await DisplayAlert("Correct!", "You may return to the question.", "Continue", null);
					await this.Navigation.PopModalAsync();
				} else {
					await DisplayAlert("Incorrect", "Please try again.", "Retry", null);
					codewordEntry.Text = "";
				}
			};

			exitButton.Clicked += async (sender, e) => {
				await this.Navigation.PopModalAsync ();
				await ProjectEcclesia.App.NavPage.PushAsync (new ProjectEcclesia.MainMenuPage ());
			};

			sl.Children.Add (instructionsLabel);
			sl.Children.Add (questionLabel);
			sl.Children.Add (codewordEntry);
			sl.Children.Add (submitButton);
			sl.Children.Add (exitButton);
			Content = sl;
		}

		private string SetCorrectCodeWord() {
			string quizName = QuizMenu.getQuizName ();
			if (quizName.Equals ("Sales")) {
				return "grapple";
			} else if (quizName.Equals ("Trivia")) {
				return "peach";
			} return "";
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