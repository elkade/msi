using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Graphics;
using FuzzyFramework.Sets;
using TextBox = System.Windows.Controls.TextBox;
using UserControl = System.Windows.Controls.UserControl;

namespace MSI
{
    /// <summary>
    /// Interaction logic for FuzzyControlBoth.xaml
    /// </summary>
    public partial class FuzzyControlLeft : UserControl
    {
        public FuzzyControlLeft()
        {
            InitializeComponent();
            Max = 100;
            Min = 0;
            _leftTop = (decimal)Min;
            _leftMid = (decimal)(Max + Min) / 2;
            _leftBot = (decimal)Max;

            UpdateChart();
        }

        private ContinuousDimension _dimension;

        public FuzzySet FuzzySet { get { return _fuzzySet; }}

        private FuzzySet _fuzzySet;

        public string Description { get; set; }

        public string FuzzyName { get; set; }

        public string Unit { get; set; }

        public double Max { get; set; }

        public double Min { get; set; }

        private void UpdateChart()
        {
            try
            {
                _dimension = new ContinuousDimension(FuzzyName, Description, Unit, (decimal)Min, (decimal)Max);

                _fuzzySet = new LeftQuadraticSet(_dimension, FuzzyName, LeftBot, LeftMid, LeftTop);
                //_fuzzySet = new RightQuadraticSet(_dimension, FuzzyName, Top, Mid,Bot);
                var chart = (PictureBox)Wfh.Child;
                RelationImage imgBuyIt = new RelationImage(_fuzzySet);
                Bitmap bmpBuyIt = new Bitmap(chart.Width, chart.Height);
                imgBuyIt.DrawImage(Graphics.FromImage(bmpBuyIt));
                chart.Image = bmpBuyIt;
            }
            catch
            {

            }
        }

        private void LeftTopSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LeftTop = (decimal)e.NewValue;
            UpdateChart();
        }

        private void LeftMiddleSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LeftMid = (decimal)e.NewValue;

            UpdateChart();
        }

        private void LeftBottomSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LeftBot = (decimal)e.NewValue;

            UpdateChart();
        }


        private decimal _leftTop;

        public decimal LeftTop
        {
            get { return _leftTop; }
            set
            {
                _leftTop = (decimal)Min + (decimal)(Max - Min) * (value / 10);
                _leftTop = _leftTop <= _leftMid ? _leftMid + 1 : _leftTop;
            }
        }

        private decimal _leftMid;

        public decimal LeftMid
        {
            get { return _leftMid; }
            set
            {
                _leftMid = (decimal)Min + (decimal)(Max - Min) * (value / 10);
                _leftMid = _leftMid >= _leftTop ? _leftTop - 1 : _leftMid;
                _leftMid = _leftMid <= _leftBot ? _leftBot + 1 : _leftMid;
            }
        }

        private decimal _leftBot;

        public decimal LeftBot
        {
            get { return _leftBot; }
            set
            {
                _leftBot = (decimal)Min + (decimal)(Max - Min) * (value / 10);
                _leftBot = _leftBot >= _leftMid ? _leftMid - 1 : _leftBot;
            }
        }

        private void MaxTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            double d;
            if (Double.TryParse(((TextBox)sender).Text, out d))
                Max = d;
            UpdateChart();
        }
    }
}
