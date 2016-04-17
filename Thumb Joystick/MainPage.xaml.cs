using System;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GpioConfiguration;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace Thumb_Joystick
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly GPIO _gpio = new GPIO();        
        readonly Mcp3008 _mcp3008 = new Mcp3008();
        readonly int _switchJoystick = 5; // define the tilt switch sensor interfaces
        int _val;

        public MainPage()
        {              
            InitializeComponent();
            var timer = new DispatcherTimer();
            _mcp3008.InitializeMcp3008(SerialComunication.SingleEnded,  SpiComunication.Spi0,  SpiMode.Mode0);
            _gpio.InitGPIO(_switchJoystick);
            SetSensor();
            timer.Interval = new TimeSpan(0, 0,0,0,1000);
            timer.Start();
            timer.Tick += _timer_Tick;
        }

        void _timer_Tick(object sender, object e)
        {
            ReadVal();
            //XCoordinate.Text = Math.Round(_mcp3008.SingleEndedResult(Channel.Ch0)).ToString(CultureInfo.InvariantCulture);
            _mcp3008.DifferentialResult(XCoordinate, YCoordinate);
        }

        private void SetSensor()
        {
            _gpio._pin[0].SetDriveMode(GpioPinDriveMode.Input);            
        }


        void ReadVal()
        {
            // digital interface will be assigned a value of 3 to read val
            _val = (int)_gpio._pin[0].Read();

            //When the tilt sensor detects a signal when the switch, LED flashes
            TblSwitch.Text = _val.Equals((int)GpioPinValue.Low) ? "Switch button pressed" : "Switch button unpressed";
        }
    }
}
