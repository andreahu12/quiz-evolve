using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
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

        const string LIAppID = "75xsd3obqg5pz8";
        const string LISecret = "LqhSj5h17jwUgSdW";
        #endregion

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
                clientId: FBAppID,
                scope: "",
                authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));

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
					await GetFBInfoAsync();
					tcs.SetResult(true);
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
        public async Task GetFBInfoAsync()
        {
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

				var email = obj["email"];
                var firstName = obj["first_name"];
                var lastName = obj["last_name"];
                var facebookID = obj["id"];
                await ParseHelper.ParseData.RegisterAccountAsync(email, facebookID, firstName, lastName);
            }
            catch (Exception ex)
            {
                //LogManager.Log(ClassName, new Exception("Was not able to get facebook name", ex));
            }
            
        }

        public async Task<bool> AuthWithLinkedInAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            Authenticator = new OAuth2Authenticator(
                clientId: LIAppID,
                clientSecret: LISecret,
                scope: "r_fullprofile r_contactinfo",
                authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
                redirectUrl: new Uri("https://evolve.xamarin.com/"),
                accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")

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
					GetLIInfoAsync();
					tcs.SetResult(true);
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

        public void GetLIInfoAsync()
        {
            string dd = SocialAccount.Username;
            var values = SocialAccount.Properties;
            var access_token = values["access_token"];
            try
            {

                //var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v1/people/~:(id,firstName,lastName,headline,picture-url,summary,educations,three-current-positions,honors-awards,site-standard-profile-request,location,api-standard-profile-request,phone-numbers)?oauth2_access_token=" + access_token + "&format=json", ""));
                //request.ContentType = "application/json";
                //request.Method = "GET";

                //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                //{
                //    System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                //    {
                //        var content = reader.ReadToEnd();
                //        if (!string.IsNullOrWhiteSpace(content))
                //        {

                //            System.Console.Out.WriteLine(content);
                //        }
                //        var result = JsonConvert.DeserializeObject<dynamic>(content);
                //    }
                //}
            }
            catch (Exception exx)
            {
                System.Console.WriteLine(exx.ToString());
            }
        }
        #endregion
    }
}
