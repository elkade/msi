using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
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

        public ContinuousDimension Dimension { get; set; }

        private FuzzySet _fuzzySet;
        private double _max;
        private double _mid;
        private double _min;
        private double _top;
        private bool _isLeft;

        public bool ShallUpdateChart = true;

        public bool IsLeft
        {
            get { return _isLeft; }
            set
            {
                _isLeft = value;
                SetParams();
            }
        }

        public FuzzyControl()
        {
            _max = 100;
            _min = 0;
            IsLeft = false;
            Dimension = new ContinuousDimension("Name", "Description", "Unit", (decimal)_min, (decimal)_max);
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

        /// <summary>
        /// Gets or sets the maximum.
        /// </summary>
        /// <value>
        /// W jednostkach prawdziwych - nie sliderowych
        /// </value>
        public double Max
        {
            get { return _max; }
            set
            {
                _max = value;
                SetParams();
                UpdateChart();
            }
        }

        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                SetParams();
                UpdateChart();
            }
        }

        private void SetParams()
        {
            if (!IsLeft)
            {
                _top = Min;
                _mid = (Max + Min)/2;
                _bot = Max;
                OnPropertyChanged("Top");
                OnPropertyChanged("Bot");
            }
            else
            {
                _top = Max;
                _mid = (Max + Min) / 2;
                _bot = Min;
                OnPropertyChanged("Top");
                OnPropertyChanged("Bot");
            }
        }

        public double Top
        {
            get { return 10 * (_top-Min) / (Max - Min); }
            set
            {
                _top = Min + (Max - Min)*(value/10);
                Debug.WriteLine(Min + "   " + _top);
                _mid = (_top + _bot)/2;
                UpdateChart();
            }
        }
        /// <summary>
        /// Gets or sets the bot.
        /// </summary>
        /// <value>
        /// zbindowany ze sliderem - przyjmuje i zwraca wartości sliderowe
        /// </value>
        public double Bot
        {
            get { return 10 * (_bot-Min) / (Max - Min); }
            set
            {
                _bot = Min + (Max - Min) * (value / 10);
                Debug.WriteLine(Min + "   " +_bot);
                _mid = (_top + _bot) / 2;
                UpdateChart();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateChart()
        {
            if (!ShallUpdateChart)
                return;
            if (_min >= _max)
                return;
            //_dimension = new ContinuousDimension(FuzzyName, Description, Unit, (decimal)_min, (decimal)_max);

            if(IsLeft)
            _fuzzySet = new LeftQuadraticSet(Dimension, FuzzyName, (decimal)_bot, (decimal)_mid, (decimal)_top);
            else
                _fuzzySet = new RightQuadraticSet(Dimension, FuzzyName, (decimal)_top, (decimal)_mid, (decimal)_bot);

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
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            UpdateChart();
        }
    }
}