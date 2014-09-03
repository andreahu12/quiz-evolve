using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Ecclesia.Auth
{
    public interface IOAuthCommunicator
    {
        string AccessToken { get; set; }
        bool IsAuthenticated{get;}
        Task<bool> AuthWithFacebook();
    }
}
