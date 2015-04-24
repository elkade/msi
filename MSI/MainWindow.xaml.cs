using System;
using System.Threading.Tasks;
using System.Windows;

namespace MSI
{
    public partial class MainWindow : Window
    {
        private FuzzyReasoning _fr;

        public MainWindow()
        {
            InitializeComponent();
            //Relation.Text = "((tall & !weighty) & goodForBasket) | (!(tall & !weighty) & !goodForBasket)";
            RainInput.Text = "180";
            VisibilityInput.Text = "0";
            TemperatureInput.Text = "80";
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

                _fr = new FuzzyReasoning(RainControl.Parameter, null, null, null/*,TemperatureControl.Parameter, VisibilityControl.Parameter,
                    HourControl.Parameter*/);

                return _fr.Work(rain, temperature).ToString(); //,temperature,hour);

            });
        }
    }
}
