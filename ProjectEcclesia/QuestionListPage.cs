using System;
using Xamarin.Forms;
using Parse;
using System.Collections.Generic;

namespace Quizes {

	public class QuestionListPage : ContentPage {

		static long goToQuestionNumber = 1;

		public QuestionListPage () {
			BackgroundColor = Color.FromHex ("#ecf0f1");
			NavigationPage.SetHasNavigationBar (this, false);

			StackLayout vl = ProjectEcclesia.HelperMethods.createVertSL();
			StackLayout hl = ProjectEcclesia.HelperMethods.createHorizSL();

			Button toQuizMenu = new Button () {
				Text = "Quiz Menu",
			};

			vl.Children.Add (toQuizMenu);

			Label pageTitle = new Label () {
				Text = QuizMenu.getQuizName() + " Questions",
			};

			vl.Children.Add (pageTitle);

			List<Button> buttonList = GenerateButtons ();

			foreach (Button item in buttonList) {
				if (hl.Children.Count == 8) {
					vl.Children.Add (hl);
					hl = ProjectEcclesia.HelperMethods.createHorizSL ();
					hl.Children.Add (item);
				} else {
					hl.Children.Add (item);
				}
			}

			toQuizMenu.Clicked += (sender, e) => {
				this.Navigation.PopAsync();
			};
				
			Content = vl;
		}

		List<Button> GenerateButtons () {
			List<Button> buttonList = new List<Button>();
			for (long i = 0; i < Quizes.QuizMenu.getTotalQuestions(); i++) {
				long num = i + 1;

				Button button = new Button () {
					Text = num.ToString(),
					TextColor = Color.White,
					BackgroundColor = Color.FromHex("#2c3e50"),
				};

				Console.WriteLine (button.Text);

				button.Clicked += async (sender, e) => {
					goToQuestionNumber = Convert.ToUInt32(button.Text);
					await this.Navigation.PushAsync(new Quizes.QuizInstructions());
				};

				buttonList.Add(button);
			}
			return buttonList;
		}

		public static long GetGoToQuestionNumber() {
			return goToQuestionNumber;
		}
	}
}

