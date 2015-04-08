using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Graphics;
using FuzzyFramework.Sets;

namespace MSI
{
    public partial class MainWindow : Window
    {
        private readonly FuzzyReasoning _fr;

        public MainWindow()
        {
            InitializeComponent();
            _fr = new FuzzyReasoning();
            //Relation.Text = "((tall & !weighty) & goodForBasket) | (!(tall & !weighty) & !goodForBasket)";

            RainInput.Text = "180";
            VisibilityInput.Text = "80";
            TemperatureInput.Text = "0";
            HourInput.Text = "0";
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            Output.Text = await Reasoning(RainInput.Text, VisibilityInput.Text, TemperatureInput.Text, HourInput.Text);
        }

        private async Task<string> Reasoning(string rainInput, string visibilityInput, string temperatureInput, string hourInput)
        {
            return await Task.Run(() =>
            {
                decimal rain;
                decimal visibility;
                decimal temperature;
                decimal hour;

                if (!Decimal.TryParse(rainInput, out rain))
                    return String.Empty;
                if (!Decimal.TryParse(visibilityInput, out visibility))
                    return String.Empty;
                if (!Decimal.TryParse(temperatureInput, out temperature))
                    return String.Empty;
                if (!Decimal.TryParse(hourInput, out hour))
                    return String.Empty;

                return _fr.Work(rain, visibility).ToString(); //,temperature,hour);

            });
        }

        private void RangeBase_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var pictureBoxBuyIt = RainWfh.Child as PictureBox;

            ContinuousDimension height = new ContinuousDimension("Height", "Personal height", "cm", 100, 250);

            //Debug.WriteLine(e.NewValue);

            decimal top = (decimal) (100 + e.NewValue * 15);

            top = top <= 180 ? 181 : top;

            FuzzySet tall = new LeftQuadraticSet(height, "Tall person", 150, 180, top);

            RelationImage imgBuyIt = new RelationImage(tall);

            Bitmap bmpBuyIt = new Bitmap(pictureBoxBuyIt.Width, pictureBoxBuyIt.Height);
            imgBuyIt.DrawImage(Graphics.FromImage(bmpBuyIt));
            pictureBoxBuyIt.Image = bmpBuyIt;
        }
    }
}
