using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace Xamarin.Ecclesia.Auth
{
    public class OAuthCommunicator : IOAuthCommunicator
    {
        #region Constatnts
        const string FBAppID = "438894422873149";
        #endregion

        #region Fields
        #endregion
        
        #region Events
        public event Action AuthUIRequest;
        //public event Action AuthCompleted;
        //public event Action AuthFailed;
        #endregion

        #region Properties
        public Account FBAccount { get; private set; }
        public OAuth2Authenticator FBAuthenticator { get; private set; }
        public string AccessToken { get; set; }
        public string KidAccessToken { get; private set; }
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(AccessToken);
            }
        }
                
        #endregion

        #region Methods
        public async Task<bool> AuthWithFacebook()
        {
            var tcs = new TaskCompletionSource<bool>();

            if (IsAuthenticated)
            {
                //AuthCompleted();
                tcs.SetResult(true);
            }
            else
            {
                FBAuthenticator = new OAuth2Authenticator(
                    clientId: FBAppID,
                    scope: "",
                    authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                    redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

                // If authorization succeeds or is canceled, .Completed will be fired.
                // TODO: complited should be only on success following with succesfull API communication
                FBAuthenticator.Completed += (s, ee) =>
                {
                    //if (AuthCompleted != null)
                    //    AuthCompleted();
                    if (!ee.IsAuthenticated)
                    {
                        return;
                    }
                    FBAccount = ee.Account;

                    tcs.SetResult(true);
                };

                FBAuthenticator.Error += (sender, e) =>
                    {
                        tcs.SetResult(false);
                    };

                if (AuthUIRequest != null)
                    AuthUIRequest();
            }
            return await tcs.Task;
        }

        void FBAuthenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
