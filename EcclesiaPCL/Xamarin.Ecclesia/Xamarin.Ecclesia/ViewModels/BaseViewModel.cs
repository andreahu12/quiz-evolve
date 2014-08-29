using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Xamarin.Ecclesia.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Constructor
        public BaseViewModel() { }
        #endregion

        #region Fields
        private bool _isBusy;
        string _errorMessage;  
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
        
        protected void NotifyPropertyChanged([CallerMemberName] string property=null)
        {
            if (PropertyChanged != null) 
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    NotifyPropertyChanged("IsBusy");
                }
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    NotifyPropertyChanged("ErrorMessage");
                }
            }
        }

        #endregion

                
    }
}
