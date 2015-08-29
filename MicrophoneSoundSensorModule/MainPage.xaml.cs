using GpioConfiguration;
using System;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace MicrophoneSoundSensorModule
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GPIO _gpio = new GPIO();
        DispatcherTimer _timer = new DispatcherTimer();
        int _microphoneSoundSensorModule = 5; // define the tilt switch sensor interfaces
        int _led = 6;// define LED Interface
        int _val = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _gpio.InitGPIO(_microphoneSoundSensorModule, _led);
            SetSensor();
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            ReadVal();
        }

        private void SetSensor()
        {
            _gpio._pin[0].SetDriveMode(GpioPinDriveMode.Input);
            _gpio._pin[1].SetDriveMode(GpioPinDriveMode.Output);
        }


        private void ReadVal()
        {
            // digital interface will be assigned a value of 3 to read val
            _val = (int)_gpio._pin[0].Read();

            //When the tilt sensor detects a signal when the switch, LED flashes
            if (_val.Equals((int)GpioPinValue.High))
            {
                _gpio._pin[1].Write(GpioPinValue.High);
            }

            else
            {
                _gpio._pin[1].Write(GpioPinValue.Low);
            }
        }
    }
}
