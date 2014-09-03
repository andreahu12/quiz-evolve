using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;
using Xamarin.Ecclesia.Models.Utils;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Auth;

namespace Xamarin.Ecclesia.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations
        UIWindow window;
        UIViewController _vc;
        OAuthCommunicator _oAuth;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            XMLHelper.XMLLoader =new XMLLoader();
            window = new UIWindow(UIScreen.MainScreen.Bounds);
            _vc=App.GetMainPage().CreateViewController();
            window.RootViewController = _vc;

            window.MakeKeyAndVisible();

            _oAuth = new OAuthCommunicator();
            AuthHelper.OAuthCommunicator = _oAuth;
            _oAuth.AuthUIRequest += _oAuth_AuthUIRequest;

            return true;
        }

        void _oAuth_AuthUIRequest()
        {
            UIViewController vc = _oAuth.FBAuthenticator.GetUI();
                        
            window.RootViewController = vc;
            window.MakeKeyAndVisible();

            UIApplication.SharedApplication.Windows[0] = window;
        }
    }
}
