using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
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
        public static readonly DependencyProperty TitleNProperty = DependencyProperty.Register(
"TitleN", typeof(string), typeof(InputFuzzyControl), new PropertyMetadata(null, OnTitleNPropertyChanged));

        private static void OnTitleNPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fuzzyControl = d as InputFuzzyControl;
            if (fuzzyControl != null) fuzzyControl.TitleN = e.NewValue as string;

            // Code here to handle any work when the value has changed
        }
        public static readonly DependencyProperty TitlePProperty = DependencyProperty.Register(
"TitleP", typeof(string), typeof(InputFuzzyControl), new PropertyMetadata(null, OnTitlePPropertyChanged));

        private static void OnTitlePPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fuzzyControl = d as InputFuzzyControl;
            if (fuzzyControl != null) fuzzyControl.TitleP = e.NewValue as string;

            // Code here to handle any work when the value has changed
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
"Title", typeof(string), typeof(InputFuzzyControl), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged();}
        }

        public string TitleN
        {
            get { return (string)GetValue(TitleNProperty); }
            set { SetValue(TitleNProperty, value); }
        }

        public string TitleP
        {
            get { return _titleP; }
            set
            {
                if (value == null) return;_titleP = value; }
        }

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
        private string _title;
        private string _titleN;
        private string _titleP;

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
            if (TitleP!=null)
                PositiveControl.Title = TitleP;
            if (TitleN != null)
                NegativeControl.Title = TitleN;
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
        //protected override void OnRender(DrawingContext drawingContext)
        //{
        //    base.OnRender(drawingContext);
        //    UpdateSubcontrols();
        //}

    }
}
