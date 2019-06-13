using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dink.Services
{
    public static class ADBCommandService
    {
        public static void SendClick(DeviceData device, ushort x, ushort y)
        {
            var receiver = new ConsoleOutputReceiver();

            AdbClient.Instance.ExecuteRemoteCommand(String.Format($"input tap {x} {y}"), device, receiver);

            Console.WriteLine($"Click sent to {x} {y}");
            //Console.WriteLine(receiver.ToString());
        }

        public static bool CheckPixel(DeviceData device, ushort x, ushort y, String pixelColor)
        {
            var receiver = new ConsoleOutputReceiver();
            uint offset = (uint)1280 * y + x;

            String command = $"cd /data; screencap screen.dump; dd if='screen.dump' bs=4 count=1 skip={offset} 2>/dev/null";
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

        private static String RunCommand(DeviceData device, String command)
        {
            var receiver = new ConsoleOutputReceiver();
            AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);
            return receiver.ToString();
        }

        public static bool IsL2RRunning(DeviceData device)
        {
            String command = "dumpsys activity | grep top-activity";
            String result = RunCommand(device, command);
            Console.WriteLine(result);

            if (result.IndexOf("netmarble") > -1)
            {
                return true;
            }
            return false;
        }

        public static bool LaunchL2R(DeviceData device)
        {
            String command = "am start -n com.netmarble.lin2ws/com.epicgames.ue4.GameActivity";
            String result = RunCommand(device, command);

            return true;
        }
        public static bool KillL2R(DeviceData device)
        {
            String command = "am force-stop com.netmarble.lin2ws";
            String result = RunCommand(device, command);

            return true;
        }
        public static bool SendTab(DeviceData device)
        {
            String command = "input keyevent 61";
            String result = RunCommand(device, command);

            return true;
        }


        public static void SwipeRight(DeviceData device)
        {

        }
    }
}
