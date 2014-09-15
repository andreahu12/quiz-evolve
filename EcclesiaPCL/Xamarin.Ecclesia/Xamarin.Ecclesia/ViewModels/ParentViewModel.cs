using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public abstract class ParentViewModel : EcclesiaViewModel
    {
        #region Constructor
        public ParentViewModel()
        {
            Children = new ObservableCollection<EcclesiaViewModel>();
        }
        #endregion

        #region Fields
        ObservableCollection<EcclesiaViewModel> _children;

        EcclesiaViewModel _selectedChild;
        #endregion

        #region Properties

        public ObservableCollection<EcclesiaViewModel> Children
        {
            get 
            {
                return _children; 
            }
            set
            {
                if (_children != value)
                {
                    _children = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public EcclesiaViewModel SelectedChild
        {
            get
            {
                return _selectedChild;
            }
            set
            {
                if (_selectedChild == value)
                {
                    _selectedChild = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Methods
        public void AddChild(EcclesiaViewModel child)
        {
            if (!_children.Contains(child))
            {
                _children.Add(child);
                child.Parent = this;
                NotifyPropertyChanged("Children");
            }
        }

        public void AddChildRandomly(EcclesiaViewModel child)
        {
            if (!_children.Contains(child))
            {
                if (AppSettings.Rnd.Next(3) > 1)
                    _children.Add(child);
                else
                    _children.Insert(0,child);
                child.Parent = this;
                NotifyPropertyChanged("Children");
            }
        }

        public void RemoveChild(EcclesiaViewModel child)
        {
            if (_children.Contains(child))
            {
                child.Parent = null;
                _children.Remove(child);
                child = null; 
                NotifyPropertyChanged("Children");
            }
        }

        public virtual void ClearChildren()
        {
            var temp = new List<EcclesiaViewModel>();
            temp.AddRange(_children);
            foreach (var vm in temp)
                RemoveChild(vm);
            temp.Clear();
            temp = null;
        }
        #endregion
    }
}
