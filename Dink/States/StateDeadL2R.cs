using Dink.Actions;
using Dink.Services;
using Microsoft.Extensions.Configuration;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dink.States
{
    class StateDeadL2R : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public StateDeadL2R(IConfiguration data) : base(data)
        {
            Sig = null;
            Next = new StateLoginScreen(data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public override bool IsState(DeviceData device)
        {
            if (!ADBCommandService.IsL2RRunning(device))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///  L2R is dead and needs to be launched.
        /// </summary>
        /// <param name="device">ADB Device</param>
        /// <returns>The next state</returns>
        public override bool Run(DeviceData device)
        {
            ushort maxWaitSeconds = 100;

        Relaunch: 
            ADBCommandService.LaunchL2R(device);
            Thread.Sleep(5000);
            if (!ADBCommandService.IsL2RRunning(device))
            {
                goto Relaunch;
            }

            
            while (maxWaitSeconds > 0)
            {
                Console.WriteLine(" maxWaitSeconds DEADL2R");

                if (Next.IsState(device))
                {
                    Console.WriteLine("NEXT STATE DEADL2R");
                    return true;
                }
                Thread.Sleep(1000);
                --maxWaitSeconds;
            }

            maxWaitSeconds += 20;
            goto Relaunch;

        }

    }
}
