using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Xamarin.Forms;
using Xamarin.Ecclesia.Auth;


namespace Xamarin.Ecclesia.WinPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        OAuthCommunicator _oAuth;


        public MainPage()
        {
            InitializeComponent();
            Content = Xamarin.Ecclesia.App.GetMainPage().ConvertPageToUIElement(this);


            _oAuth = new OAuthCommunicator();
            _oAuth.AuthUIRequest += oAuth_AuthUIRequest;
            AuthHelper.OAuthCommunicator = _oAuth;
        }

        void oAuth_AuthUIRequest()
        {
            Uri uri = _oAuth.FBAuthenticator.GetUI();
            NavigationService.Navigate(uri);
            
        }
    }
}
