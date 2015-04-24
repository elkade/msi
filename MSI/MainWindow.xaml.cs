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
            VisibilityInput.Text = "900";
            TemperatureInput.Text = "20";
            HourInput.Text = "15";
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

                _fr = new FuzzyReasoning(RainControl.Parameter,TemperatureControl.Parameter, VisibilityControl.Parameter,
                    HourControl.Parameter);

                return _fr.Work(rain, temperature, visibility, hour).ToString(); //,temperature,hour);

            });
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RainControl.Redraw();
            TemperatureControl.Redraw();
            VisibilityControl.Redraw();
            HourControl.Redraw();
        }
    }
}
