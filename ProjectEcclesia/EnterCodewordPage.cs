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
		 * Contains constructor to create the modally pushed EnterCodewordPage.
		 * This page gets a random but relevant representative and displays their name and picture.
		 * Also contains the question the user missed, and an entry to enter the codeword.
		 * */
	public class EnterCodewordPage : ContentPage {

		string repPicURL;

		/**
			 * Constructor to create an EnterCodewordPage
			 * */
		public EnterCodewordPage(ParseObject currentObject) {
			StackLayout sl = ProjectEcclesia.HelperMethods.createVertSL ();
			BackgroundColor = Color.FromHex ("#2c3e50");
			Label instructionsLabel = new Label () {
				Text = string.Format("Please talk to {0}.\n", SetRepName()),
				TextColor = Color.FromHex("#b455b6"),
				Font = Font.SystemFontOfSize(NamedSize.Large),
			};

			Image repPic = new Image { Aspect = Aspect.AspectFit };
			repPic.Source = ImageSource.FromUri (new Uri (repPicURL));

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
			sl.Children.Add (repPic);
			sl.Children.Add (questionLabel);
			sl.Children.Add (codewordEntry);
			sl.Children.Add (submitButton);
			sl.Children.Add (exitButton);
			Content = sl;
		}

		/**
			 * <summary>
			 * Sets the correct codeword based on which quiz the user is taking.
			 * </summary>
			 * */
		private string SetCorrectCodeWord() {
			string quizName = QuizMenu.getQuizName ();
			if (quizName.Equals ("Sales")) {
				return "grapple";
			} else if (quizName.Equals ("Trivia")) {
				return "peach";
			} else if (quizName.Equals ("People")) {
				return "pear";
			} return "";
		}

		/**
			 * <summary>
			 * Sets the representative to talk to.
			 * </summary>
			 * */
		private string SetRepName() {
			string quizName = QuizMenu.getQuizName ();
			if (quizName.Equals ("Sales")) {
				return RandomSalesRep ();
			} else if (quizName.Equals ("Trivia")) {
				return RandomTriviaRep ();
			} else if (quizName.Equals ("People")) {
				return RandomPeopleRep ();
			} return "";
		}

		/**
			 * <summary>
			 * Generates a random sales rep.
			 * </summary>
			 * */
		private string RandomSalesRep() {
			Random generator = new Random ();
			int num = generator.Next (3);
			Console.WriteLine ("random num " + num);
			if (num == 0) {
				repPicURL = "http://m.c.lnkd.licdn.com/mpr/pub/image-d62u-1_SRDbowCIEheYodL43P96kDRAECVN1dcXwPMfRarg4d621CzISPj-uYEDxglxn/arwa-kaddoura.jpg";
				return "Arwa Kaddoura";
			} else {
				repPicURL = "http://m.c.lnkd.licdn.com/mpr/pub/image-PyyhxnbTQg9OGHcVvo_7PCqcar2ozszVXyywPvsTa0dxQ7CxPyywXRVTapEyQ5sucfzR/kai-mak.jpg";
				return "Kai Mak";
			}
		}

		/**
			 * <summary>
			 * Generates a random trivia rep.
			 * </summary>
			 * */
		private string RandomTriviaRep() {
			Random generator = new Random ();
			int num = generator.Next (3);
			Console.WriteLine ("random num " + num);
			if (num == 0) {
				repPicURL = "https://www.linkedin.com/mpr/pub/image-4fabukm7F22P9oSCrP3AMrdnABkeJJph_tAlKkNsAfkpJsnh4fal9NK7APQyJ6Rr0oiN/laura-quest.jpg";
				return "Laura Quest";
			} else {
				repPicURL = "http://xamarin.com/images/about/robross.jpg";
				return "Rob Ross";
			}
		}

		/**
			 * <summary>
			 * Generates a random sales rep.
			 * </summary>
			 * */

		private string RandomPeopleRep() {
			Random generator = new Random ();
			int num = generator.Next (3);
			Console.WriteLine ("random num " + num);
			if (num == 0) {
				repPicURL = "http://m.c.lnkd.licdn.com/mpr/pub/image-bAlKSzAAfoJrzk4J2aNvbwImZlBBl2XKqA2B3zgQZzteAnJxbAlB3Y4AZwPLI2pdkH4A/victoria-englund-phr.jpg";
				return "Victoria Grothey";
			} else {
				repPicURL = "http://m.c.lnkd.licdn.com/mpr/pub/image-wo8k5a4FvXAGYdUPDyw9ZA_q_n6YDUpPFa86voWF_UHh7pFRwo86v-AF_8t2w0qci7k_/mallory-smith.jpg";
				return "Mallory Smith";
			}
		}
	}
}
