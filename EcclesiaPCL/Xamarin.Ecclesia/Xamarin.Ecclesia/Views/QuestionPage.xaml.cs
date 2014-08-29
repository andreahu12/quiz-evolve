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
    public partial class QuestionPage : BaseView
    {
        public QuestionPage()
        {
            //Initialize UI
            InitializeComponent();
            
        }

        #region Navigation
        //Any event handlers attach here before we navigate to thispage
        public override void AttachNavigationEvents()
        {
            BindingContext = CommonActions.ActiveQuestion;
        }

        //detach event handlers, on navigating out of the page
        public override void DetachNavigationEvents()
        {
            BindingContext = null;
        }
        #endregion

        #region Controls Handlers

        async public void ItemTapped(object sender, ItemTappedEventArgs args)
        {
            ((AnswerViewModel)args.Item).CheckAnswer();
            if (CommonActions.ActiveQuestion.HasNextQuestion)
            {
                CommonActions.ActiveQuestion = CommonActions.ActiveQuestion.NextQuestion;
                await Navigation.PushAsync(new QuestionPage());
            }
        }
        #endregion
    }
}
