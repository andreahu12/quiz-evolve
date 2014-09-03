using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Ecclesia.Models.Utils;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Auth;


namespace Xamarin.Ecclesia.Droid
{
    [Activity(Label = "Xamarin.Ecclesia", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        OAuthCommunicator _oAuth;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            SetPage(App.GetMainPage());
            XMLHelper.XMLLoader =new XMLLoader(this);

            _oAuth = new OAuthCommunicator();
            _oAuth.AuthUIRequest += oAuth_AuthUIRequest;
            AuthHelper.OAuthCommunicator = _oAuth;

        }

        void oAuth_AuthUIRequest()
        {
            var intent = _oAuth.FBAuthenticator.GetUI(this);
            StartActivity(intent);
        }
    }
}

