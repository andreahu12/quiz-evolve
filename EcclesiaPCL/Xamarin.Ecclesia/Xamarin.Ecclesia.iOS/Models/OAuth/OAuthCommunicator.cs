using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Ecclesia.Parse;

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
        public event Action AuthFinished;
        //public event Action AuthFailed;
        #endregion

        #region Properties
        public Account FBAccount { get; private set; }
        public OAuth2Authenticator FBAuthenticator { get; private set; }
        #endregion

        #region Methods
        public async Task<bool> AuthWithFacebookAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            FBAuthenticator = new OAuth2Authenticator(
                clientId: FBAppID,
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

            // If authorization succeeds or is canceled, .Completed will be fired.
            // TODO: complited should be only on success following with succesfull API communication
            FBAuthenticator.Completed += async(s, ee) =>
            {
                if (AuthFinished != null)
                    AuthFinished();

                if (!ee.IsAuthenticated)
                {
					tcs.SetResult(false);
                }
				else
				{
                    FBAccount = ee.Account;
					await GetFBInfoAsync();
					tcs.SetResult(true);
				}
            };

            FBAuthenticator.Error += (sender, e) =>
                {
                    tcs.SetResult(false);
                };

            if (AuthUIRequest != null)
                AuthUIRequest();
            return await tcs.Task;
        }
        public async Task GetFBInfoAsync()
        {
            // Now that we're logged in, make a OAuth2 request to get the user's info.
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, FBAccount);
            try
            {
                Response response = await request.GetResponseAsync();
                var str =  response.GetResponseText();
                var obj = JsonValue.Parse(str);

				var email = obj["email"].OfType<string>().First();
                var name = obj["name"];
                var facebookID = obj["id"];
                await ParseHelper.ParseData.RegisterAccountAsync(email, facebookID, name, "");
            }
            catch (Exception ex)
            {
                //LogManager.Log(ClassName, new Exception("Was not able to get facebook name", ex));
            }
            
        }
              

        #endregion
    }
}
