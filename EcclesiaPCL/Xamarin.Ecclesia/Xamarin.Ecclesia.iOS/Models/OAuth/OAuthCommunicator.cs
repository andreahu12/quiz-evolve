using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Ecclesia.DataObjects;
using Xamarin.Ecclesia.Parse;
using Xamarin.Ecclesia.Settings;

namespace Xamarin.Ecclesia.Auth
{
    public class OAuthCommunicator : IOAuthCommunicator
    {
        #region Fields
        #endregion
        
        #region Events
        public event Action AuthUIRequest;
        public event Action AuthFinished;
        #endregion

        #region Properties
        public Account SocialAccount { get; private set; }
        public OAuth2Authenticator Authenticator { get; private set; }
        #endregion

        #region Methods
        public async Task<bool> AuthWithFacebookAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            Authenticator = new OAuth2Authenticator(
                clientId: Constants.FBAppID,
                scope: "",
                authorizeUrl: new Uri(Constants.FBAuthURL),
                redirectUrl: new Uri(Constants.FBRedirectURL));

            Authenticator.AllowCancel = true;
            // If authorization succeeds or is canceled, .Completed will be fired.
            // TODO: complited should be only on success following with succesfull API communication
            Authenticator.Completed += async(s, ee) =>
            {
                if (AuthFinished != null)
                    AuthFinished();

                if (!ee.IsAuthenticated)
                {
					tcs.SetResult(false);
                }
				else
				{
                    SocialAccount = ee.Account;
					tcs.SetResult(await GetFBInfoAsync());
				}
            };

            Authenticator.Error += (sender, e) =>
                {
                    tcs.SetResult(false);
                };

            if (AuthUIRequest != null)
                AuthUIRequest();
            return await tcs.Task;
        }
        public async Task<bool> GetFBInfoAsync()
        {
            UserAccount user = null;
            // Now that we're logged in, make a OAuth2 request to get the user's info.
            var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, SocialAccount);
            try
            {
                Response response = await request.GetResponseAsync();
                string str;
#if WINDOWS_PHONE
                str = await response.GetResponseTextAsync();
#else
                str = response.GetResponseText();
#endif
                var obj = JsonValue.Parse(str);
                var id = obj["id"];
				var email = obj["email"];
                var firstName = obj["first_name"];
                var lastName = obj["last_name"];
                var imageUrl = string.Format("http://graph.facebook.com/{0}/picture?type=square", id).Replace("\"","");
                user = await ParseHelper.ParseData.RegisterAccountAsync(email, firstName, lastName,imageUrl);
            }
            catch (Exception ex)
            {
                ParseHelper.ParseData.LogException(ex);
            }
            AppSettings.CurrentAccount = user;
            return user != null;
        }

        public async Task<bool> AuthWithLinkedInAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            Authenticator = new OAuth2Authenticator(
                clientId: Constants.LIAppID,
                clientSecret: Constants.LISecret,
                scope: "r_fullprofile r_emailaddress",
                authorizeUrl: new Uri(Constants.LIAuthURL),
                redirectUrl: new Uri(Constants.LIRedirectURL),
                accessTokenUrl: new Uri(Constants.LIAccessTokenURL)
            );

            // If authorization succeeds or is canceled, .Completed will be fired.
            Authenticator.AllowCancel = true;
            Authenticator.Completed += async(s, ee) =>
            {
                if (AuthFinished != null)
                    AuthFinished();

                if (!ee.IsAuthenticated)
                {
					tcs.SetResult(false);
                }
				else
				{
                    SocialAccount = ee.Account;
					tcs.SetResult(await GetLIInfoAsync());
				}
            };

            Authenticator.Error += (sender, e) =>
                {
                    tcs.SetResult(false);
                };

            if (AuthUIRequest != null)
                AuthUIRequest();
            return await tcs.Task;
        }

        public async Task<bool> GetLIInfoAsync()
        {
            string dd = SocialAccount.Username;
            var values = SocialAccount.Properties;
            var access_token = values["access_token"];
            UserAccount user=null;
            try
            {

                var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~:(id,firstName,lastName,picture-url,email-address)?oauth2_access_token=" + access_token + "&format=json", ""));
                request.ContentType = "application/json";
                request.Method = "GET";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(content))
                        {

                            System.Console.Out.WriteLine(content);
                        }
                        var obj = JsonValue.Parse(content);
                        var email = obj["emailAddress"];
                        var firstName = obj["firstName"];
                        var lastName = obj["lastName"];
                        var imageUrl = obj["pictureUrl"];
                        user =await ParseHelper.ParseData.RegisterAccountAsync(email, firstName, lastName,imageUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                ParseHelper.ParseData.LogException(ex);
            }
            AppSettings.CurrentAccount = user;
            return user!=null;
        }
        #endregion
    }
}
