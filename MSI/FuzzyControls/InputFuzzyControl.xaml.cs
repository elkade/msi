using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using FuzzyFramework.Dimensions;
using MSI.Annotations;

namespace MSI.FuzzyControls
{
    /// <summary>
    /// Interaction logic for InputFuzzyControl.xaml
    /// </summary>
    public partial class InputFuzzyControl : UserControl, INotifyPropertyChanged
    {

        private double _value;

        public double SliderValue
        {
            get { return 10 * (Value - Min) / (Max - Min); }
            set { OnPropertyChanged("Value"); Value = Min + (Max - Min) * (value / 10); }
        }

        public double Value
        {
            get { return Math.Round(_value, 2); }
            set { _value = value; OnPropertyChanged("SliderValue"); }
        }


        private double _min;
        private double _max;

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                SliderValue = 5;
                //double d;
                //if (Double.TryParse(_min, out d))
                //{
                UpdateSubcontrols();
                //}
                SliderValue = 5;
                OnPropertyChanged();
            }
        }

        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                SliderValue = 5;
                //double d;
                //if (Double.TryParse(_max, out d))
                //{
                UpdateSubcontrols();
                SliderValue = 5;
                //}
                OnPropertyChanged();
            }
        }

        public ContinuousDimension Dimension { get; set; }
        private void UpdateSubcontrols()
        {
            if (_min < _max)
            {
                Dimension = new ContinuousDimension("", "", "", (decimal)_min, (decimal)_max);
                PositiveControl.Dimension = Dimension;
                NegativeControl.Dimension = Dimension;
            }
            PositiveControl.ShallUpdateChart = false;
            NegativeControl.ShallUpdateChart = false;
            PositiveControl.Max = _max;
            NegativeControl.Max = _max;
            PositiveControl.Min = _min;
            NegativeControl.Min = _min;
            PositiveControl.ShallUpdateChart = true;
            NegativeControl.ShallUpdateChart = true;
            PositiveControl.UpdateChart();
            NegativeControl.UpdateChart();
        }

        public InputFuzzyControl()
        {
            InitializeComponent();
            MinTextBox.Text = Min.ToString();
            MaxTextBox.Text = Max.ToString();
        }
        public FuzzyParameter Parameter
        {
            get
            {
                return new FuzzyParameter
                {
                    PositiveSet = PositiveControl.FuzzySet,
                    NegativeSet = NegativeControl.FuzzySet,
                    Dimension = Dimension,
                };
            }
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
