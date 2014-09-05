using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Xamarin.Ecclesia.Auth
{
    public static class AuthHelper
    {
        public static IOAuthCommunicator OAuthCommunicator{get;set;}

        public async static Task<bool> AuthWithFacebook()
        {
            return await OAuthCommunicator.AuthWithFacebookAsync();
        }
    }
}
