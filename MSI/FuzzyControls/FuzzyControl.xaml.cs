using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Graphics;
using FuzzyFramework.Sets;
using MSI.Annotations;
using UserControl = System.Windows.Controls.UserControl;

namespace MSI
{
    /// <summary>
    ///     Interaction logic for FuzzyControlBoth.xaml
    /// </summary>
    public partial class FuzzyControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MinProperty = DependencyProperty.Register(
            "Min", typeof (double), typeof (FuzzyControl), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register(
            "Max", typeof (double), typeof (FuzzyControl), new PropertyMetadata(default(double)));

        private double _bot;


        private ContinuousDimension _dimension;

        private FuzzySet _fuzzySet;
        private double _max;
        private double _mid;
        private double _min;
        private double _top;

        public bool IsLeft { get; set; }

        public FuzzyControl()
        {
            IsLeft = false;

            _max = 100;
            _min = 0;

            _top = Min;
            _mid = (Max + Min)/2;
            _bot = Max;

            InitializeComponent();

            UpdateChart();
        }

        public FuzzySet FuzzySet
        {
            get { return _fuzzySet; }
        }

        public string Description { get; set; }

        public string FuzzyName { get; set; }

        public string Unit { get; set; }

        public double Max
        {
            get { return _max; }
            set
            {
                if (value <= _min || value < double.Epsilon)
                    return;
                _max = value;
                _top = _top >= _max ? _max - 1 : _top;
                _mid = _mid >= _max ? _max - 1 : _mid;
                _bot = _bot >= _max ? _max - 1 : _bot;
                _top = _top < _min ? _min : _top;
                _mid = _mid < _top ? _top + 1 : _mid;
                _bot = _bot < _mid ? _mid + 1 : _bot;
                OnPropertyChanged("Top");
                OnPropertyChanged("Mid");
                OnPropertyChanged("Bot");
                OnPropertyChanged();
                UpdateChart();
            }
        }

        public double Min
        {
            get { return _min; }
            set
            {
                if (value >= _max)
                    return;
                _max = value;
                _top = _top <= _min ? _min + 1 : _top;
                _mid = _mid <= _min ? _min + 2 : _mid;
                _bot = _bot <= _min ? _min + 2 : _bot;
                _top = _top > _max ? _max - 1 : _top;
                _mid = _mid > _top ? _top - 1 : _mid;
                _bot = _bot > _mid ? _mid - 1 : _bot;
                OnPropertyChanged("Top");
                OnPropertyChanged("Mid");
                OnPropertyChanged("Bot");
                OnPropertyChanged();
                UpdateChart();
            }
        }

        public double Top
        {
            get { return 10*_top/(Max - Min); }
            set
            {
                _top = Min + (Max - Min)*(value/10);
                _top = _top >= _mid ? _mid - 1 : _top;
                UpdateChart();
            }
        }

        public double Mid
        {
            get { return 10*_mid/(Max - Min); }
            set
            {
                _mid = Min + (Max - Min)*(value/10);
                _mid = _mid <= _top ? _top + 1 : _mid;
                _mid = _mid >= _bot ? _bot - 1 : _mid;
                UpdateChart();
            }
        }

        public double Bot
        {
            get { return 10*_bot/(Max - Min); }
            set
            {
                _bot = Min + (Max - Min)*(value/10);
                _bot = _bot <= _mid ? _mid + 1 : _bot;
                UpdateChart();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateChart()
        {
            if (_min >= _max)
                return;
            _dimension = new ContinuousDimension(FuzzyName, Description, Unit, (decimal)_min, (decimal)_max);

            if (_top == _mid || _mid == _bot)
                return;
            if(IsLeft)
            _fuzzySet = new LeftQuadraticSet(_dimension, FuzzyName, (decimal) _top, (decimal) _mid, (decimal) _bot);
            else
                _fuzzySet = new RightQuadraticSet(_dimension, FuzzyName, (decimal)_top, (decimal)_mid, (decimal)_bot);

            //_fuzzySet = new RightQuadraticSet(_dimension, FuzzyName, Top, Mid,Bot);
            var chart = (PictureBox) Wfh.Child;
            var imgBuyIt = new RelationImage(_fuzzySet);
            var bmpBuyIt = new Bitmap(chart.Width, chart.Height);
            imgBuyIt.DrawImage(Graphics.FromImage(bmpBuyIt));
            chart.Image = bmpBuyIt;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}