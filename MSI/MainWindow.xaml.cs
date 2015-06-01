using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using FuzzyFramework.Defuzzification;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Graphics;

namespace MSI
{
    public enum DefuzMethod
    {
        //CenterOfGravity = 0,
        LeftOfMaximum = 0,
        RightOfMaximum = 1,
        MeanOfMaximum = 2,
    }
    public partial class MainWindow : Window
    {


        public DefuzMethod Method { get; set; }

        private FuzzyReasoning _fr;
        private ContinuousDimension CondDimension;
        public MainWindow()
        {
            InitializeComponent();
            //Relation.Text = "((tall & !weighty) & goodForBasket) | (!(tall & !weighty) & !goodForBasket)";
            RainControl.Value = 5;
            FogControl.Value = 5;
            TemperatureControl.Value = 5;
            //DarknessControl.Value = 5;
            CondDimension = new ContinuousDimension("", "", "", 0, 10);
            GoodCondControl.Dimension = CondDimension;
            BadCondControl.Dimension = CondDimension;
            GoodCondControl.Max = 10;
            GoodCondControl.Min = 0;
            BadCondControl.Max = 10;
            BadCondControl.Min = 0;
        }

        private bool _isProcessing = false;
        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isProcessing)
                return;
            _isProcessing = true;
            Output.Text =
                await
                    Reasoning(RainControl.Value.ToString(),TemperatureControl.Value.ToString(), FogControl.Value.ToString()
                        /*, DarknessControl.Value.ToString()*/);
            if (!String.IsNullOrEmpty(Output.Text))
            {
                double result = Double.Parse(Output.Text);
                if (result < 2)
                    Interpretation.Text = "   Na drodze panują fatalne warunki.";
                else if (result < 4)
                    Interpretation.Text = "   Na drodze panują złe warunki.";
                else if (result < 6)
                    Interpretation.Text = "   Na drodze panują średnie warunki.";
                else if (result < 8)
                    Interpretation.Text = "   Na drodze panują dobre warunki.";
                else
                    Interpretation.Text = "   Na drodze panują znakomite warunki.";
            }
            _isProcessing = false;
        }


        private async Task<string> Reasoning(string rainInput, string temperatureInput, string fogInput/*, string darknessInput*/)
        {
            Debug.WriteLine(rainInput);
            Debug.WriteLine(fogInput);
            Debug.WriteLine(temperatureInput);
            //Debug.WriteLine(darknessInput);
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
               // if (!Decimal.TryParse(darknessInput, out darkness))
                //    return String.Empty;
                var condParameter = new FuzzyParameter
                {
                    PositiveSet = GoodCondControl.FuzzySet,
                    NegativeSet = BadCondControl.FuzzySet,
                    Dimension = CondDimension,
                };
                _fr = new FuzzyReasoning(RainControl.Parameter,TemperatureControl.Parameter, FogControl.Parameter,
                    /*DarknessControl.Parameter,*/ condParameter);

                var r =  _fr.Work(rain, temperature, fog/*, darkness*/, (DefuzzificationFactory.DefuzzificationMethod)(Method+1)); //,temperature,hour);
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
