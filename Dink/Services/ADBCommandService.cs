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

            Console.WriteLine("The device responded:");
            Console.WriteLine(receiver.ToString());
        }
    }
}
