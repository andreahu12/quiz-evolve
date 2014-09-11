using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Xamarin.Ecclesia.Models.Utils;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Auth;
using Parse;
using Xamarin.Ecclesia.Settings;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        #region Fields
        //main app windows
        UIWindow _window;
        //app view controller
        UIViewController _vc;
        //OAuth communicator class for Facebook/LinkedIn
        OAuthCommunicator _oAuth;
        #endregion

        
        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this 
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //Initialize Xamarin.Forms
            Xamarin.Forms.Forms.Init();
            //XML loader instance
            XMLHelper.XMLLoader =new XMLLoader();

			//Initialize settings
            AppSettings.Init(new LocalSettings());

            // Initialize the parse client with your Application ID and .NET Key found on
            // your Parse dashboard
            ParseClient.Initialize(Ecclesia.Settings.Constants.ParseID, Ecclesia.Settings.Constants.ParseNETKey);
			ParseHelper.ParseData = new ParseData ();

            //set main window 
            _window = new UIWindow(UIScreen.MainScreen.Bounds);
            _vc=App.GetMainPage().CreateViewController();
            _window.RootViewController = _vc;
            //and show it
            _window.MakeKeyAndVisible();

            //OAuth communicator instance
            _oAuth = new OAuthCommunicator();
            AuthHelper.OAuthCommunicator = _oAuth;
            //register authhandlers
            _oAuth.AuthUIRequest += _oAuth_AuthUIRequest;
			_oAuth.AuthFinished +=	_oAuth_AuthFinished;
            return true;
        }

        /// <summary>
        /// Request to show login view 
        /// </summary>
        void _oAuth_AuthUIRequest()
        {
            UIViewController vc = _oAuth.Authenticator.GetUI();
                        
            _window.RootViewController = vc;
            _window.MakeKeyAndVisible();

            UIApplication.SharedApplication.Windows[0] = _window;
        }

        /// <summary>
        /// Login completed, return to main app view
        /// </summary>
		void _oAuth_AuthFinished()
		{
			_window.RootViewController = _vc;
			_window.MakeKeyAndVisible();
			UIApplication.SharedApplication.Windows[0] = _window;
		}
    }
}
