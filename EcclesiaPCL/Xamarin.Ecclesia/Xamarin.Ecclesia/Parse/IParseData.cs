using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Accounts;

namespace Xamarin.Ecclesia.Parse
{
    public interface IParseData
    {
        UserAccount GetCurrentAccount();
        Task<UserAccount> RegisterAccount(string email, string socialId, string firstName, string lastName);
        Task<UserAccount> SigInAccount(string email, string socialId);
    }
}
