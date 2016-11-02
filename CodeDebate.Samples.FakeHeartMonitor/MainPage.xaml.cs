using System;
using System.Net.Http;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using CodeDebate.Samples.FakeHeartMonitor.Entities;
using Newtonsoft.Json;

namespace CodeDebate.Samples.FakeHeartMonitor
{
    public sealed partial class MainPage : Page
    {
        private const string SharedAccessSignature = "PLACE HOLDER";

        private const string EventHubRestUri = "PLACE HOLDER";

        private readonly DispatcherTimer _dtimer;
        private bool _running;

        public MainPage()
        {
            InitializeComponent();

            _dtimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(1)};

            _dtimer.Tick += Dtimer_Tick;
        }

        private void Dtimer_Tick(object sender, object e)
        {
            var random = new Random();

            var patientA = new HeartBeatRateReading
            {
                PatientIdentifier = "Patient A",
                HeartBeatRateValue = random.Next(80, 95)
            };

            var patientB = new HeartBeatRateReading
            {
                PatientIdentifier = "Patient B",
                HeartBeatRateValue = random.Next(80, 95)
            };

            var patientC = new HeartBeatRateReading
            {
                PatientIdentifier = "Patient C",
                HeartBeatRateValue = random.Next(80, 95)
            };

            PatientAHeartRateLabel.Text = patientA.HeartBeatRateValue.ToString();
            PatientBHeartRateLabel.Text = patientB.HeartBeatRateValue.ToString();
            PatientCHeartRateLabel.Text = patientC.HeartBeatRateValue.ToString();

            PublishEvent(JsonConvert.SerializeObject(patientA));
            PublishEvent(JsonConvert.SerializeObject(patientB));
            PublishEvent(JsonConvert.SerializeObject(patientC));
        }

        private void StartStopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _running = _running != true;

            if (_running)
                _dtimer.Start();
            else
                _dtimer.Stop();
        }

        private static void PublishEvent(string jsonContent)
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", SharedAccessSignature);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            content.Headers.Add("ContentType", "application/json");

            httpClient.PostAsync(EventHubRestUri, content);
        }
    }
}