using System;
using Windows.UI.Xaml;

namespace GpioConfiguration
{
    public class ClockTime
    {
        DispatcherTimer _timer = new DispatcherTimer();
        public bool _timeElapsed { get; private set; }
        public bool _startStopClockTime { get; set; } 
        public int _interval { private get; set; }

        public void Clock()
        {
            _timeElapsed = false;
            _timer.Interval = TimeSpan.FromMilliseconds(.001);
            _timer.Tick += Timer_Tick;

            if(_startStopClockTime.Equals(true))
            {
                _timer.Start();
            }

            else
            {
                _timer.Stop();
            }
        }

        private void Timer_Tick(object sender, object e)
        {
            _timeElapsed = true;
        }
    }
}
