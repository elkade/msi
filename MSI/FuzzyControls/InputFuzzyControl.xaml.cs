using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using MSI.Annotations;

namespace MSI.FuzzyControls
{
    /// <summary>
    /// Interaction logic for InputFuzzyControl.xaml
    /// </summary>
    public partial class InputFuzzyControl : UserControl, INotifyPropertyChanged
    {
        private string _min;
        private string _max;

        public string Min
        {
            get { return _min; }
            set
            {
                _min = value;
                double d;
                if (Double.TryParse(_min, out d))
                {
                    PositiveControl.Min = d;
                    NegativeControl.Min = d;
                }
                OnPropertyChanged();
            }
        }

        public string Max
        {
            get { return _max; }
            set
            {
                _max = value;
                double d;
                if (Double.TryParse(_max, out d))
                {
                    PositiveControl.Max = d;
                    NegativeControl.Max = d;
                }
                OnPropertyChanged();
            }
        }

        public InputFuzzyControl()
        {
            InitializeComponent();
            MinTextBox.Text = Min;
            MaxTextBox.Text = Max;
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
        public void Redraw()
        {
            NegativeControl.UpdateChart();
            PositiveControl.UpdateChart();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
