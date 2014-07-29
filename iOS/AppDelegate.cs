using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Parse;

namespace SalesPlaybookApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;

		public AppDelegate() {
			// Initialize the Parse client with your Application ID and .NET Key found on
			// your Parse dashboard
			ParseClient.Initialize("CTD7gv8TT58loz9xR9ezaZN8ztZZr06HltauRHKv",
				"AqOAZGE8wouNsbXWbm0cSDMdzh1U5KTbNC4UnR4s");
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Forms.Init ();

			window = new UIWindow (UIScreen.MainScreen.Bounds);

			window.RootViewController = ProjectEcclesia.App.GetMainPage ().CreateViewController ();
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}