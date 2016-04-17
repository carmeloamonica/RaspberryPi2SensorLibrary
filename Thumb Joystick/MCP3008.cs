using System;
using System.Globalization;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Thumb_Joystick
{
    public class Mcp3008
    {
        SpiDevice _device;
        string _serialcomunication;
        string _choicechannel;
        const int Resolutionbits = 10;
        const int Shiftbyte = 8;


        byte[] _ch0;
        byte[] _ch1;
        byte[] _ch2;
        byte[] _ch3;
        byte[] _ch4;
        byte[] _ch5;
        byte[] _ch6;
        byte[] _ch7;
        byte[] _datareceived = {0, 0, 0};
        byte[] _datareceived1 = { 0, 0, 0 };


        public async void InitializeMcp3008(SerialComunication serialcomunication, SpiComunication spicomunication, SpiMode mode)
        {
            _serialcomunication = serialcomunication.ToString();           
            var spiconnectionsettings = new SpiConnectionSettings((int) spicomunication)
            {
                ClockFrequency = 1650000,
                Mode = mode
            };


            var spiDevice = SpiDevice.GetDeviceSelector(spicomunication.ToString());
            var deviceInformation = await DeviceInformation.FindAllAsync(spiDevice);


            if (deviceInformation != null && deviceInformation.Count > 0)
            {
                _device = await SpiDevice.FromIdAsync(deviceInformation[0].Id, spiconnectionsettings);
            }

            else
            {
                var dialog = new MessageDialog("Device not found");
                await dialog.ShowAsync();
            }
        }

        public double SingleEndedResult(Channel channel)
        {
            _choicechannel = channel.ToString();
            _ch0 = new byte[] { 1, 0x80, 0 };
            _ch1 = new byte[] { 1, 0x90, 0 };
            _ch2 = new byte[] { 1, 0xA0, 0 };
            _ch3 = new byte[] { 1, 0xB0, 0 };
            _ch4 = new byte[] { 1, 0xC0, 0 };
            _ch5 = new byte[] { 1, 0xD0, 0 };
            _ch6 = new byte[] { 1, 0xE0, 0 };
            _ch7 = new byte[] { 1, 0xF0, 0 };

            switch (_choicechannel)
            {
                case "Ch0":
                    _device.TransferFullDuplex(_ch0, _datareceived);
                    break;

                case "Ch1":
                    _device.TransferFullDuplex(_ch1, _datareceived);
                    break;

                case "Ch2":
                    _device.TransferFullDuplex(_ch2, _datareceived);
                    break;

                case "Ch3":
                    _device.TransferFullDuplex(_ch3, _datareceived);
                    break;

                case "Ch4":
                    _device.TransferFullDuplex(_ch4, _datareceived);
                    break;

                case "Ch5":
                    _device.TransferFullDuplex(_ch5, _datareceived);
                    break;

                case "Ch6":
                    _device.TransferFullDuplex(_ch6, _datareceived);
                    break;

                case "Ch7":
                    _device.TransferFullDuplex(_ch7, _datareceived);
                    break;
            }

            var result = ((_datareceived[1] & 0x03) << Shiftbyte) + _datareceived[2];
            var ohm = result / Resolutionbits;

            //var Ry = (float)(1023 - _DATARECEIVED[0]) * 10 / _DATARECEIVED[0];
            return ohm;
        }

        public void DifferentialResult(params TextBlock []textBlocks)
        {
            double[] result = {0, 0};
            double[] ohm = {0, 0};

            _ch0 = new byte[] { 1, 0x80, 0 };
            _ch1 = new byte[] { 1, 0x90, 0 };

            _device.TransferFullDuplex(_ch0, _datareceived);
            _device.TransferFullDuplex(_ch1, _datareceived1);

            result[0] = ((_datareceived[1] & 0x03) << Shiftbyte) + _datareceived[2];
            result[1] = ((_datareceived1[1] & 0x03) << Shiftbyte) + _datareceived1[2];

            ohm[0] = result[0] / Resolutionbits;
            ohm[1] = result[1] / Resolutionbits;

            textBlocks[0].Text = ohm[0].ToString(CultureInfo.InvariantCulture);
            textBlocks[1].Text = ohm[1].ToString(CultureInfo.InvariantCulture);
        }
    }
}

    public enum SerialComunication
    {
        SingleEnded,
        Differential
    }

    public enum Channel
    {
        Ch0,
        Ch1,
        Ch2,
        Ch3,
        Ch4,
        Ch5,
        Ch6,
        Ch7
    }

    public enum SpiComunication
    {
        Spi0,
        Spi1
    }
