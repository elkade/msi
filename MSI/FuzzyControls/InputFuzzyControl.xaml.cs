using System;
using System.Windows.Controls;

namespace MSI.FuzzyControls
{
    /// <summary>
    /// Interaction logic for InputFuzzyControl.xaml
    /// </summary>
    public partial class InputFuzzyControl : UserControl
    {
        public InputFuzzyControl()
        {
            InitializeComponent();
        }
        public FuzzyParameter Parameter
        {
            get
            {
                return new FuzzyParameter
                {
                    PositiveSet = PositiveControl.FuzzySet,
                    NegativeSet = NegativeControl.FuzzySet
                };
            }
        }

        private void MaxTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double d;
            if (Double.TryParse(((TextBox) sender).Text, out d))
            {
                PositiveControl.Max = d;
                NegativeControl.Max = d;
            }
        }
    }
}
