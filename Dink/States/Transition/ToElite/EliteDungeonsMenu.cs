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
    class EliteDungeonsMenu : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public EliteDungeonsMenu(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:elite_dungeons_menu:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:elite_dungeons_menu:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:elite_dungeons_menu:color"],
                X = x,
                Y = y
            };

            Next = null;
            
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
            if (Next != null)
            {
                ushort maxWaitSeconds = 100;

                Thread.Sleep(5000);
                if (!ADBCommandService.IsL2RRunning(device))
                {
                    return false;
                }

                ADBCommandService.SendClick(device, Sig.X, Sig.Y);

                while (maxWaitSeconds > 0)
                {
                    if (Next.IsState(device))
                    {
                        return true;
                    }
                    Thread.Sleep(1000);
                    --maxWaitSeconds;
                }
                return false;
            }

            return false;
        }
    }

}
