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
    class StateCharSelectAD : State
    {
        public StateCharSelectAD(IConfiguration data) : base(data)
        {
            ushort.TryParse(_data["pixels:char_select_screen_ad:coords:x"], out ushort x);
            ushort.TryParse(_data["pixels:char_select_screen_ad:coords:y"], out ushort y);
            Sig = new Signifier
            {
                Color = _data["pixels:char_select_screen_ad:color"],
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

        public override bool Run(DeviceData device)
        {
            Thread.Sleep(2000);

            ushort maxWaitSeconds;

            ushort.TryParse(_data["macros:char_select_exit_ad:x"], out ushort x);
            ushort.TryParse(_data["macros:char_select_exit_ad:y"], out ushort y);

        OutAd:
            ADBCommandService.SendClick(device, x, y);
            maxWaitSeconds = 20;

            while (maxWaitSeconds > 0)
            {
                Thread.Sleep(1000);
                if (Next.IsState(device))
                {
                    return true;
                }
                else if (IsState(device))
                {
                    goto OutAd;
                }
                --maxWaitSeconds;
            }

            return false;


        }
    }
}
