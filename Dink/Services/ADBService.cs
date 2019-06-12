using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Services
{
    public class ADBService
    {
        private IConfiguration _config { get; set; }
        private AdbServer _server { get; set; }
        public ADBService(IConfiguration config)
        {
            _config = config;
            _server = new AdbServer();
            var result = _server.StartServer($"{AppContext.BaseDirectory}\\platform-tools\\adb.exe", restartServerIfNewer: false);
        }

        public DeviceData getDevice(String serial)
        {
            var devices = AdbClient.Instance.GetDevices();

            foreach (var device in devices)
            {
                //var receiver = new ConsoleOutputReceiver();
                //AdbClient.Instance.ExecuteRemoteCommand("dumpsys iphonesubinfo2", device, receiver);
                Console.WriteLine(device.Serial.Substring(device.Serial.Length - 5) + ";;;;" + serial);
                if (device.Serial.Substring(device.Serial.Length - 5).Equals(serial)) {
                    return device;
                }
             }

            return null;
        }

        public void listDeviceSerial()
        {
            var devices = AdbClient.Instance.GetDevices();

            foreach (var device in devices)
            {
                //var receiver = new ConsoleOutputReceiver();
                //AdbClient.Instance.ExecuteRemoteCommand("dumpsys iphonesubinfo2", device, receiver);
                Console.WriteLine(device.Serial.Substring(device.Serial.Length - 5));
            }
        }
    }
}
