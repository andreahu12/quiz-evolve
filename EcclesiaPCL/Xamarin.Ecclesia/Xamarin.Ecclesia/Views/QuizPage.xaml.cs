using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Ecclesia.Utils;
using Xamarin.Ecclesia.ViewModels;
using Xamarin.Forms;

namespace Xamarin.Ecclesia.Views
{
    public partial class QuizPage : BaseView
    {
        public QuizPage()
        {
            //Initialize UI
            InitializeComponent();
            
        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public override void AttachNavigationEvents()
        {
            CommonActions.ActiveQuiz.LoadQuestionsFromParse();
            BindingContext = CommonActions.ActiveQuiz;
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
            BindingContext = null;
        }
        #endregion

        #region Controls Handlers
                
        public async void ItemTapped(object sender, ItemTappedEventArgs args)
        {
			var qvm = args.Item as QuestionViewModel;
			if (qvm.IsEnabled) {
				CommonActions.ActiveQuestion = qvm;
				await Navigation.PushAsync (new QuestionPage ());
			}

        }
        #endregion
    }
}
