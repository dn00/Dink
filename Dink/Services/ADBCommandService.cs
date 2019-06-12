using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Services
{
    public static class ADBCommandService
    {
        public static void SendClick(DeviceData device, uint x, uint y)
        {
            var receiver = new ConsoleOutputReceiver();

            AdbClient.Instance.ExecuteRemoteCommand(String.Format($"input tap {x} {y}"), device, receiver);

            Console.WriteLine($"Click sent to {x} {y}");
            //Console.WriteLine(receiver.ToString());
        }

        public static bool CheckPixel(DeviceData device, uint x, uint y, String pixelColor)
        {
            var receiver = new ConsoleOutputReceiver();
            uint skip = 1280 * y + x;

            String command = $"cd /data; screencap screen.dump; dd if='screen.dump' bs=4 count=1 skip={skip} 2>/dev/null";
            AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);

            string result = StringToHex(receiver.ToString()).Substring(0,6);
            Console.WriteLine("Looking for " + pixelColor + ". Found " + result);
            if (result.Equals(pixelColor)) {
                return true;
            }
            return false;
        }

        private static string StringToHex(string hexstring)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char t in hexstring)
            {
                //Note: X for upper, x for lower case letters
                sb.Append(Convert.ToInt32(t).ToString("x"));
            }
            return sb.ToString();
        }
    }
}
