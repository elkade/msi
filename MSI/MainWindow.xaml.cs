using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using FuzzyFramework.Graphics;

namespace MSI
{
    public partial class MainWindow : Window
    {
        private FuzzyReasoning _fr;

        public MainWindow()
        {
            InitializeComponent();
            //Relation.Text = "((tall & !weighty) & goodForBasket) | (!(tall & !weighty) & !goodForBasket)";
            RainControl.Value = 5;
            FogControl.Value = 5;
            TemperatureControl.Value = 5;
            DarknessControl.Value = 5;
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            Output.Text = await Reasoning(RainControl.Value.ToString(), FogControl.Value.ToString(), TemperatureControl.Value.ToString(), DarknessControl.Value.ToString());
        }

        private async Task<string> Reasoning(string rainInput, string fogInput, string temperatureInput, string darknessInput)
        {
            Debug.WriteLine(rainInput);
            Debug.WriteLine(fogInput);
            Debug.WriteLine(temperatureInput);
            Debug.WriteLine(darknessInput);
            return await Task.Run(() =>
            {
                decimal rain;
                decimal fog;
                decimal temperature;
                decimal darkness;

                if (!Decimal.TryParse(rainInput, out rain))
                    return String.Empty;
                if (!Decimal.TryParse(fogInput, out fog))
                    return String.Empty;
                if (!Decimal.TryParse(temperatureInput, out temperature))
                    return String.Empty;
                if (!Decimal.TryParse(darknessInput, out darkness))
                    return String.Empty;

                _fr = new FuzzyReasoning(RainControl.Parameter,TemperatureControl.Parameter, FogControl.Parameter,
                    DarknessControl.Parameter);

                var r =  _fr.Work(rain, temperature, fog, darkness); //,temperature,hour);
                var chart = (PictureBox)Wfh.Child;
                var imgBuyIt = new RelationImage(r.Relation,r.Inputs,r.OutputDimension);
                var bmpBuyIt = new Bitmap(chart.Width, chart.Height);
                imgBuyIt.DrawImage(Graphics.FromImage(bmpBuyIt));
                chart.Image = bmpBuyIt;
                return r.CrispValue.ToString();
            });
        }
    }
}
