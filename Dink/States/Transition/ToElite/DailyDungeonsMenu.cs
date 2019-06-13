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
    class DailyDungeonMenu : State
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public DailyDungeonMenu(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:daily_dungeon_menus:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:daily_dungeon_menus:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:daily_dungeon_menus:color"],
                X = x,
                Y = y
            };

            Next = null;
        }

        public override bool IsState(DeviceData device)
        {
            if (ADBCommandService.CheckPixel(device, Sig.X, Sig.Y, Sig.Color))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>


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
