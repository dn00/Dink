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
    class StateLoginScreen : State
    {
        public StateLoginScreen(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:StateLoginScreen:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:StateLoginScreen:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:StateLoginScreen:color"],
                X = x,
                Y = y
            };

            Next = new StateCharSelect(data);
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
        /// Macro through the login screen
        /// </summary>
        /// <param name="device">ADB Device</param>
        /// <returns>The next State. Returns null if there's an error</returns>
        public override bool Run(DeviceData device)
        {
            Thread.Sleep(2000);

            ushort maxWaitSeconds;

        Login:
            maxWaitSeconds = 100;
            ushort.TryParse(_data["macros:login:x"], out ushort xLogin);
            ushort.TryParse(_data["macros:login:y"], out ushort yLogin);
            ADBCommandService.SendClick(device, xLogin, yLogin);

            while(maxWaitSeconds > 0)
            {
               Thread.Sleep(1000);
               if (Next.IsState(device))
               {
                    return true;
               } else if (IsState(device))
               {
                    goto Login;
               }
                --maxWaitSeconds;
            }

            return false;


        }
    }
}
