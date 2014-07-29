using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Parse;


namespace ProjectEcclesia.Android
{
	[Activity (Label = "ProjectEcclesia.Android.Android", MainLauncher = true)]
	public class MainActivity : AndroidActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			Xamarin.Forms.Forms.Init (this, bundle);

			// Initialize the parse client with your Application ID and .NET Key found on
			// your Parse dashboard
			ParseClient.Initialize("CTD7gv8TT58loz9xR9ezaZN8ztZZr06HltauRHKv",
				"AqOAZGE8wouNsbXWbm0cSDMdzh1U5KTbNC4UnR4s");

			SetPage (App.GetMainPage ());
		}
	}
}

