using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Ecclesia.Settings
{
    public class Constants
    {
        #region Parse.com
        public const string ParseID = "CTD7gv8TT58loz9xR9ezaZN8ztZZr06HltauRHKv";
        public const string ParseNETKey = "AqOAZGE8wouNsbXWbm0cSDMdzh1U5KTbNC4UnR4s";
        #endregion

        #region Facebook auth
        public const string FBAppID = "438894422873149";
        public const string FBAuthURL = "https://m.facebook.com/dialog/oauth/";
        public const string FBRedirectURL = "http://www.facebook.com/connect/login_success.html";
        #endregion

        #region LinkedIn
        public const string LIAppID = "75xsd3obqg5pz8";
        public const string LISecret = "LqhSj5h17jwUgSdW";

        public const string LIAuthURL = "https://www.linkedin.com/uas/oauth2/authorization";
        public const string LIRedirectURL = "https://evolve.xamarin.com/";
        public const string LIAccessTokenURL = "https://www.linkedin.com/uas/oauth2/accessToken";
        
        #endregion

    }
}
