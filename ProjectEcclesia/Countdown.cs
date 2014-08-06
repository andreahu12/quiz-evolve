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
		 * Contains constructor for a countdown page
		 * that counts down from 3 seconds to when the timed quiz starts.
		 * Also passes down question obj to the next question page.
		 * */

	public class Countdown : ContentPage {

		Label countdownLabel = new Label ();
		int secondsLeft = 3;
		System.Timers.Timer timer;
		ParseObject obj;

		/**
			 * Constructor for Countdown page for quiz.
			 * Passes down the calculated question object (obj) that was queried for
			 * in the instructions page.
			 * */
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

		/**
			 * <summary>
			 * Displays countdown on a label.
			 * </summary>
			 * */
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
}