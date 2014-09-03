using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.Views;
using Xamarin.Ecclesia.XML;
using Xamarin.Forms;

namespace Xamarin.Ecclesia
{
    public class App
    {
        public static Page GetMainPage()
        {
            return new NavigationPage( new LoginPage());
        }
               
    }
}
