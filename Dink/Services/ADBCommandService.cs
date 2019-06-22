using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dink.Services
{
    /// <summary>
    /// Higher levels methods for ADB
    /// </summary>
    public static class ADBCommandService
    {
        /// <summary>
        /// Send click to emulator instance
        /// </summary>
        /// <param name="device">The device data object</param>
        /// <param name="x">x value screen coordinate</param>
        /// <param name="y">y value screen coordinate</param>
        public static void SendClick(DeviceData device, ushort x, ushort y)
        {
            var receiver = new ConsoleOutputReceiver();

            AdbClient.Instance.ExecuteRemoteCommand(String.Format($"input tap {x} {y}"), device, receiver);

            Console.WriteLine($"Click sent to {x} {y}");
            //Console.WriteLine(receiver.ToString());
        }

        /// <summary>
        /// Send 3 clicks to emulator instance
        /// </summary>
        /// <param name="device">The device data object</param>
        /// <param name="x">x value screen coordinate</param>
        /// <param name="y">y value screen coordinate</param>
        public static void SendTripleClick(DeviceData device, ushort x, ushort y)
        {
            SendClick(device, x, y);
            Thread.Sleep(500);
            SendClick(device, x, y);
            Thread.Sleep(500);
            SendClick(device, x, y);
            Thread.Sleep(500);
        }

        /// <summary>
        /// Check if pixel color at x, y is the same as pixelColor
        /// </summary>
        /// <param name="device">The device data object</param>
        /// <param name="x">x value screen coordinate</param>
        /// <param name="y">y value screen coordinate</param>
        /// <param name="pixelColor">The hex color value of the pixel</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the hex string retrieved from ADB to color hex value
        /// </summary>
        /// <param name="hexstring">The hex string</param>
        /// <returns></returns>
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

        /// <summary>
        /// Run command with ADB on device
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <param name="command">Command to be run.</param>
        /// <returns></returns>
        private static String RunCommand(DeviceData device, String command)
        {
            var receiver = new ConsoleOutputReceiver();
            AdbClient.Instance.ExecuteRemoteCommand(command, device, receiver);
            return receiver.ToString();
        }

        /// <summary>
        /// Check if L2R is topmost activity in emulator
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Command to launch L2R
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
        public static bool LaunchL2R(DeviceData device)
        {
            String command = "am start -n com.netmarble.lin2ws/com.epicgames.ue4.GameActivity";
            String result = RunCommand(device, command);

            return true;
        }

        /// <summary>
        /// Command to kill L2R activity
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
        public static bool KillL2R(DeviceData device)
        {
            String command = "am force-stop com.netmarble.lin2ws";
            String result = RunCommand(device, command);

            return true;
        }

        /// <summary>
        /// Send a tab key event
        /// </summary>
        /// <param name="device">The device data object.</param>
        /// <returns></returns>
        public static bool SendTab(DeviceData device)
        {
            String command = "input keyevent 61";
            String result = RunCommand(device, command);

            return true;
        }

        /// <summary>
        /// Swipe right
        /// </summary>
        /// <param name="device">The device data object.</param>
        public static void SwipeRight(DeviceData device)
        {
            throw new NotImplementedException();
        }
    }
}
