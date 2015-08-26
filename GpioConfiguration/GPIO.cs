using Windows.Devices.Gpio;

namespace GpioConfiguration
{
    public class GPIO
    {
        public GpioPin[] _pin { get; private set; }

        public GpioPin[] InitGPIO(params int[] pins)
        {
            var gpio = GpioController.GetDefault();
            var numberpins = pins.Length;
            _pin = new GpioPin[pins.Length];

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                return null;
            }

            for (var index = 0; index < numberpins; index++)
            {
                _pin[index] = gpio.OpenPin(pins[index]);
            }

            if (_pin != null)
            {
                return null;
            }

            return _pin;
        }
    }
}
