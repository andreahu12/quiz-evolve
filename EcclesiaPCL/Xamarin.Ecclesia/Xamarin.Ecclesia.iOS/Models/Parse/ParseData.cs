using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Ecclesia.Accounts;
using Parse;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Settings;

namespace Xamarin.Ecclesia.Parse
{
    public class ParseData:IParseData
    {
        public UserAccount GetCurrentAccount()
        {
            if (ParseUser.CurrentUser != null)
            {
                return AccountFromParseUser(ParseUser.CurrentUser);
            }
            return null;
        }

        public async Task<UserAccount> SigInAccountAsync(string email, string socialId)
        {
            var user = await ParseUser.LogInAsync(email, socialId);
            SaveLocal(email, socialId);
            return AccountFromParseUser(user);
        }
        
        public async Task<UserAccount> RegisterAccountAsync(string email, string socialId, string firstName, string lastName)
        {
            bool isRegistered = false;

            var user = new ParseUser()
            {
                Username = email,
                Email = email,
                Password = socialId,
            };

            user["first_name"] = firstName;
            user["last_name"] = lastName;
            try
            {
                await user.SignUpAsync();
                SaveLocal(email, socialId);
            }
            catch (ParseException p)
            {
                //TODO: Log it
                isRegistered = true;
            }
            catch (Exception a)
            {
                //TODO: Log it
				var t = a.Message;
            }
            
            if (isRegistered)
                return await SigInAccountAsync(email, socialId);
            else
                return AccountFromParseUser(user);
        }

        UserAccount AccountFromParseUser(ParseUser parseUser)
        {
            var account = new UserAccount();
            account.FirstName = parseUser["first_name"].ToString();
            account.LastName = parseUser["last_name"].ToString();
            account.ID = parseUser.ObjectId;
            return account;
        }

        void SaveLocal(string email, string socialId)
        {
            AppSettings.AccountEmail = email;
            AppSettings.AccountID = socialId;
        }
    }
}