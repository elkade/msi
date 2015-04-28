using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MSI
{
    public partial class MainWindow : Window
    {
        private FuzzyReasoning _fr;

        public MainWindow()
        {
            InitializeComponent();
            //Relation.Text = "((tall & !weighty) & goodForBasket) | (!(tall & !weighty) & !goodForBasket)";
            RainInput.Text = "5";
            FogInput.Text = "5";
            TemperatureInput.Text = "5";
            DarknessInput.Text = "5";
        }

        private async void Submit_OnClick(object sender, RoutedEventArgs e)
        {
            Output.Text = await Reasoning(RainInput.Text, FogInput.Text, TemperatureInput.Text, DarknessInput.Text);
        }

        private async Task<string> Reasoning(string rainInput, string fogInput, string temperatureInput, string darknessInput)
        {
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

                return _fr.Work(rain, temperature, fog, darkness).ToString(); //,temperature,hour);

            });
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RainControl.Redraw();
            TemperatureControl.Redraw();
            FogControl.Redraw();
            DarknessControl.Redraw();
        }
    }
}
