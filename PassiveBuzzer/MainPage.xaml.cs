using GpioConfiguration;
using System;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace PassiveBuzzer
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GPIO _gpio = new GPIO();
        DispatcherTimer _timer = new DispatcherTimer();
        int _passivebuzzersensor = 5; // define the tilt switch sensor interfaces
        int _val = 0;// define numeric variables val


        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _gpio.InitGPIO(_passivebuzzersensor);
            SetSensor();
            _timer.Interval = TimeSpan.FromMilliseconds(0.1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            ReadVal();
        }

        private void SetSensor()
        {
            _gpio._pin[0].SetDriveMode(GpioPinDriveMode.Output);
            _gpio._pin[0].Write(GpioPinValue.Low);
        }

        private void ReadVal()
        {
            for (var i = 0; i < 1000; i++) // Wen a frequency sound
            {
                _gpio._pin[0].Write(GpioPinValue.High);
                Task.Delay(5).Wait();
                _gpio._pin[0].Write(GpioPinValue.Low);
                Task.Delay(5).Wait();
            }      
        }

        private void btnSound_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

