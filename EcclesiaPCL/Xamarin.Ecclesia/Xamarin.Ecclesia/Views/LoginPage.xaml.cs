using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.ViewModels;
using Xamarin.Forms;

namespace Xamarin.Ecclesia.Views
{
    public partial class LoginPage:BaseView
    {
        public LoginPage()
        {
            //Initialize UI
            InitializeComponent();
            //Set viewmodel
            BindingContext = ViewModelProvider.GetViewModel<LoginViewModel>();

        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public override void AttachNavigationEvents()
        {
            
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
        }
        #endregion

        #region Controls Handlers
        async void OnButtonClicked(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new InstructionsPage());
        }
        #endregion
    }
}
