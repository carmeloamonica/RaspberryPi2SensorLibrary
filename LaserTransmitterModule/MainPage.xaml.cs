using GpioConfiguration;
using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace LaserTransmitterModule
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GPIO _gpio = new GPIO();
        DispatcherTimer _timer = new DispatcherTimer();
        int _ledTransmitterModule = 5; // define the tilt switch sensor interfaces

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _gpio.InitGPIO(_ledTransmitterModule);
            SetSensor();
            _timer.Interval = TimeSpan.FromMilliseconds(.001);
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
        }


        private void ReadVal()
        {
            //When the tilt sensor detects a signal when the switch, LED flashes
            if (_gpio._pin[0].Read().Equals(GpioPinValue.Low))
            {
                _gpio._pin[0].Write(GpioPinValue.High);
                tblLed.Text = "On";
            }

            else
            {
                _gpio._pin[0].Write(GpioPinValue.Low);
                tblLed.Text = "Off";
            }
        }
    }
}
