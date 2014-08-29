using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Ecclesia.Settings;
using Xamarin.Forms;


namespace Xamarin.Ecclesia.ViewModels
{
    public class InstructionsViewModel : EcclesiaViewModel
    {
        #region Constructor
        public InstructionsViewModel()
        {
            Title = "Instructions";
            BackgroundColor = AppSettings.PageBackgroundColor;
            Description = "Instruction text. Instruction text. instruction text. instruction text. instruction text. Instruction text. Instruction text. instruction text. instruction text. instruction text.";
        }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        public string Description
        { get; private set; }
        #endregion

        #region Methods
        
        #endregion
    }
}
