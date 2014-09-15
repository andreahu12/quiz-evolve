using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;
using Xamarin.Ecclesia.XML;
using Xamarin.Ecclesia.Parse;

namespace Xamarin.Ecclesia.ViewModels
{
    public class MyInfoViewModel : QuizDataBaseViewModel
    {
        #region Constructor
        public MyInfoViewModel()
            : base()
        {
            Title = "MyInfo";
            BackgroundColor = AppSettings.PageBackgroundColor;
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return AppSettings.CurrentAccount.FullName;
            }
        }

        public ImageSource Avatar
        {
            get
            {
                return ImageSource.FromUri(new Uri(AppSettings.CurrentAccount.ImageUrl));
            }
        }

        #endregion

        #region Methods
        
        #endregion
    }
}
