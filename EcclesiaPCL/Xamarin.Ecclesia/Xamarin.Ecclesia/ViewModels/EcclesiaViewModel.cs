using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class EcclesiaViewModel : BaseViewModel
    {
        #region Fields
        string _title;

        protected Random Rnd = new Random(); 
        Color _backgroundColor;
        #endregion

        #region Properties
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                    NotifyPropertyChanged("TitleCapital");
                }
            }
        }

        public string TitleCapital
        {
            get
            {
                return Title.ToUpper();
            }
        }

        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
                NotifyPropertyChanged();
            }
        }

        public EcclesiaViewModel Parent
        { get; set; }
        #endregion

        #region Methods
        
        #endregion
    }
}
