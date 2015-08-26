using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using System;
using RotaryEncoder.Classes;
using Windows.UI.Xaml.Navigation;


// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace RotaryEncoder
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int _pinDT = 5;  // Connected to DT on KY-040
        int _pinCLK = 6;  // Connected to CLK on KY-040
        int _encoderPos = 0;
        int _pinLastDT;
        int _OldVal;
        GPIO _gpio = new GPIO();
        DispatcherTimer _timer = new DispatcherTimer();
        
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _gpio.InitGPIO(_pinDT, _pinCLK);
            SetSensor();
            tbkPosizione.Text = _encoderPos.ToString();
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
            _gpio._pin[0].SetDriveMode(GpioPinDriveMode.Input);
            _gpio._pin[1].SetDriveMode(GpioPinDriveMode.Input);        
            _pinLastDT = (int)_gpio._pin[0].Read();
        }

        private void ReadVal()
        {
            _OldVal = (int)_gpio._pin[0].Read();

            if (_OldVal != _pinLastDT)
            {
                if ((int)_gpio._pin[1].Read() != _OldVal)
                {
                    _encoderPos++;
                }

                else
                {
                    _encoderPos--;
                }
                tbkPosizione.Text = _encoderPos.ToString();
            }
            _pinLastDT = _OldVal;
        }

        private void btnTilt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

