﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Ecclesia.Auth
{
    public interface IOAuthCommunicator
    {
        Task<bool> AuthWithFacebook();
    }
}
