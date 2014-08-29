using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Xamarin.Ecclesia.Settings
{
    public class AppSettings
    {
        #region Constants
        


        const int _numOfRuns = 0;
        #endregion

        #region Fields
        static ISettingsContainer _localSettings;

        public static Color PageBackgroundColor = Color.Transparent;
        #endregion

        #region Properties
        
        public static int NumOfRuns
        {
            get
            {
                if (_localSettings.GetValue("NumOfRuns") == null)
                {
                    _localSettings.SetValue("NumOfRuns", _numOfRuns);
                    return _numOfRuns;
                }
                return Convert.ToInt32(_localSettings.GetValue("NumOfRuns"));
            }
            set
            {
                _localSettings.SetValue("NumOfRuns", value);

            }
        }

        
        #endregion

        #region Methods
        public static void Init(ISettingsContainer settingsContainer)
        {
            _localSettings = settingsContainer;
        }
        #endregion
    }
}
